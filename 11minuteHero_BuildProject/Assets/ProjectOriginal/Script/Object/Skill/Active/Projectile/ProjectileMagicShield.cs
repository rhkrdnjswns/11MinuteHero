using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMagicShield : Projectile
{
    private Vector3 yDir = Vector3.right + Vector3.forward;
    [SerializeField] protected Transform objTransform;
    protected float activateTime;
    private float rotSpeed;
    private float knockBackSpeed;
    private float knockBackDuration;

    public override void SetKnockBackData(float speed, float duration)
    {
        knockBackSpeed = speed;
        knockBackDuration = duration;
    }
    public override void SetActivateTime(float time)
    {
        activateTime = time;
    }
    public override void SetRotationSpeed(float speed)
    {
        rotSpeed = speed;
    }
    public override void ShotProjectile()
    {
        transform.localRotation = Quaternion.identity;
        base.ShotProjectile();
        StartCoroutine(Co_Rotate());
    }
    protected override IEnumerator Co_Shot()
    {
        float timer = 0;
        while(timer < activateTime)
        {
            transform.position += shotDirection * speed / 2 * Time.deltaTime;
            transform.position += yDir * (5 - (speed * timer)) * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        owner.ReturnProjectile(this);
    }
    protected virtual IEnumerator Co_Rotate()
    {
        while(true)
        {
            objTransform.Rotate(transform.forward * rotSpeed * Time.deltaTime);
            yield return null;
        }
    }
    protected override void OnTriggerEnter(Collider other) //����ü �浹 ó��
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER)) //���Ϳ� �ε��� ���
        {
            Character c = other.GetComponent<Character>();
            c.Hit(damage); //Monster Ŭ������ �����Ͽ� ������ ����
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
#endif

            if (!c.IsDie) c.KnockBack(knockBackSpeed, knockBackDuration, (c.transform.position - transform.position).normalized);
        }
    }
}
