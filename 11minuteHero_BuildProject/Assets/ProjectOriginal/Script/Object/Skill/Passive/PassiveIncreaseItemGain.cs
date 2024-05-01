public class PassiveIncreaseItemGain : PassiveSkill //������ ȹ�� ���� ���� �нú� ��ų.
{
    protected override void UpdateSkillData()
    {
        InGameManager.Instance.Player.IncreaseItemColliderRadius(percentage);
        SetDescription();
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.Bow);
        if (skill == null) return;
        skill.SetEvlotionCondition();

        skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.Knife);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }

    protected override void SetDescription()
    {
        descriptionStringBuilder.Clear();
        descriptionStringBuilder.Append("������ ȹ�� ������ ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%��ŭ �����մϴ�.");
        base.SetDescription();
    }
}
