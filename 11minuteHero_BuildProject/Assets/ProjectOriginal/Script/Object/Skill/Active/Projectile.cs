using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour //투사체 클래스. 화살, 단검 등 투사체들은 해당 클래스를 상속받을 거임. //투사체는 Weapon과 별개의 객체로 관리
{
    protected RangedAttackUtility rangedAttackUtility; //투사체를 발사한 원거리스킬 참조
    protected Vector3 shotDirection; //투사체가 날아갈 방향
    
    public bool IsMaxLevel { get; set; } //투사체가 무기의 최대 레벨 특수 효과를 사용하기 위해 체크
    public virtual void SetCount(int count)
    {
        //관통형 투사체의 경우 관통횟수 설정, 단검의 경우 튕기는 횟수 설정
    }
    public virtual void SetActivateTime(float time)
    {
        //일정 시간이 지난 후 사라지는 투사체의 경우 시간 설정
    }
    public virtual void SetDistance(float distance)
    {
        //날아가는 거리가 정해진 투사체의 경우 거리 설정
    }
    public virtual void SetAttackRadiusUtility(AttackRadiusUtility attackRadiusUtility)
    {
        //반경 공격을 하는 투사체의 경우 참조 설정
    }
    public virtual void SetRangeAttackUtility(RangedAttackUtility rangedSkill) //투사체 초기화
    {
        rangedAttackUtility = rangedSkill;
    }
    public void SetShotDirection(Vector3 direction) //날아갈 방향 설정
    {
        shotDirection = direction;
    }
    public virtual void IncreaseSize(float value)
    {
        transform.localScale += Vector3.one * (value / 100f);
    }
    public virtual void ShotProjectile(Transform target) //투사체 발사 (유도형 투사체의 경우 target을 매개변수로 받기 위한 오버로딩)
    {
        StartCoroutine(Co_Shot());
    }
    public virtual void ShotProjectile(Vector3 pos) //폭탄류 투사체를 특정 위치로 날리는 경우 호출
    {
        StartCoroutine(Co_Shot());
    }
    public virtual void ShotProjectile() //투사체 발사
    {
        StartCoroutine(Co_Shot());
    }
    protected abstract IEnumerator Co_Shot(); //투사체가 날아가는 코루틴

    protected virtual void OnTriggerEnter(Collider other) //투사체 충돌 처리
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER)) //몬스터와 부딪힌 경우
        {
            other.GetComponent<Monster>().Hit(rangedAttackUtility.ProjectileDamage); //Monster 클래스를 추출하여 데미지 연산
            rangedAttackUtility.ReturnProjectile(this); //이후 풀로 되돌림
            // -> 필드 위에 많은 수의 몬스터가 존재하기 때문에 충돌한 몬스터를 검출하는 게 몬스터 리스트를 순회하는 작업보다 성능면에서 효율적임
        }
    }
}
