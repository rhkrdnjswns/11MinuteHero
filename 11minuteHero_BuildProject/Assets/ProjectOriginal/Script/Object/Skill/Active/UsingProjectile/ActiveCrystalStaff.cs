using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActiveCrystalStaff : ActiveSkillUsingProjectile
{
    [SerializeField] private int penetrationCount; //관통 횟수
    [SerializeField] private float activateTime; //활성 시간
    [SerializeField] private float sensingRadius;

    private Collider[] sensingCollisionArray = new Collider[50];
    public override void InitSkill()
    {
        base.InitSkill();

        UpdateSkillData();
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        while (true)
        {
            yield return coolTimeDelay;
            AttackInRadius();
        }
    }
    private void AttackInRadius()
    {
        int num = Physics.OverlapSphereNonAlloc(transform.root.position, sensingRadius, sensingCollisionArray, ConstDefine.LAYER_MONSTER);
        if (num == 0) return;

#if UNITY_EDITOR
        AttackCount++;
#endif
        AttackInRangeUtility.QuickSortCollisionArray(sensingCollisionArray, 0, num - 1);

        Vector3 targetPos = sensingCollisionArray[0].transform.position + Vector3.up * 0.5f;

        if (projectileUtility.IsValid())
        {
            InitProjectile();
        }
        Projectile p = projectileUtility.GetProjectile();
        p.SetShotDirection((targetPos - p.transform.position).normalized);

        p.ShotProjectile();
    }
    protected override void InitProjectile()
    {
        base.InitProjectile();
        projectileUtility.SetActivateTime(activateTime);
        projectileUtility.SetPenetrationCount(penetrationCount);
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
