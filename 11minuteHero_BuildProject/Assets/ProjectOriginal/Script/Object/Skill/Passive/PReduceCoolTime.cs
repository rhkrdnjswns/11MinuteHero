using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PReduceCoolTime : SPassive //무기 쿨타임 감소 패시브 스킬.
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
            item.ReduceCoolTime(percentage);
        }
        description = $"스킬 쿨타임이 {((percentage * (level + 1)) > percentage * ConstDefine.SKILL_MAX_LEVEL ? percentage * ConstDefine.SKILL_MAX_LEVEL : percentage * (level + 1))}%만큼 감소합니다.";
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.FireBall);
        if (skill == null) return;
        skill.SetEvlotionCondition();

        skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.CrystalStaff);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }
}
