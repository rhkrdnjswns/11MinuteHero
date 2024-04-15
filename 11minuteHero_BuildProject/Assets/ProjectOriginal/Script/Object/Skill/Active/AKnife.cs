using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKnife : AActiveSkill
{
    [SerializeField] protected RangedAttackUtility rangedAttackUtility;
    [SerializeField] protected AttackRadiusUtility attackRadiusUtility;
    [SerializeField] protected AttackRadiusUtility knifeAttackRadiusUtility;

    [SerializeField] private float shotInterval;
    private bool bShotDone;

    private WaitForSeconds shotDelay;
    protected override void Awake()
    {
        base.Awake();

        rangedAttackUtility.Parent = transform;
        rangedAttackUtility.CreateNewProjectile(knifeAttackRadiusUtility);
        shotDelay = new WaitForSeconds(shotInterval);
    }
    public override void InitSkill()
    {
        base.InitSkill();

        SetCurrentDamage();
        UpdateSkillData();
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        WaitUntil shotDone = new WaitUntil(() => bShotDone);
        while (true)
        {
            yield return coolTimeDelay;
            StartCoroutine(Co_AttackInRadius());
            yield return shotDone;
        }
    }
    protected override void SetCurrentDamage()
    {
        currentDamage = damage;
        rangedAttackUtility.SetDamage(currentDamage);
    }
    protected override void UpdateSkillData()
    {
        rangedAttackUtility.SetCount(2 * level);
        rangedAttackUtility.ShotCount = level;
    }
    protected override void SetCurrentRange(float value)
    {
        rangedAttackUtility.IncreaseSize(value);
    }
    protected virtual IEnumerator Co_AttackInRadius()
    {
        bShotDone = false;
        Collider[] inRadiusMonsterArray = attackRadiusUtility.GetLayerInRadius(transform.root);
        if (inRadiusMonsterArray.Length == 0)
        {
            bShotDone = true;
            yield break;
        }
#if UNITY_EDITOR
        AttackCount++;
#endif
        int monsterIndex = Random.Range(0, inRadiusMonsterArray.Length);
        Transform t;
        Projectile p;
        for (int i = 0; i < rangedAttackUtility.ShotCount; i++)
        {
            if (!rangedAttackUtility.IsValid())
            {
                rangedAttackUtility.CreateNewProjectile(knifeAttackRadiusUtility);
                rangedAttackUtility.SetCount(2 * level);
            }
            p = rangedAttackUtility.SummonProjectile();
            t = inRadiusMonsterArray[monsterIndex++].transform;
            if (monsterIndex >= inRadiusMonsterArray.Length) monsterIndex = 0;

            p.ShotProjectile(t);
            if (i < rangedAttackUtility.ShotCount - 1) yield return shotInterval; //마지막 투사체 발사 시에는 텀 없게 하기
        }
        bShotDone = true;
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseHp) > 0
            && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseItemGain) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.ShadowKnife);
            bCanEvolution = true;
        }
    }
}
