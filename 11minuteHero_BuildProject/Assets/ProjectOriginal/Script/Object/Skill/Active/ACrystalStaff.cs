using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACrystalStaff : AActiveSkill
{
    [SerializeField] protected RangedAttackUtility rangedAttackUtility;
    [SerializeField] private AttackRadiusUtility attackRadiusUtility;

    [SerializeField] private int count; //관통 횟수
    [SerializeField] private float activateTime; //활성 시간
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
        rangedAttackUtility.SetCount(count);

        UpdateSkillData();
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentCoolTime);
            StartCoroutine(Co_AttackInRadius());
        }
    }
    protected override void SetCurrentDamage()
    {
        currentDamage = damage * level;
        rangedAttackUtility.SetDamage(currentDamage);
    }
    protected override void SetCurrentRange(float value)
    {
        rangedAttackUtility.IncreaseSize(value);
    }
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
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
            rangedAttackUtility.SetCount(count);
        }
        Projectile p = rangedAttackUtility.SummonProjectile();
        p.transform.forward = (t.position - transform.root.position).normalized;
        p.SetShotDirection(p.transform.forward);
        p.ShotProjectile();
        yield return null;
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.ReduceCoolTime) > 0
            && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseExpGain) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.MagicArrow);
            bCanEvolution = true;
        }
    }
}
