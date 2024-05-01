using System.Collections;
using UnityEngine;

public class ActiveBattleBluntWeapon : ActiveSkillUsingProjectile //전투망치와 전투도끼 클래스
{
    public enum EBattleBluntWeaponType //무기 타입.
    {
        Hammer,
        Ax,
        ThunderStrike
    }
    [SerializeField] private EBattleBluntWeaponType type;

    [SerializeField] private float distance;
    [SerializeField] private float increaseSizeValue;
    [SerializeField] private float rotateSpeed;
    private float knockBackSpeed;
    private float knockBackDuration;

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
        //SetDirectionArrayByType(); //현재 플레이어의 앞방향을 기준으로 타입에 맞는 사출 방향 결정

        for (int i = 0; i < currentShotCount; i++) //ShotCount만큼 투사체 발사
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
    protected override IEnumerator Co_ActiveSkillAction() //액티브 스킬 기능 작동 코루틴. 쿨타임 마다 투사체를 날림
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
        projectileUtility.SetKnockBackData(knockBackSpeed, knockBackDuration);
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
    protected override void ReadActiveCSVData()
    {
        base.ReadActiveCSVData();

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 13);
        currentShotCount = shotCount;

        increaseSizeValue = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 16);
        speed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 18);
        rotateSpeed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 22);
        distance = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 26);

        knockBackSpeed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 35);
        knockBackDuration = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 36);
    }
    protected override void ReadEvolutionCSVData()
    {
        base.ReadEvolutionCSVData();

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 11);
        currentShotCount = shotCount;

        speed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 13);
        rotateSpeed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 16);
        distance = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 20);

        knockBackSpeed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 34);
        knockBackDuration = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 35);
    }
}
