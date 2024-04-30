using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAurora : ActiveSkill //오라 스킬 클래스
{
    [SerializeField] protected ParticleSystem mainParticle;
    [SerializeField] protected ParticleSystem sideParticle;
    [SerializeField] protected ParticleSystem sparksParticle;

    [SerializeField] protected float sensingRadius;
    [SerializeField] private float originRadius;
    [SerializeField] private float IncreaseRadiusValue;

    protected Collider[] collisionArray = new Collider[100];
    public override void InitSkill()
    {
        base.InitSkill();

        originRadius = sensingRadius;

        transform.SetParent(null);

        SetCurrentDamage();
        
        SetParticleByRadius();
        
        mainParticle.Play();
    }
    protected override void UpdateSkillData()
    {
        base.UpdateSkillData();

        sensingRadius += IncreaseRadiusValue;
        SetParticleByRadius();
    }
    protected override void SetCurrentRange(float value)
    {
        sensingRadius += originRadius * value / 100f;
        SetParticleByRadius();
    }
    protected override IEnumerator Co_ActiveSkillAction() //스킬 기능 코루틴
    {
        while(true)
        {
            yield return coolTimeDelay;

            int num = Physics.OverlapSphereNonAlloc(transform.root.position, sensingRadius, collisionArray, ConstDefine.LAYER_MONSTER);
            AttackInRangeUtility.AttackLayerInRange(collisionArray, currentDamage, num);
#if UNITY_EDITOR
            AttackCount++;
            TotalDamage += currentDamage * num;
#endif
        }
    }
    protected virtual void SetParticleByRadius()
    {
        ParticleSystem.MainModule module = mainParticle.main;
        module.startSize = sensingRadius * 2;

        module = sideParticle.main;
        module.startSizeX = sensingRadius;
        module.startSizeZ = sensingRadius;

        ParticleSystem.ShapeModule shape = sparksParticle.shape;
        shape.radius = sensingRadius;
    }
    private void FixedUpdate()
    {
        transform.position = InGameManager.Instance.Player.transform.position;
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.Recovery) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.SoulHarvest);
            bCanEvolution = true;
        }
    }
}
