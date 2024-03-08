using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESkillType
{
    Active = 0,
    Passive,
    Evolution
}
public enum ESkillActiveID
{
    Sword = 1,
    Bow,
    Staff,
    BattleHammer,
    BattleAx,
    Aurora,
    Wisp,
    FireBall,
    Meteor,
    EarthSpell,
    BloodSword,
    CurseSword,
    CrystalStaff,
    FireBomb,
    ShockBomb,
    MagicShield,
    Knife
}
public enum ESkillPassiveID
{
    Recovery = 18,
    IncreaseExpGain,
    ReduceDamage,
    IncreaseHp,
    IncreaseMoveSpeed,
    IncreaseDamage,
    ReduceCoolTime,
    IncreaseItemGain,
    IncreaseAttackRange
}
public enum ESkillEvolutionIndex
{
    HolySword = 0,
    LionBow,
    MagicMissile,
    ThunderStrike,
    SoulHarvest,
    FireStorm,
    HellFire,
    MeteorShower,
    EarthImplosion,
    DevilSword,
    MagicArrow,
    HolyWater,
    FaintBomb,
    Renotoros,
    ShadowKnife
}
public abstract class Skill : MonoBehaviour //����, �нú�, ��Ƽ�� ��ų �� ��� �÷��̾� ��ų���� ��ӹ޴� �θ� Ŭ����
{
    [SerializeField] protected ESkillType eSkillType;
    [SerializeField] protected int id; //��ų ���� id. ��ų ������ id�� �Ѵ�.

    protected int level; //��ų ����

    [SerializeField] protected Sprite icon; //��ų ������
    [SerializeField] protected string description; //��ų ����
    [SerializeField] protected string name; //��ų �̸�

    public ESkillType ESkillType { get => eSkillType; }
    public Sprite Icon { get => icon; }
    public string Description { get => description; }
    public string Name { get => name; }
    public int Level { get => level; }
    public int Id { get => id; }

    public void Reinforce() //��� ��ų�� ��ȭ �Լ�. ���� ������ �� ��ü���� ������
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL) return;
        level++;
        UpdateSkillData();
    }
    public virtual void InitSkill() //��Ϳ� �´� �ʱ�ȭ ������
    {
        level = 1;
        transform.SetParent(InGameManager.Instance.Player.transform);
        transform.localPosition = Vector3.up * 0.5f;
        transform.localRotation = Quaternion.identity;
        Debug.Log($"{name} �߰�"); //�׽�Ʈ

        gameObject.SetActive(true);
    }
    protected abstract void UpdateSkillData();
    public abstract void SetEvlotionCondition();
}
