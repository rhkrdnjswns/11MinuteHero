using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMagicShield : AActiveSkill
{
    [SerializeField] protected RangedAttackUtility rangedAttackUtility;
    [SerializeField] private float shotInterval;
    [SerializeField] protected float activateTime;
    private bool bShotDone;
    protected override void Awake()
    {
        base.Awake();
        rangedAttackUtility.Parent = transform;
        rangedAttackUtility.CreateNewProjectile();
        rangedAttackUtility.SetActivateTime(activateTime);
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
        currentDamage = damage * level;
        rangedAttackUtility.SetDamage(currentDamage);
    }
    protected override void SetCurrentRange(float value)
    {
        rangedAttackUtility.IncreaseSize(value);
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentCoolTime);
            StartCoroutine(Co_ShotProjectile());
            yield return new WaitUntil(() => bShotDone);
        }
    }
    private IEnumerator Co_ShotProjectile()
    {
        bShotDone = false;
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
            if(i < rangedAttackUtility.ShotCount - 1) yield return new WaitForSeconds(shotInterval); //마지막 투사체 발사 시에는 텀 없게 하기
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
