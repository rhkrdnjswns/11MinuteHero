public class PassiveIncreaseDamage : PassiveSkill //������ ���� �нú� ��ų.
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
        descriptionStringBuilder.Append("���ݷ��� ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%��ŭ �����մϴ�.");

        currentDescriptionStringBuilder.Clear();
        currentDescriptionStringBuilder.Append("���ݷ��� ");
        currentDescriptionStringBuilder.Append(percentage * level);
        currentDescriptionStringBuilder.Append("%��ŭ �����մϴ�.");

        base.SetDescription();
    }
}
