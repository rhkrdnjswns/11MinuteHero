using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ABattleBluntWeapon : AActiveSkill //전투망치와 전투도끼 클래스
{
    public enum EBattleBluntWeaponType //무기 타입.
    {
        Hammer,
        Ax,
        ThunderStrike
    }
    [SerializeField] protected RangedAttackUtility rangedAttackUtility;
    [SerializeField] private float distance; //날아갈 거리. distance만큼 날아갔다가 돌아옴
    [SerializeField] private EBattleBluntWeaponType type;

    private int returnCount;
    public int ReturnCount { get => returnCount; set => returnCount = value; }
    private Vector3[] directionArray;
    protected override void Awake()
    {
        base.Awake();
        rangedAttackUtility.Parent = transform;
        rangedAttackUtility.CreateNewProjectile();
        if (type == EBattleBluntWeaponType.ThunderStrike) directionArray = new Vector3[8];
        else directionArray = new Vector3[4];

        foreach(var item in rangedAttackUtility.AllProjectileList)
        {
            item.GetComponent<PBattleBluntWeapon>().SetBattalBluntWeapon(this);
        }
    }
    public override void InitSkill() //초기화 함수 재정의
    {
        base.InitSkill();
        rangedAttackUtility.SetDistance(distance); //전투망치, 도끼는 날아가는 거리가 정해져있기 때문에 거리 설정
        rangedAttackUtility.ShotCount = 4;

        SetCurrentDamage();
    }
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
        rangedAttackUtility.IncreaseSize(10f);
    }
    protected override void SetCurrentDamage()
    {
        currentDamage = damage * level;
        rangedAttackUtility.SetDamage(currentDamage);
    }
    protected override void SetCurrentRange(float value)
    {
        rangedAttackUtility.IncreaseSize(value);
    }
    private void ShotProjectile()
    {
#if UNITY_EDITOR
        AttackCount++;
#endif
        returnCount = 0;
        SetDirectionArrayByType(); //현재 플레이어의 앞방향을 기준으로 타입에 맞는 사출 방향 결정

        for (int i = 0; i < rangedAttackUtility.ShotCount; i++) //ShotCount만큼 투사체 발사
        {
            if (!rangedAttackUtility.IsValid())
            {
                rangedAttackUtility.CreateNewProjectile();
                rangedAttackUtility.SetDistance(distance);
            }
            Projectile p = rangedAttackUtility.SummonProjectile();
            p.SetShotDirection(directionArray[i]);

            p.ShotProjectile();
        }
    }
    protected override IEnumerator Co_ActiveSkillAction() //액티브 스킬 기능 작동 코루틴. 쿨타임 마다 투사체를 날림
    {
        while (true)
        {
            yield return new WaitForSeconds(CurrentCoolTime);
            ShotProjectile();
            yield return new WaitUntil(() => returnCount == rangedAttackUtility.ShotCount);
        }
    }
    private void SetDirectionArrayByType() //전투망치와 전투도끼의 투사체 방향 설정
    {
        switch (type)
        {
            case EBattleBluntWeaponType.Hammer: //플레이어 기준 동 서 남 북 방향으로 나감
                directionArray[0] = transform.root.forward;
                directionArray[1] = transform.root.right;
                directionArray[2] = transform.root.forward * -1;
                directionArray[3] = transform.root.right * -1;
                break;
            case EBattleBluntWeaponType.Ax: //플레이어 기준 북동 북서 남동 남서 방향으로 나감
                directionArray[0] = (transform.root.forward + transform.root.right).normalized;
                directionArray[1] = (transform.root.forward * -1 + transform.root.right).normalized;
                directionArray[2] = (transform.root.forward * -1 + transform.root.right * -1).normalized;
                directionArray[3] = (transform.root.forward + transform.root.right * -1).normalized;
                break;
            case EBattleBluntWeaponType.ThunderStrike: //진화스킬인 천둥강타는 8방향으로 전부 나감
                directionArray[0] = transform.root.forward;
                directionArray[1] = transform.root.right;
                directionArray[2] = transform.root.forward * -1;
                directionArray[3] = transform.root.right * -1;
                directionArray[4] = (transform.root.forward + transform.root.right).normalized;
                directionArray[5] = (transform.root.forward * -1 + transform.root.right).normalized;
                directionArray[6] = (transform.root.forward * -1 + transform.root.right * -1).normalized;
                directionArray[7] = (transform.root.forward + transform.root.right * -1).normalized;
                break;
            default:
                break;
        }
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
