using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePenetration : Projectile
{
    protected int count; //���� ������ Ƚ��
    protected int currentCount; //���� ������ Ƚ��
    protected float activateTime; //Ȱ�� �ð�
    public override void SetActivateTime(float time)
    {
        activateTime = time;
    }
    public override void SetPenetrationCount(int count)
    {
        this.count = count;
        currentCount = count;
    }
    protected override IEnumerator Co_Shot()
    {
        float timer = 0;
        while (timer < activateTime)
        {
            transform.position += shotDirection * speed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        currentCount = count;
        owner.ReturnProjectile(this);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            other.GetComponent<Character>().Hit(damage); //Monster Ŭ������ �����Ͽ� ������ ����
            currentCount--;
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
#endif
            if (currentCount > 0) return;
            currentCount = count; //���� Ƚ���� ���� �Ҹ��� ��� ����ü ȸ��
            owner.ReturnProjectile(this);
        }
    }
}
