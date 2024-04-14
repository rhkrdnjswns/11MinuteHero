using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WRangedWeapon : AWeapon //원거리 무기. (09/25)액티브 스킬 구현 끝낸 후 리팩토링 시에 RangedWeapon으로 클래스 이름 변경
{
    [SerializeField] protected RangedAttackUtility rangedAttackUtility;
    [SerializeField] protected float secondDamage;
    private float secondBaseDamage;
    private Coroutine attackCoroutine;

    protected override void Awake()
    {
        base.Awake();
        rangedAttackUtility.Parent = transform;
        rangedAttackUtility.CreateNewProjectile();
        secondBaseDamage = secondDamage;
    }
    public override void IncreaseDamage(float value)
    {
        increaseDamage += value; //현재 데미지 증가치 갱신
        damage += baseDamage * value / 100; //합연산 데미지 증가시킴
        secondDamage += secondBaseDamage * value / 100;
        SetCurrentDamage();
    }
    protected override void SetCurrentDamage()
    {
        currentDamage = damage + secondDamage * level;
        rangedAttackUtility.SetDamage(currentDamage);
    }
    public override void Attack() //공격 함수 오버라이딩
    {
        attackCoroutine = StartCoroutine(Co_Shot());
    }
    protected abstract IEnumerator Co_Shot(); //투사체 발사 코루틴
    protected override void SetCurrentRange(float value)
    {
        rangedAttackUtility.IncreaseSize(value);
    }
    public virtual void DoMaxLevelEffect() //무기 최대 레벨 달성 시 적용할 특수효과 재정의를 위한 함수
    {
        //Do Something in child
    }
}
