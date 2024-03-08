using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABomb : AActiveSkill
{
    [SerializeField] protected RangedAttackUtility rangedAttackUtility;
    [SerializeField] protected AttackRadiusUtility attackRadiusUtility;
    [SerializeField] protected AttackRadiusUtility bombAttackRadiusUtility;
    [SerializeField] protected float shotInterval = 0.3f;
    [SerializeField] private float secondDamage;
    [Range(1, 89)]
    [SerializeField] protected float angle;
    private float secondBaseDamage;
    protected bool bShotDone;

    private float originRadius;
    protected override void Awake()
    {
        base.Awake();
        originRadius = bombAttackRadiusUtility.Radius;
        rangedAttackUtility.Parent = transform;
        rangedAttackUtility.CreateNewProjectile(bombAttackRadiusUtility);
        SetBombProjectile();
        secondBaseDamage = secondDamage;
    }
    protected virtual void SetBombProjectile()
    {
        foreach (var item in rangedAttackUtility.AllProjectileList)
        {
            item.GetComponent<PBomb>().SetAngle(angle);
        }
    }
    public override void IncreaseDamage(float value)
    {
        increaseDamage += value; //현재 데미지 증가치 갱신
        damage += baseDamage * value / 100; //합연산 데미지 증가시킴
        secondDamage += secondBaseDamage * value / 100;
        SetCurrentDamage();
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
        currentDamage = damage + secondDamage * level;
        rangedAttackUtility.SetDamage(currentDamage);
    }
    protected override void SetCurrentRange(float value)
    {
        bombAttackRadiusUtility.Radius += originRadius * value / 100f;
        rangedAttackUtility.IncreaseSize(value);
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentCoolTime);
            StartCoroutine(Co_AttackInRadius());
            yield return new WaitUntil(() => bShotDone);
        }
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

        int monsterIndex = Random.Range(0, inRadiusMonsterArray.Length);
        Transform t;
        Projectile p;
        for (int i = 0; i < rangedAttackUtility.ShotCount; i++)
        {
            if (!rangedAttackUtility.IsValid())
            {
                rangedAttackUtility.CreateNewProjectile(bombAttackRadiusUtility);
                SetBombProjectile();
            }
            p = rangedAttackUtility.SummonProjectile();
            t = inRadiusMonsterArray[monsterIndex++].transform;
            p.ShotProjectile(t.position);

            if (monsterIndex >= inRadiusMonsterArray.Length) monsterIndex = 0;
            
            if (i == rangedAttackUtility.ShotCount - 1) break;
            
            yield return new WaitForSeconds(shotInterval);
        }
        bShotDone = true;
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
}
