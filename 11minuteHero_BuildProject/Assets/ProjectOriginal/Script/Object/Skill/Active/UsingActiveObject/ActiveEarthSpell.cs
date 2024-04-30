using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEarthSpell : ActiveSkillUsingActiveObject //대지의 마법서 스킬 클래스
{
    [SerializeField] private float radius;
    private float originRadius;
    [SerializeField] protected float slowDuration;
    [SerializeField] protected float slowPercentage;

    public override void InitSkill()//오브젝트 위치 갱신 및 컴포넌트 참조 인스턴스화
    {
        base.InitSkill();

        originRadius = radius;
        UpdateSkillData();
    }
    protected override void SetCurrentRange(float value)
    {
        base.SetCurrentRange(value);
        radius += originRadius * value / 100;
    }
    protected override IEnumerator Co_ActiveSkillAction() //스킬 기능 코루틴 
    {
        while(true)
        {
            yield return coolTimeDelay;
            ActiveObject obj = activeObjectUtility.GetObject();
            obj.Activate();
#if UNITY_EDITOR
            AttackCount++;
#endif
        }
    }
    protected override void InitActiveObject()
    {
        base.InitActiveObject();
        activeObjectUtility.SetAttackRadius(radius);
        activeObjectUtility.SetSlowDownData(slowPercentage, slowDuration);
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseMoveSpeed) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.EarthImplosion);
            bCanEvolution = true;
        }
    }
}
