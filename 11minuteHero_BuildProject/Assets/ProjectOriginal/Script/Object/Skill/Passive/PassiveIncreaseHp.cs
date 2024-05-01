public class PassiveIncreaseHp : PassiveSkill //최대 체력 증가 패시브 스킬.
{
    protected override void UpdateSkillData()
    {
        InGameManager.Instance.Player.IncreaseMaxHp(percentage);
        SetDescription();
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.Sword);
        if (skill == null) return;
        skill.SetEvlotionCondition();

        skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.Knife);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }

    protected override void SetDescription()
    {
        descriptionStringBuilder.Clear();
        descriptionStringBuilder.Append("체력이 ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%만큼 증가합니다.");
        base.SetDescription();
    }
}
