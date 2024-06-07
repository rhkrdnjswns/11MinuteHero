using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFireBall : AActiveSkill //���̾ ��ų Ŭ����
{
    [SerializeField] protected RangedAttackUtility rangedAttackUtility; 

    [SerializeField] private float activateTime; //Ȱ�� �ð�

    [SerializeField] private AttackRadiusUtility attackRadiusUtility;

    [SerializeField] protected int penetrationCount;

    protected float yOffset; //���� ����ü ������ ���� y ������ ������

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
        rangedAttackUtility.SetCount(penetrationCount + level);

        SetCurrentDamage();
    }
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
        rangedAttackUtility.SetCount(penetrationCount + level);
        rangedAttackUtility.IncreaseSize(13f);
        yOffset += 0.07f;
    }
    protected override void SetCurrentDamage()
    {
        CurrentDamage = damage * level;
        rangedAttackUtility.SetDamage(CurrentDamage);
    }
    protected override void SetCurrentRange(float value)
    {
        rangedAttackUtility.IncreaseSize(value);
        yOffset += 0.07f;
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(CurrentCoolTime);
            StartCoroutine(Co_AttackInRadius());
        }
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
            rangedAttackUtility.SetCount(9 + level);
        }
        Projectile p = rangedAttackUtility.SummonProjectile();
        p.SetShotDirection((t.position - transform.root.position).normalized);

        p.transform.position += Vector3.up * yOffset; //������ �����Ҽ��� y ��ġ�� ���ݾ� ���� �÷���

        p.ShotProjectile();
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.ReduceCoolTime) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.HellFire);
            bCanEvolution = true;
        }
    }
}
