using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveKnife : ActiveSkillUsingProjectile
{
    [SerializeField] private float shotInterval;
    [SerializeField] protected float sensingRadius;
    [SerializeField] protected float projectileSensingRadius;
    protected float originRadius;
    [SerializeField] private int bounceCount;

    protected Collider[] sensingCollisionArray = new Collider[50];
    private WaitForSeconds shotDelay;
    public override void ActivateSkill()
    {
        base.ActivateSkill();

        UpdateSkillData();
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        while (true)
        {
            yield return coolTimeDelay;
            yield return StartCoroutine(Co_AttackInRadius());
        }
    }
    protected override void SetCurrentRange(float value)
    {
        base.SetCurrentRange(value);

        projectileSensingRadius += originRadius * value / 100;
        projectileUtility.SetAttackRadius(projectileSensingRadius);
    }
    protected override void UpdateSkillData()
    {
        projectileUtility.SetBounceCount(bounceCount + level);
        currentShotCount = shotCount + level;
    }
    protected virtual IEnumerator Co_AttackInRadius()
    {
        int num = Physics.OverlapSphereNonAlloc(transform.root.position, sensingRadius, sensingCollisionArray, ConstDefine.LAYER_MONSTER);
        if (num == 0) yield break;

#if UNITY_EDITOR
        AttackCount++;
#endif

        int monsterIndex = Random.Range(0, num);

        for (int i = 0; i < currentShotCount; i++)
        {
            if (!projectileUtility.IsValid())
            {
                InitProjectile();
            }
            Projectile p = projectileUtility.GetProjectile();
            p.SetTargetTransform(sensingCollisionArray[monsterIndex++].transform);

            p.ShotProjectile();

            if (monsterIndex >= num) monsterIndex = 0;

            if (i < currentShotCount - 1) yield return shotDelay; //마지막 투사체 발사 시에는 텀 없게 하기
        }
    }
    protected override void InitProjectile()
    {
        base.InitProjectile();
        projectileUtility.SetAttackRadius(projectileSensingRadius);
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
    protected override void ReadActiveCSVData()
    {
        base.ReadActiveCSVData();

        sensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 4);

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 14);
        currentShotCount = shotCount;

        speed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 18);
        shotInterval = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 19);
        shotDelay = new WaitForSeconds(shotInterval);

        projectileSensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 31);
        originRadius = projectileSensingRadius;

        bounceCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 32);
    }
}
