using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIncreaseHp : SPassive //최대 체력 증가 패시브 스킬.
{
    public override void InitSkill()
    {
        base.InitSkill();
        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        InGameManager.Instance.Player.IncreaseMaxHp(percentage);
        description = $"체력이 {((percentage * (level + 1)) > percentage * ConstDefine.SKILL_MAX_LEVEL ? percentage * ConstDefine.SKILL_MAX_LEVEL : percentage * (level + 1))}%만큼 증가합니다.";
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
