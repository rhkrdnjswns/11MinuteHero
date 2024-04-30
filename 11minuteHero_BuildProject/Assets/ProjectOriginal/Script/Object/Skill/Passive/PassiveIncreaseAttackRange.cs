public class PassiveIncreaseAttackRange : PassiveSkill //공격 사거리 증가 패시브 스킬. 원거리 무기의 경우 투사체의 속도도 같이 증가함
{
    protected override void UpdateSkillData()
    {
        foreach (var item in InGameManager.Instance.SkillManager.ActiveSkillList)
        {
            item.IncreaseRange(percentage);
        }
        descriptionStringBuilder.Clear();
        descriptionStringBuilder.Append("공격 범위가 ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%만큼 증가합니다.");

        base.UpdateSkillData();
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.FireBomb);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }
}
