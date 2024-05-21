using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveRecovery : PassiveSkill //체력 재생 패시브 스킬
{
    public override void ActivateSkill()
    {
        base.ActivateSkill();
        StartCoroutine(Co_Recovery());
    }
    protected override void UpdateSkillData()
    {
        SetDescription();
    }
    private IEnumerator Co_Recovery() //체력 재생 코루틴
    {
        WaitForSeconds delay = new WaitForSeconds(1f);
        while (true)
        {
            yield return delay;
            InGameManager.Instance.Player.RecoverHp(percentage * level, EApplicableType.Percentage);
        }
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.Aurora);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }

    protected override void SetDescription()
    {
        descriptionStringBuilder.Clear();
        descriptionStringBuilder.Append("초당 최대 체력의 ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%를 회복합니다.");

        currentDescriptionStringBuilder.Clear();
        currentDescriptionStringBuilder.Append("초당 최대 체력의 ");
        currentDescriptionStringBuilder.Append(percentage * level);
        currentDescriptionStringBuilder.Append("%를 회복합니다.");

        base.SetDescription();
    }
}
