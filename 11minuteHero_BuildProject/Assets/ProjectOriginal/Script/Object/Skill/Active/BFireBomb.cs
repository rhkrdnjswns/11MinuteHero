using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFireBomb : ABomb
{
    [SerializeField] private float dotDuration;
    [SerializeField] private float dotInterval;
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseAttackRange) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.HolyWater);
            bCanEvolution = true;
        }
    }
    protected override void SetBombProjectile()
    {
        foreach (var item in rangedAttackUtility.AllProjectileList)
        {
            item.GetComponent<PBomb>().SetAngle(angle);
            item.GetComponent<PFireBomb>().SetDotInfo(dotDuration, dotInterval);
        }
    }
}
