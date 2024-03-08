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
public abstract class Skill : MonoBehaviour //무기, 패시브, 액티브 스킬 등 모든 플레이어 스킬들이 상속받는 부모 클래스
{
    [SerializeField] protected ESkillType eSkillType;
    [SerializeField] protected int id; //스킬 고유 id. 스킬 구분은 id로 한다.

    protected int level; //스킬 레벨

    [SerializeField] protected Sprite icon; //스킬 아이콘
    [SerializeField] protected string description; //스킬 설명
    [SerializeField] protected string name; //스킬 이름

    public ESkillType ESkillType { get => eSkillType; }
    public Sprite Icon { get => icon; }
    public string Description { get => description; }
    public string Name { get => name; }
    public int Level { get => level; }
    public int Id { get => id; }

    public void Reinforce() //모든 스킬의 강화 함수. 세부 내용은 각 객체들이 재정의
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL) return;
        level++;
        UpdateSkillData();
    }
    public virtual void InitSkill() //기믹에 맞는 초기화 재정의
    {
        level = 1;
        transform.SetParent(InGameManager.Instance.Player.transform);
        transform.localPosition = Vector3.up * 0.5f;
        transform.localRotation = Quaternion.identity;
        Debug.Log($"{name} 추가"); //테스트

        gameObject.SetActive(true);
    }
    protected abstract void UpdateSkillData();
    public abstract void SetEvlotionCondition();
}
