using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIncreaseExpGain : SPassive //����ġ ȹ�淮 ���� �нú� ��ų.
{
    public override void InitSkill()
    {
        base.InitSkill();
        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        InGameManager.Instance.Player.ExpGained += percentage;
        description = $"����ġ ȹ�淮�� {((percentage * (level + 1)) > percentage * ConstDefine.SKILL_MAX_LEVEL ? percentage * ConstDefine.SKILL_MAX_LEVEL : percentage * (level + 1))}%��ŭ �����մϴ�.";
    }
    public override void SetEvlotionCondition()
    {
        Skill skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.Staff);
        if (skill == null) return;
        skill.SetEvlotionCondition();

        skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.CrystalStaff);
        if (skill == null) return;
        skill.SetEvlotionCondition();

        skill = InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.ShockBomb);
        if (skill == null) return;
        skill.SetEvlotionCondition();
    }
}
