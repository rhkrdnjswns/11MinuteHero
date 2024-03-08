using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon2 : Skill
{
    public enum eWeaponType //무기 타입
    {
        Range,
        Melee
    }
    [SerializeField] private eWeaponType weaponType;

    private ParticleSystem maxLevelParticle;
    [SerializeField] private GameObject maxLevelParticlePrefab; //무기 강화가 최대치에 도달했을 때 재생될 파티클

    [SerializeField] protected float weaponCoolTime; //무기 쿨타임
    [SerializeField] protected float increaseDamage; //공격력 증가치
    [SerializeField] protected float damage; //공격력

    public virtual void UpdateDamage()
    {
        damage = increaseDamage * level;
    }
    public void ReduceCoolTime(float percentage) //무기 쿨타임 감소 함수
    {
        weaponCoolTime -= weaponCoolTime * percentage / 100;
    }
    private void CreateMaxLevelParticle() //무기 최대레벨 파티클 생성
    {
        GameObject obj = Instantiate(maxLevelParticlePrefab);

        obj.transform.SetParent(InGameManager.Instance.Player.transform);
        maxLevelParticle = obj.GetComponent<ParticleSystem>();
    }
    public float WeaponCoolTime { get => weaponCoolTime; }
    public eWeaponType WeaponType { get => weaponType; }

    public virtual void Attack() //공격 함수 (자식객체에서 전부 재정의)
    {
        return;
    }

    protected override void UpdateSkillData()
    {
        throw new System.NotImplementedException();
    }

    public override void SetEvlotionCondition()
    {
        throw new System.NotImplementedException();
    }
}
