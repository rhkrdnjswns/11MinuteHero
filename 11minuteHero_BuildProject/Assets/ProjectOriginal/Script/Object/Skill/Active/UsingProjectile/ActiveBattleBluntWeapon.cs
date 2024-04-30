using System.Collections;
using UnityEngine;

public class ActiveBattleBluntWeapon : ActiveSkillUsingProjectile //������ġ�� �������� Ŭ����
{
    public enum EBattleBluntWeaponType //���� Ÿ��.
    {
        Hammer,
        Ax,
        ThunderStrike
    }
    [SerializeField] private EBattleBluntWeaponType type;

    [SerializeField] private float distance;
    [SerializeField] private float increaseSizeValue;
    [SerializeField] private float rotateSpeed;

    private int returnCount;
    protected override void UpdateSkillData()
    {
        base.UpdateSkillData();
        projectileUtility.IncreaseSize(increaseSizeValue);
    }
    private void ShotProjectile()
    {
#if UNITY_EDITOR
        AttackCount++;
#endif
        returnCount = 0;
        //SetDirectionArrayByType(); //���� �÷��̾��� �չ����� �������� Ÿ�Կ� �´� ���� ���� ����

        for (int i = 0; i < currentShotCount; i++) //ShotCount��ŭ ����ü �߻�
        {
            if (!projectileUtility.IsValid())
            {
                InitProjectile();
            }
            Projectile p = projectileUtility.GetProjectile();
            p.SetShotDirection(GetDirection(i));

            p.ShotProjectile();
        }
    }
    protected override IEnumerator Co_ActiveSkillAction() //��Ƽ�� ��ų ��� �۵� �ڷ�ƾ. ��Ÿ�� ���� ����ü�� ����
    {
        WaitUntil IsReturn = new WaitUntil(() => returnCount == currentShotCount);
        while (true)
        {
            yield return coolTimeDelay;

            ShotProjectile();

            yield return IsReturn;
        }
    }
    private Vector3 GetDirection(int count)
    {
        float degree = 360 / currentShotCount;
        Vector3 direction;
        Quaternion rot = Quaternion.Euler(0, degree * count, 0);
        if (type == EBattleBluntWeaponType.Ax)
        {
            direction = rot * (transform.root.forward + transform.root.right).normalized;
        }
        else
        {
            direction = rot * transform.root.forward;
        }

        return direction;
    }
    protected override void InitProjectile()
    {
        base.InitProjectile();
        projectileUtility.SetRotateSpeed(rotateSpeed);
        projectileUtility.SetDistance(distance);
        projectileUtility.SetAction(() => returnCount++);
    }
    public override void SetEvlotionCondition()
    {
        switch (type)
        {
            case EBattleBluntWeaponType.Hammer:
                if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.BattleAx).Level == ConstDefine.SKILL_MAX_LEVEL)
                {
                    InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.ThunderStrike);
                    bCanEvolution = true;
                }
                break;
            case EBattleBluntWeaponType.Ax:
                if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetActiveSkill((int)ESkillActiveID.BattleHammer).Level == ConstDefine.SKILL_MAX_LEVEL)
                {
                    InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.ThunderStrike);
                    bCanEvolution = true;
                }
                break;
            default:
                //Debug.LogError("UnDefined Type");
                break;
        }
    }
}