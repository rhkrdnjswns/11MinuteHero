using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPenetrationProjectile : Projectile
{
    protected int count; //���� ������ Ƚ��
    protected int currentCount; //���� ������ Ƚ��
    protected float activateTime; //Ȱ�� �ð�
    public override void SetCount(int count)
    {
        this.count = count;
        currentCount = this.count;
    }
    public override void SetActivateTime(float time)
    {
        activateTime = time;
    }
    protected override IEnumerator Co_Shot()
    {
        float timer = 0;
        while (timer < activateTime)
        {
            transform.position += shotDirection * rangedAttackUtility.ProjectileSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        currentCount = count;
        rangedAttackUtility.ReturnProjectile(this);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER))
        {
            other.GetComponent<Character>().Hit(rangedAttackUtility.ProjectileDamage); //Monster Ŭ������ �����Ͽ� ������ ����
            currentCount--;
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += rangedAttackUtility.ProjectileDamage;
#endif
            if (currentCount > 0) return;
            currentCount = count; //���� Ƚ���� ���� �Ҹ��� ��� ����ü ȸ��
            rangedAttackUtility.ReturnProjectile(this);
        }
    }
}
