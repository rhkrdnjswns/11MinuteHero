using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveReduceDamage : PassiveSkill //피해량 감소 패시브 스킬.
{
    protected override void UpdateSkillData()
    {
        InGameManager.Instance.Player.DamageReduction += percentage;

        SetDescription();
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.Wisp);
        if (skill == null) return;
        skill.SetEvlotionCondition();

        skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.MagicShield);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }

    protected override void SetDescription()
    {
        descriptionStringBuilder.Clear();
        descriptionStringBuilder.Append("받는 피해량이 ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%만큼 감소합니다.");
        base.SetDescription();
    }
}
