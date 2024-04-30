using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMagicShield : ActiveSkillUsingProjectile
{
    [SerializeField] private float shotInterval;
    [SerializeField] protected float activateTime;
    [SerializeField] protected float rotateSpeed;

    private WaitForSeconds shotDelay;
    public override void InitSkill()
    {
        base.InitSkill();

        shotDelay = new WaitForSeconds(shotInterval);
        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        base.UpdateSkillData();
        currentShotCount = level;
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        while (true)
        {
            yield return coolTimeDelay;
            yield return StartCoroutine(Co_ShotProjectile());
        }
    }
    private IEnumerator Co_ShotProjectile()
    {
#if UNITY_EDITOR
        AttackCount++;
#endif
        for (int i = 0; i < currentShotCount; i++)
        {
            if (!projectileUtility.IsValid())
            {
                InitProjectile();
            }
            Projectile p = projectileUtility.GetProjectile();
            p.SetShotDirection(GetDirection(i));

            p.ShotProjectile();
            if(i < currentShotCount - 1) yield return shotDelay; //마지막 투사체 발사 시에는 텀 없게 하기
        }
    }
    protected virtual Vector3 GetDirection(int i)
    {
        if (i % 2 == 0)
        {
            return Vector3.right + Vector3.back;
        }
        else
        {
            return Vector3.left + Vector3.forward;
        }
    }
    protected override void InitProjectile()
    {
        base.InitProjectile();
        projectileUtility.SetActivateTime(activateTime);
        projectileUtility.SetRotateSpeed(rotateSpeed);
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
