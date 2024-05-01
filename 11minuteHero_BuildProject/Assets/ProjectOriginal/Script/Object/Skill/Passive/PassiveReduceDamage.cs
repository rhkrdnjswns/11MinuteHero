using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveReduceDamage : PassiveSkill //���ط� ���� �нú� ��ų.
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
        descriptionStringBuilder.Append("�޴� ���ط��� ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%��ŭ �����մϴ�.");
        base.SetDescription();
    }
}
