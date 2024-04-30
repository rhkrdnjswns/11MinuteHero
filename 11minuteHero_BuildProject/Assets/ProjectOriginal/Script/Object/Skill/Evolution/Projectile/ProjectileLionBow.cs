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
            other.GetComponent<Character>().Hit(damage); //Monster 클래스를 추출하여 데미지 연산
            currentCount--;
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
#endif
            SetSpeedUp();

            if (currentCount > 0) return;
            currentCount = count; //관통 횟수를 전부 소모한 경우 투사체 회수
            owner.ReturnProjectile(this);
        }
    }
}
