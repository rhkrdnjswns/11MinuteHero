using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMagicShield : Projectile
{
    private Vector3 yDir = Vector3.right + Vector3.forward;
    [SerializeField] protected Transform objTransform;
    protected float activateTime;
    public override void SetActivateTime(float time)
    {
        activateTime = time;
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
            transform.position += shotDirection * rangedAttackUtility.ProjectileSpeed * Time.deltaTime;
            transform.position += yDir * (5 - (5 * timer)) * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        rangedAttackUtility.ReturnProjectile(this);
    }
    protected virtual IEnumerator Co_Rotate()
    {
        while(true)
        {
            objTransform.Rotate(transform.forward * 360 * Time.deltaTime);
            yield return null;
        }
    }
    protected override void OnTriggerEnter(Collider other) //����ü �浹 ó��
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER)) //���Ϳ� �ε��� ���
        {
            Character c = other.GetComponent<Character>();
            c.Hit(rangedAttackUtility.ProjectileDamage); //Monster Ŭ������ �����Ͽ� ������ ����
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += rangedAttackUtility.ProjectileDamage;
#endif

            if (!c.IsDie) c.KnockBack(0.3f, 0.3f);
        }
    }
}
