public class PassiveIncreaseMoveSpeed : PassiveSkill //이동 속도 증가 패시브 스킬.
{
    protected override void UpdateSkillData()
    {
        InGameManager.Instance.Player.IncreaseSpeed(percentage, EApplicableType.Percentage);
        SetDescription();
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.EarthSpell);
        if (skill == null) return;
        skill.SetEvlotionCondition();

        skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.ShockBomb);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }

    protected override void SetDescription()
    {
        descriptionStringBuilder.Clear();
        descriptionStringBuilder.Append("이동속도가 ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%만큼 증가합니다.");
        base.SetDescription();
    }
}
