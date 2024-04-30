using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveFireBall : ActiveSkillUsingProjectile //���̾ ��ų Ŭ����
{
    [SerializeField] private float activateTime; //Ȱ�� �ð�
    [SerializeField] private float sensingRadius; //���� �ݰ�
    [SerializeField] protected int penetrationCount;
    [SerializeField] protected int currentPenetrationCount;
    [SerializeField] private float increaseSizeValue;

    private Collider[] sensingCollisionArray = new Collider[100];
    protected float yOffset; //���� ����ü ������ ���� y ������ ������

    protected override void UpdateSkillData()
    {
        base.UpdateSkillData();

        currentPenetrationCount = penetrationCount + level;
        projectileUtility.SetPenetrationCount(currentPenetrationCount);
        projectileUtility.IncreaseSize(increaseSizeValue);

        yOffset += increaseSizeValue / 2;
    }
    protected override void SetCurrentRange(float value)
    {
        float increaseValue = 1 * value / 100;
        projectileUtility.IncreaseSize(increaseValue);
        yOffset += increaseValue / 2;
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

        if (!projectileUtility.IsValid())
        {
            InitProjectile();
        }
        Projectile p = projectileUtility.GetProjectile();
        p.SetShotDirection((targetPos - p.transform.position).normalized);

        p.transform.position += Vector3.up * yOffset; //������ �����Ҽ��� y ��ġ�� ���ݾ� ���� �÷���

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
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.ReduceCoolTime) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.HellFire);
            bCanEvolution = true;
        }
    }
    protected override void ReadCSVData()
    {
        base.ReadCSVData();

        if (eSkillType == ESkillType.Evolution) return;
        sensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 4);

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 13);
        currentShotCount = shotCount;

        increaseSizeValue = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 16);
        activateTime = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 17);
        speed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 18);

        penetrationCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 21);
        currentPenetrationCount = penetrationCount;

    }
}
