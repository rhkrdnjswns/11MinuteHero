using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveIncreaseExpGain : PassiveSkill //경험치 획득량 증가 패시브 스킬.
{
    protected override void UpdateSkillData()
    {
        InGameManager.Instance.Player.ExpGained += (int)percentage;
        SetDescription();
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.Staff);
        if (skill != null)
        {
            skill.SetEvlotionCondition();
        }
        skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.CrystalStaff);
        if (skill != null)
        {
            skill.SetEvlotionCondition();
        }

        skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.ShockBomb);
        if (skill != null)
        {
            skill.SetEvlotionCondition();
        }
    }

    protected override void SetDescription()
    {
        descriptionStringBuilder.Clear();
        descriptionStringBuilder.Append("경험치 획득량이 ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%만큼 증가합니다.");

        currentDescriptionStringBuilder.Clear();
        currentDescriptionStringBuilder.Append("경험치 획득량이 ");
        currentDescriptionStringBuilder.Append(percentage * level);
        currentDescriptionStringBuilder.Append("%만큼 증가합니다.");

        base.SetDescription();
    }
}
