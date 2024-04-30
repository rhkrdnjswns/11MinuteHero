using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLionBow : ProjectilePenetration
{
    private System.Action SetSpeedUp;
    public override void SetAction(System.Action action)
    {
        this.SetSpeedUp = action;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER))
        {
            other.GetComponent<Character>().Hit(damage); //Monster Ŭ������ �����Ͽ� ������ ����
            currentCount--;
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
#endif
            SetSpeedUp();

            if (currentCount > 0) return;
            currentCount = count; //���� Ƚ���� ���� �Ҹ��� ��� ����ü ȸ��
            owner.ReturnProjectile(this);
        }
    }
}
