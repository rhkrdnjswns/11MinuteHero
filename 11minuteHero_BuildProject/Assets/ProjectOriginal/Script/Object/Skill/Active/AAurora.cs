using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAurora : AActiveSkill //오라 스킬 클래스
{
    [SerializeField] protected AttackRadiusUtility attackRadiusUtility;

    [SerializeField] protected ParticleSystem mainParticle;
    [SerializeField] protected ParticleSystem sideParticle;
    [SerializeField] protected ParticleSystem sparksParticle;

    private float originRadius;
    protected override void Awake()
    {
        base.Awake();

        originRadius = attackRadiusUtility.Radius;
    }
    public override void InitSkill()
    {
        base.InitSkill();

        transform.SetParent(null);
        SetCurrentDamage();
        SetParticleByRadius();
        transform.position = InGameManager.Instance.Player.transform.position;
        mainParticle.Play();
    }
    protected override void UpdateSkillData()
    {
        CurrentDamage = damage * level;
        attackRadiusUtility.Radius += 0.5f;
        SetParticleByRadius();
    }
    protected override void SetCurrentDamage()
    {
        CurrentDamage = damage * level;
    }
    protected override void SetCurrentRange(float value)
    {
        attackRadiusUtility.Radius += originRadius * value / 100f;
        SetParticleByRadius();
    }
    protected override IEnumerator Co_ActiveSkillAction() //스킬 기능 코루틴
    {
        while(true)
        {
            yield return new WaitForSeconds(CurrentCoolTime);
            attackRadiusUtility.AttackLayerInRadius(attackRadiusUtility.GetLayerInRadius(transform.root), CurrentDamage);
#if UNITY_EDITOR
            AttackCount++;
            int count = attackRadiusUtility.GetLayerInRadius(transform.root).Length;
            TotalDamage += CurrentDamage * count;
#endif
        }
    }
    protected virtual void SetParticleByRadius()
    {
        ParticleSystem.MainModule module = mainParticle.main;
        module.startSize = attackRadiusUtility.Radius * 2;

        module = sideParticle.main;
        module.startSizeX = attackRadiusUtility.Radius;
        module.startSizeZ = attackRadiusUtility.Radius;

        ParticleSystem.ShapeModule shape = sparksParticle.shape;
        shape.radius = attackRadiusUtility.Radius;
    }
    private void OnDrawGizmos() //씬뷰에서 공격 반경 표시를 위한 기즈모 그리기
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.root.position, attackRadiusUtility.Radius);
    }
    private void Update()
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
