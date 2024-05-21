using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveIncreaseExpGain : PassiveSkill //����ġ ȹ�淮 ���� �нú� ��ų.
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
        descriptionStringBuilder.Append("����ġ ȹ�淮�� ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%��ŭ �����մϴ�.");

        currentDescriptionStringBuilder.Clear();
        currentDescriptionStringBuilder.Append("����ġ ȹ�淮�� ");
        currentDescriptionStringBuilder.Append(percentage * level);
        currentDescriptionStringBuilder.Append("%��ŭ �����մϴ�.");

        base.SetDescription();
    }
}
