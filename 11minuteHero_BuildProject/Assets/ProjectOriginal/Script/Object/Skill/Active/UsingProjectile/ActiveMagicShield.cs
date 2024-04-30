using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMagicShield : ActiveSkillUsingProjectile
{
    [SerializeField] private float shotInterval;
    [SerializeField] protected float activateTime;
    [SerializeField] protected float rotateSpeed;

    private WaitForSeconds shotDelay;
    public override void ActivateSkill()
    {
        base.ActivateSkill();

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
            if(i < currentShotCount - 1) yield return shotDelay; //������ ����ü �߻� �ÿ��� �� ���� �ϱ�
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
    protected override void ReadCSVData()
    {
        base.ReadCSVData();

        if (eSkillType == ESkillType.Evolution) return;
        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 14);
        currentShotCount = shotCount;

        speed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 18);
        shotInterval = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 19);
        shotDelay = new WaitForSeconds(shotInterval);

        rotateSpeed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 22);
    }
}
