public class PassiveIncreaseDamage : PassiveSkill //데미지 증가 패시브 스킬.
{
    protected override void UpdateSkillData()
    {
        foreach (var item in InGameManager.Instance.SkillManager.ActiveSkillList)
        {
            item.IncreaseDamage(percentage);
        }
        SetDescription();
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.Meteor);
        if (skill != null)
        {
            skill.SetEvlotionCondition();
        }

        skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.MagicShield);
        if (skill != null)
        {
            skill.SetEvlotionCondition();
        }
    }
    protected override void SetDescription()
    {
        descriptionStringBuilder.Clear();
        descriptionStringBuilder.Append("공격력이 ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%만큼 증가합니다.");

        currentDescriptionStringBuilder.Clear();
        currentDescriptionStringBuilder.Append("공격력이 ");
        currentDescriptionStringBuilder.Append(percentage * level);
        currentDescriptionStringBuilder.Append("%만큼 증가합니다.");

        base.SetDescription();
    }
}
