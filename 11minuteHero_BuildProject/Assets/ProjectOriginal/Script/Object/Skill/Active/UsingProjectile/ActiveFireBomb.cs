using System.Collections;
using UnityEngine;

public class ActiveFireBomb : ActiveSkillUsingProjectile
{
    [Range(1, 89)]
    [SerializeField] protected float angle;
    [SerializeField] protected float shotInterval = 0.3f;
    [SerializeField] private float dotDuration;
    [SerializeField] private float dotInterval;
    [SerializeField] private float dotAreaRadius;
    [SerializeField] private float sensingRadius;

    protected WaitForSeconds shotDelay;

    private Collider[] sensingCollisionArray = new Collider[50];
    private float originRadius;
    public override void ActivateSkill()
    {
        base.ActivateSkill();

        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        base.UpdateSkillData();
        currentShotCount = shotCount * level;
    }
    protected override void SetCurrentRange(float value)
    {
        base.SetCurrentRange(value);
        dotAreaRadius += originRadius * value / 100f;
        projectileUtility.SetAttackRadius(dotAreaRadius);
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        while (true)
        {
            yield return coolTimeDelay;
            yield return StartCoroutine(Co_AttackInRadius());
        }
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
            p.SetMotion(sensingCollisionArray[monsterIndex++].transform.position);

            p.ShotProjectile();

            if (monsterIndex >= num) monsterIndex = 0;

            if (i == currentShotCount - 1) break;

            yield return shotDelay;
        }
    }
    protected override void InitProjectile()
    {
        base.InitProjectile();
        projectileUtility.SetAngle(angle);
        projectileUtility.SetDotData(dotInterval, dotDuration);
        projectileUtility.SetAttackRadius(dotAreaRadius);
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseAttackRange) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.HolyWater);
            bCanEvolution = true;
        }
    }
    protected override void ReadCSVData()
    {
        base.ReadCSVData();

        if (eSkillType == ESkillType.Evolution) return;
        sensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 4);
        dotInterval = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 8);

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 14);
        currentShotCount = shotCount;

        shotInterval = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 19);
        shotDelay = new WaitForSeconds(shotInterval);

        dotAreaRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 28);
        originRadius = dotAreaRadius;

        dotDuration = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 29);
    }

}
