using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIncreaseAttackRange : SPassive //공격 사거리 증가 패시브 스킬. 원거리 무기의 경우 투사체의 속도도 같이 증가함
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
        description = $"공격 범위가 {((percentage * (level + 1)) > percentage * ConstDefine.SKILL_MAX_LEVEL ? percentage * ConstDefine.SKILL_MAX_LEVEL : percentage * (level + 1))}%만큼 증가합니다.";
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.FireBomb);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }
}
