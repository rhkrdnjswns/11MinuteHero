using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon1 : Skill //모든 무기 클래스들의 부모 클래스. (09/25)액티브 스킬 제작 완료 후 리팩토링 시에 Weapon으로 클래스 이름 바꿔야함
{
    public enum eWeaponType //무기 타입
    {
        Range,
        Melee
    }
    [SerializeField] private eWeaponType weaponType; //무기에 따른 애니메이션 재생을 위한 무기 타입 나누기


    private ParticleSystem maxLevelParticle; //무기 최대 레벨 도달 시 플레이될 파티클
    [SerializeField] private GameObject maxLevelParticlePrefab; //무기 강화가 최대치에 도달했을 때 재생될 파티클

    [SerializeField] protected float coolTime; //무기 쿨타임
    private float originCoolTime; //무기 기본 쿨타임
    [SerializeField] protected float increaseDamage; //공격력 증가치
    [SerializeField] protected float damage; //공격력

    public eWeaponType WeaponType { get => weaponType; }

    public float CoolTime { get => coolTime; }


    public virtual void InitWeapon() //플레이어의 Start에서 호출됨. 따라서 무기 데이터 초기화를 해당 함수에서 진행
    {
        CreateMaxLevelParticle();
        originCoolTime = coolTime;
        level = 1;
        UpdateDamage();
    }
    public virtual void UpdateDamage() //무기 데미지 갱신. 플레이어 공격력 * (무기 공격력 증가치 * 무기 레벨)
    {
        damage = increaseDamage * level;
    }
    public virtual void UpdateRange(float percentage) //무기 종류에 맞게 사거리 갱신
    {
        //Do Something
    }
    public virtual void Attack() //공격 함수 (자식객체에서 전부 재정의)
    {
        return;
    }
    public void ReduceCoolTime(float percentage) //무기 쿨타임 감소 함수
    {
        coolTime -= originCoolTime * percentage / 100;
    }
    private void CreateMaxLevelParticle() //무기 최대레벨 파티클 생성
    {
        GameObject obj = Instantiate(maxLevelParticlePrefab);

        obj.transform.SetParent(InGameManager.Instance.Player.transform);
        maxLevelParticle = obj.GetComponent<ParticleSystem>();
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
