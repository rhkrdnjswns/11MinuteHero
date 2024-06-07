using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIncreaseHp : SPassive //�ִ� ü�� ���� �нú� ��ų.
{
    public override void InitSkill()
    {
        base.InitSkill();
        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        InGameManager.Instance.Player.IncreaseMaxHp(percentage);
        description = $"ü���� {((percentage * (level + 1)) > percentage * ConstDefine.SKILL_MAX_LEVEL ? percentage * ConstDefine.SKILL_MAX_LEVEL : percentage * (level + 1))}%��ŭ �����մϴ�.";
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
}
