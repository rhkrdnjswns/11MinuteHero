public class PassiveIncreaseItemGain : PassiveSkill //아이템 획득 범위 증가 패시브 스킬.
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
        descriptionStringBuilder.Append("아이템 획득 범위가 ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%만큼 증가합니다.");
        base.SetDescription();
    }
}
