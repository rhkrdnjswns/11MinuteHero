using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ABattleBluntWeapon : AActiveSkill //������ġ�� �������� Ŭ����
{
    public enum EBattleBluntWeaponType //���� Ÿ��.
    {
        Hammer,
        Ax,
        ThunderStrike
    }
    [SerializeField] protected RangedAttackUtility rangedAttackUtility;
    [SerializeField] private float distance; //���ư� �Ÿ�. distance��ŭ ���ư��ٰ� ���ƿ�
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
    public override void InitSkill() //�ʱ�ȭ �Լ� ������
    {
        base.InitSkill();
        rangedAttackUtility.SetDistance(distance); //������ġ, ������ ���ư��� �Ÿ��� �������ֱ� ������ �Ÿ� ����
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
        SetDirectionArrayByType(); //���� �÷��̾��� �չ����� �������� Ÿ�Կ� �´� ���� ���� ����

        for (int i = 0; i < rangedAttackUtility.ShotCount; i++) //ShotCount��ŭ ����ü �߻�
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
    protected override IEnumerator Co_ActiveSkillAction() //��Ƽ�� ��ų ��� �۵� �ڷ�ƾ. ��Ÿ�� ���� ����ü�� ����
    {
        while (true)
        {
            yield return new WaitForSeconds(CurrentCoolTime);
            ShotProjectile();
            yield return new WaitUntil(() => returnCount == rangedAttackUtility.ShotCount);
        }
    }
    private void SetDirectionArrayByType() //������ġ�� ���������� ����ü ���� ����
    {
        switch (type)
        {
            case EBattleBluntWeaponType.Hammer: //�÷��̾� ���� �� �� �� �� �������� ����
                directionArray[0] = transform.root.forward;
                directionArray[1] = transform.root.right;
                directionArray[2] = transform.root.forward * -1;
                directionArray[3] = transform.root.right * -1;
                break;
            case EBattleBluntWeaponType.Ax: //�÷��̾� ���� �ϵ� �ϼ� ���� ���� �������� ����
                directionArray[0] = (transform.root.forward + transform.root.right).normalized;
                directionArray[1] = (transform.root.forward * -1 + transform.root.right).normalized;
                directionArray[2] = (transform.root.forward * -1 + transform.root.right * -1).normalized;
                directionArray[3] = (transform.root.forward + transform.root.right * -1).normalized;
                break;
            case EBattleBluntWeaponType.ThunderStrike: //��ȭ��ų�� õ�հ�Ÿ�� 8�������� ���� ����
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
