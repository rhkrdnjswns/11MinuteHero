using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveShockBomb : ActiveSkillUsingProjectile
{
    [Range(1, 89)]
    [SerializeField] protected float angle;
    [SerializeField] protected float shotInterval = 0.3f;
    [SerializeField] protected float slowDownValue;
    [SerializeField] protected float debuffDuration;
    [SerializeField] protected float sensingRadius;

    private Collider[] sensingCollisionArray = new Collider[50];

    protected WaitForSeconds shotDelay;

    private float originRadius;
   [SerializeField] private float radius;
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
        radius += originRadius * value / 100f;
        projectileUtility.SetAttackRadius(radius);
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
        projectileUtility.SetSlowDownData(slowDownValue, debuffDuration);
        projectileUtility.SetAttackRadius(radius);
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseMoveSpeed) > 0
            && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseExpGain) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.FaintBomb);
            bCanEvolution = true;
        }
    }
    protected override void ReadActiveCSVData()
    {
        base.ReadActiveCSVData();

        sensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 4);

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 14);
        currentShotCount = shotCount;

        shotInterval = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 19);
        shotDelay = new WaitForSeconds(shotInterval);

        radius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 27);
        originRadius = radius;

        slowDownValue = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 33);
        debuffDuration = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 34);
    }
    protected override void ReadEvolutionCSVData()
    {
        base.ReadEvolutionCSVData();

        sensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 4);

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 11);
        currentShotCount = shotCount;

        shotInterval = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 14);
        shotDelay = new WaitForSeconds(shotInterval);

        radius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 21);
        originRadius = radius;

        debuffDuration = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 33);
    }
}
