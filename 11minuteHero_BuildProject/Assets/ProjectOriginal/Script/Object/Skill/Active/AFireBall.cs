using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFireBall : AActiveSkill //파이어볼 스킬 클래스
{
    [SerializeField] protected RangedAttackUtility rangedAttackUtility; 

    [SerializeField] private float activateTime; //활성 시간

    [SerializeField] private AttackRadiusUtility attackRadiusUtility;

    [SerializeField] protected int penetrationCount;

    protected float yOffset; //현재 투사체 범위에 따른 y 포지션 조정값

    protected override void Awake()
    {
        base.Awake();
        rangedAttackUtility.Parent = transform;
        rangedAttackUtility.CreateNewProjectile();
    }
    public override void InitSkill()
    {
        base.InitSkill();
        rangedAttackUtility.SetActivateTime(activateTime);
        rangedAttackUtility.SetCount(penetrationCount + level);

        SetCurrentDamage();
    }
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
        rangedAttackUtility.SetCount(penetrationCount + level);
        rangedAttackUtility.IncreaseSize(13f);
        yOffset += 0.07f;
    }
    protected override void SetCurrentDamage()
    {
        CurrentDamage = damage * level;
        rangedAttackUtility.SetDamage(CurrentDamage);
    }
    protected override void SetCurrentRange(float value)
    {
        rangedAttackUtility.IncreaseSize(value);
        yOffset += 0.07f;
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(CurrentCoolTime);
            StartCoroutine(Co_AttackInRadius());
        }
    }
    private IEnumerator Co_AttackInRadius()
    {
        Collider[] inRadiusMonsterArray = attackRadiusUtility.GetLayerInRadiusSortedByDistance(transform.root);
        if (inRadiusMonsterArray.Length == 0) yield break;

        Transform t = inRadiusMonsterArray[0].transform;

        if (!rangedAttackUtility.IsValid())
        {
            rangedAttackUtility.CreateNewProjectile();
            rangedAttackUtility.SetActivateTime(activateTime);
            rangedAttackUtility.SetCount(9 + level);
        }
        Projectile p = rangedAttackUtility.SummonProjectile();
        p.SetShotDirection((t.position - transform.root.position).normalized);

        p.transform.position += Vector3.up * yOffset; //범위가 증가할수록 y 위치를 조금씩 위로 올려줌

        p.ShotProjectile();
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.ReduceCoolTime) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.HellFire);
            bCanEvolution = true;
        }
    }
}
