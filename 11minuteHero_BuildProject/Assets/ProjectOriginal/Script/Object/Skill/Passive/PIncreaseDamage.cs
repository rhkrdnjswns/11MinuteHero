using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIncreaseDamage : SPassive //������ ���� �нú� ��ų.
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
            item.IncreaseDamage(percentage);
        }
        description = $"���ݷ��� {((percentage * (level + 1)) > percentage * ConstDefine.SKILL_MAX_LEVEL ? percentage * ConstDefine.SKILL_MAX_LEVEL : percentage * (level + 1))}%��ŭ �����մϴ�.";
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.Meteor);
        if (skill == null) return;
        skill.SetEvlotionCondition();

        skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.MagicShield);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }
}
