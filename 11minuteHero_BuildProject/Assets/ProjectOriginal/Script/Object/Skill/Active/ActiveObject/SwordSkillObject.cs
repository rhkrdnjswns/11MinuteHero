using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillObject : ActiveObject
{
    public enum ESwordType
    {
        BloodSword,
        CurseSword,
        DevilSword
    }
    private Transform rotTransform; //자전할 오브젝트 트랜스폼 (회전 효과를 위한 참조)
    private Vector3 rotDirection; //자전 방향

    [SerializeField] private ParticleSystem particle; //파티클
    [SerializeField] private ESwordType eSwordType; //검 타입 (피로물든 검 / 저주받은 검)

    [SerializeField] private SwordSkillAfterImage[] afterImageArray; //잔상 참조 배열
    private Vector3 activePos; //생성 위치

    private float arrivalSecond;
    private float degree;
    private float attackRadius;
    private float rotateSpeed;

    private Collider[] collisionArray = new Collider[50];

    private WaitForSeconds attackDelay;
    private WaitForSeconds afterImageDelay;
#if UNITY_EDITOR
    public int index;
#endif
    public bool bEndRev { get; set; }
    public SwordSkillAfterImage[] GetAfterImage()
    {
        return afterImageArray;
    }
    public void InitObject(float distance, float attackInterval, float arrivalSecond, float degree, float rotSpeed)
    {
        switch (eSwordType)
        {
            case ESwordType.BloodSword:
                rotTransform = transform.GetChild(0).GetComponent<Transform>();
                rotDirection = Vector3.back;
                activePos = new Vector3(distance, 0.5f, distance);
                break;
            case ESwordType.CurseSword:
                rotTransform = transform;
                rotDirection = Vector3.down;
                activePos = new Vector3(-distance, 0.5f, -distance);
                break;
            case ESwordType.DevilSword:
                rotTransform = transform.GetChild(0).GetComponent<Transform>();
                rotDirection = Vector3.back;
                activePos = (Vector3.forward * distance) + (Vector3.up * 0.5f);
                break;
            default:
                Debug.LogError("UnDefined SwordType");
                break;
        }
        foreach (var item in afterImageArray)
        {
            item.SetAfterImage(eSwordType);
        }
        this.arrivalSecond = arrivalSecond;
        this.degree = degree;
        rotateSpeed = rotSpeed;

        afterImageDelay = new WaitForSeconds(0.1f);
        attackDelay = new WaitForSeconds(attackInterval);
    }
    public override void SetAttackRadius(float radius)
    {
        attackRadius = radius;
    }
    public void ActivateSkill(Transform revAxis)
    {
        transform.localPosition = activePos;
        foreach (var item in afterImageArray)
        {
            item.transform.SetParent(revAxis);
        }
        gameObject.SetActive(true);

        particle.Play();

        StartCoroutine(Co_RotationSword());
        StartCoroutine(Co_RevolutionSword(revAxis));
        StartCoroutine(Co_ActiveAfterImage(revAxis));

        StartCoroutine(Co_AttakcRadius());
    }
    public override void IncreaseSize(float value)
    {
        base.IncreaseSize(value);
        foreach (var item in afterImageArray)
        {
            item.transform.localScale = transform.localScale;
        }
    }
    private IEnumerator Co_ActiveAfterImage(Transform revAxis)
    {
        foreach(var item in afterImageArray)
        {
            yield return afterImageDelay;
            item.transform.localPosition = activePos;
            item.SetAfterImage(revAxis, transform, rotDirection, arrivalSecond, degree, transform.localScale, rotateSpeed);
        }
    }
    private IEnumerator Co_AttakcRadius()
    {
        while (true)
        {
            yield return attackDelay;

            int num = Physics.OverlapSphereNonAlloc(transform.position, attackRadius, collisionArray, ConstDefine.LAYER_MONSTER);
            AttackInRangeUtility.AttackLayerInRange(collisionArray, damage, num);
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += num * damage;
#endif
        }
    }
    private IEnumerator Co_RotationSword() //검 오브젝트 자전
    {
        while (true)
        {
            rotTransform.Rotate(rotDirection * rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }
    private IEnumerator Co_RevolutionSword(Transform revAxis) //검 오브젝트 공전. 현재 회전각에서 arrivalSecond동안 degree 까지 회전
    {
        float angle = 0;
        while (angle <= degree)
        {
            transform.RotateAround(revAxis.position, Vector3.up, degree / arrivalSecond * Time.deltaTime);
            angle += degree / arrivalSecond * Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
        bEndRev = true;
    }
}
