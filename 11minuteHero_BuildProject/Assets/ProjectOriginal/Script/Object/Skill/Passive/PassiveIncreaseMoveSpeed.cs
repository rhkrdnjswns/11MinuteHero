public class PassiveIncreaseMoveSpeed : PassiveSkill //�̵� �ӵ� ���� �нú� ��ų.
{
    protected override void UpdateSkillData()
    {
        InGameManager.Instance.Player.IncreaseSpeed(percentage, EApplicableType.Percentage);
        SetDescription();
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.EarthSpell);
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
        descriptionStringBuilder.Append("�̵��ӵ��� ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%��ŭ �����մϴ�.");

        currentDescriptionStringBuilder.Clear();
        currentDescriptionStringBuilder.Append("�̵��ӵ��� ");
        currentDescriptionStringBuilder.Append(percentage * level);
        currentDescriptionStringBuilder.Append("%��ŭ �����մϴ�.");

        base.SetDescription();
    }
}
