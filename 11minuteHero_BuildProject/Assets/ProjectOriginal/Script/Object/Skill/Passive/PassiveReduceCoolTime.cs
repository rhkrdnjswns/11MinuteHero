using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveReduceCoolTime : PassiveSkill //���� ��Ÿ�� ���� �нú� ��ų.
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
        if (skill != null)
        {
            skill.SetEvlotionCondition();
        }

        skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.CrystalStaff);
        if (skill != null)
        {
            skill.SetEvlotionCondition();
        }
    }

    protected override void SetDescription()
    {
        descriptionStringBuilder.Clear();
        descriptionStringBuilder.Append("��ų ��Ÿ���� ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%��ŭ �����մϴ�.");

        currentDescriptionStringBuilder.Clear();
        currentDescriptionStringBuilder.Append("��ų ��Ÿ���� ");
        currentDescriptionStringBuilder.Append(percentage * level);
        currentDescriptionStringBuilder.Append("%��ŭ �����մϴ�.");

        base.SetDescription();
    }
}
