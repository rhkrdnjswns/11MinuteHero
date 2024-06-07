using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMagicShield : AActiveSkill
{
    [SerializeField] protected RangedAttackUtility rangedAttackUtility;
    [SerializeField] private float shotInterval;
    [SerializeField] protected float activateTime;
    private bool bShotDone;

    private WaitForSeconds shotDelay;
    protected override void Awake()
    {
        base.Awake();
        rangedAttackUtility.Parent = transform;
        rangedAttackUtility.CreateNewProjectile();
        rangedAttackUtility.SetActivateTime(activateTime);
        shotDelay = new WaitForSeconds(shotInterval);
    }
    public override void InitSkill()
    {
        base.InitSkill();

        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
        rangedAttackUtility.ShotCount = level;
    }
    protected override void SetCurrentDamage()
    {
        CurrentDamage = damage * level;
        rangedAttackUtility.SetDamage(CurrentDamage);
    }
    protected override void SetCurrentRange(float value)
    {
        rangedAttackUtility.IncreaseSize(value);
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        WaitUntil shotDone = new WaitUntil(() => bShotDone);
        while (true)
        {
            yield return coolTimeDelay;
            StartCoroutine(Co_ShotProjectile());
            yield return shotDone;
        }
    }
    private IEnumerator Co_ShotProjectile()
    {
        bShotDone = false;
#if UNITY_EDITOR
        AttackCount++;
#endif
        Projectile p;
        for (int i = 0; i < rangedAttackUtility.ShotCount; i++)
        {
            if (!rangedAttackUtility.IsValid()) rangedAttackUtility.CreateNewProjectile();
            p = rangedAttackUtility.SummonProjectile();
            if(i % 2 == 0) //왼쪽 오른쪽 번갈아가면서 소환
            {
                p.SetShotDirection(Vector3.right + Vector3.back);
            }
            else
            {
                p.SetShotDirection(Vector3.left + Vector3.forward);
            }

            p.ShotProjectile();
            if(i < rangedAttackUtility.ShotCount - 1) yield return shotDelay; //마지막 투사체 발사 시에는 텀 없게 하기
        }
        bShotDone = true;
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseDamage) > 0
            && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.ReduceDamage) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.Renotoros);
            bCanEvolution = true;
        }
    }
}
