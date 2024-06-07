using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIncreaseAttackRange : SPassive //���� ��Ÿ� ���� �нú� ��ų. ���Ÿ� ������ ��� ����ü�� �ӵ��� ���� ������
{
    public override void InitSkill()
    {
        base.InitSkill();
        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        foreach (var item in InGameManager.Instance.SkillManager.ActiveSkillList)
        {
            item.IncreaseRange(percentage);
        }
        description = $"���� ������ {((percentage * (level + 1)) > percentage * ConstDefine.SKILL_MAX_LEVEL ? percentage * ConstDefine.SKILL_MAX_LEVEL : percentage * (level + 1))}%��ŭ �����մϴ�.";
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.FireBomb);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }
}
