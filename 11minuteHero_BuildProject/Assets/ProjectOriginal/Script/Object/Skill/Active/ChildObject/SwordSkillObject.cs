using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillObject : MonoBehaviour
{
    public enum ESwordType
    {
        BloodSword,
        CurseSword,
        DevilSword
    }
    private Transform rotTransform; //������ ������Ʈ Ʈ������ (ȸ�� ȿ���� ���� ����)
    private Vector3 rotDirection; //���� ����

    [SerializeField] private ParticleSystem particle; //��ƼŬ
    [SerializeField] private ESwordType eSwordType; //�� Ÿ�� (�Ƿι��� �� / ���ֹ��� ��)

    [SerializeField] private SwordSkillAfterImage[] afterImageArray; //�ܻ� ���� �迭
    private AttackRadiusUtility attackRadiusUtility; //�ݰ� ���� Ŭ���� ����
    private Vector3 activePos; //���� ��ġ

    private float damage; //���ݷ�

    public bool bEndRev { get; set; }
    public SwordSkillAfterImage[] GetAfterImage()
    {
        return afterImageArray;
    }
    public void InitObject(AttackRadiusUtility attackRadiusUtility, float distance)
    {
        this.attackRadiusUtility = attackRadiusUtility;

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
    }
    public void ActivateSkill(Transform revAxis, float arrivalSecond, float degree, float damage, float attackInterval)
    {
        this.damage = damage;

        transform.localPosition = activePos;
        foreach (var item in afterImageArray)
        {
            item.transform.SetParent(revAxis);
        }
        gameObject.SetActive(true);

        particle.Play();

        StartCoroutine(Co_RotationSword());
        StartCoroutine(Co_RevolutionSword(revAxis, arrivalSecond, degree));
        StartCoroutine(Co_ActiveAfterImage(revAxis, arrivalSecond, degree));

        StartCoroutine(Co_AttakcRadius(attackInterval));
    }
    public void SetAfterImageSize()
    {
        foreach (var item in afterImageArray)
        {
            item.transform.localScale = transform.localScale;
        }
    }
    private IEnumerator Co_ActiveAfterImage(Transform revAxis, float arrivalSecond, float degree)
    {
        foreach(var item in afterImageArray)
        {
            yield return new WaitForSeconds(0.1f);
            item.transform.localPosition = activePos;
            item.SetAfterImage(revAxis, transform, rotDirection, arrivalSecond, degree);
        }
    }
    private IEnumerator Co_AttakcRadius(float attackInterval)
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            attackRadiusUtility.AttackLayerInRadius(attackRadiusUtility.GetLayerInRadius(transform), damage);
        }
    }
    private IEnumerator Co_RotationSword() //�� ������Ʈ ����
    {
        while (true)
        {
            rotTransform.Rotate(rotDirection * 720 * Time.deltaTime);
            yield return null;
        }
    }
    private IEnumerator Co_RevolutionSword(Transform revAxis, float arrivalSecond, float degree) //�� ������Ʈ ����. ���� ȸ�������� arrivalSecond���� degree ���� ȸ��
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
    private void OnDrawGizmos()
    {
        if (attackRadiusUtility == null) return;
        Gizmos.DrawWireSphere(transform.position, attackRadiusUtility.Radius);
    }
}
