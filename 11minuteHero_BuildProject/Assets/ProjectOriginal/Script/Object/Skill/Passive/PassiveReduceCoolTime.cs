using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveReduceCoolTime : PassiveSkill //무기 쿨타임 감소 패시브 스킬.
{
    protected override void UpdateSkillData()
    {
        foreach (var item in InGameManager.Instance.SkillManager.ActiveSkillList)
        {
            item.ReduceCoolTime(percentage);
        }
        SetDescription();
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.FireBall);
        if (skill == null) return;
        skill.SetEvlotionCondition();

        skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.CrystalStaff);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }

    protected override void SetDescription()
    {
        descriptionStringBuilder.Clear();
        descriptionStringBuilder.Append("스킬 쿨타임이 ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%만큼 감소합니다.");
        base.SetDescription();
    }
}
