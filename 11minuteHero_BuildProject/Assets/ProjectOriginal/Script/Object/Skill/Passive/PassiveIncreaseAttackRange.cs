public class PassiveIncreaseAttackRange : PassiveSkill //���� ��Ÿ� ���� �нú� ��ų. ���Ÿ� ������ ��� ����ü�� �ӵ��� ���� ������
{
    protected override void UpdateSkillData()
    {
        foreach (var item in InGameManager.Instance.SkillManager.ActiveSkillList)
        {
            item.IncreaseRange(percentage);
        }
        descriptionStringBuilder.Clear();
        descriptionStringBuilder.Append("���� ������ ");
        descriptionStringBuilder.Append(GetPercentageForDescription());
        descriptionStringBuilder.Append("%��ŭ �����մϴ�.");

        base.UpdateSkillData();
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.FireBomb);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }
}
