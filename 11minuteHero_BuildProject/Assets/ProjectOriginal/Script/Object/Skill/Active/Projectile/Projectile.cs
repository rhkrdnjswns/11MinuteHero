using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour //투사체 클래스. 화살, 단검 등 투사체들은 해당 클래스를 상속받을 거임. //투사체는 Weapon과 별개의 객체로 관리
{
    protected Vector3 shotDirection; //투사체가 날아갈 방향

    protected float damage;
    protected float speed;
    protected IProjectileUsable owner;
    protected string targetTag;
#if UNITY_EDITOR
    public int index;
#endif
    public bool IsMaxLevel { get; set; } //투사체가 무기의 최대 레벨 특수 효과를 사용하기 위해 체크

    #region ---------모든 투사체 공통사항--------------
    public virtual void SetOwner(IProjectileUsable owner)
    {
        this.owner = owner;
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    public void SetTargetTag(string tag)
    {
        targetTag = tag;
    }
    #endregion

    public virtual void SetPenetrationCount(int count)
    {
        //관통형 투사체의 경우 관통횟수 설정
    }
    public virtual void SetActivateTime(float time)
    {
        //활성화 시간이 정해진 투사체의 경우 활성화 시간 설정
    }
    public virtual void SetBounceCount(int count)
    {
        //튕기는 투사체의 경우 튕기는 횟수 설정
    }
    public virtual void SetTargetTransform(Transform transform)
    {
        //유도형 투사체의 경우 타겟 참조 설정
    }
    public virtual void SetDistance(float distance)
    {
        //날아가는 거리가 정해진 투사체의 경우 거리 설정
    }
    public virtual void SetAttackRadius(float radius)
    {
        //반경 공격을 하는 투사체의 경우 반경 설정
    }
    public virtual void SetRotationSpeed(float speed)
    {
        //자전하는 투사체의 경우 자전 속도 설정
    }
    public virtual void SetSensingRadius(float radius)
    {
        //주변 반경을 감지하는 투사체의 경우 감지 반경 설정
    }
    public virtual void SetDotData(float dotInterval, float duration)
    {
        //도트 데미지를 입히는 투사체의 경우 데미지 주기와 지속시간 설정
    }
    public virtual void SetCount(int count)
    {
        //관통형 투사체의 경우 관통횟수 설정, 단검의 경우 튕기는 횟수 설정
    }
    public virtual void SetTargetPos(Vector3 pos)
    {
        //특정 위치로 날아가는 투사체의 경우 위치 설정
    }
    public virtual void SetExplosionDamage(float damage)
    {
        //폭발하는 투사체의 경우 폭발 데미지 설정
    }
    public virtual void SetAngle(float angle)
    {
        //포물선 운동을 하는 투사체의 각도 설정
    }
    public virtual void SetMotion(Vector3 pos)
    {
        //포물선 운동을 하는 투사체의 경우 목표 지점까지의 포물선 운동 관련 수치 설정
    }
    public virtual void SetSlowDownData(float value, float duration)
    {
        //적중 시 이동속도 저하를 적용하는 투사체의 경우 감소치와 지속시간 설정
    }
    public virtual void SetStunDuration(float duration)
    {
        //적중 시 기절시키는 투사체의 경우 기절 지속시간 설정
    }
    public virtual void SetAction(System.Action action)
    {
        //특정 이벤트를 발신해야 하는 투사체의 경우 이벤트 설정
    }
    public virtual void ShotProjectile() //투사체 발사
    {
        StartCoroutine(Co_Shot());
    }
    public void SetShotDirection(Vector3 direction) //날아갈 방향 설정
    {
        shotDirection = direction;
    }
    public virtual void IncreaseSize(float value)
    {
        transform.localScale += Vector3.one * value;
    }
    protected abstract IEnumerator Co_Shot(); //투사체가 날아가는 코루틴

    protected virtual void OnTriggerEnter(Collider other) //투사체 충돌 처리
    {
        if (other.CompareTag(targetTag)) 
        {
            other.GetComponent<Character>().Hit(damage); //Monster 클래스를 추출하여 데미지 연산
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
#endif
            owner.ReturnProjectile(this); //이후 풀로 되돌림
            // -> 필드 위에 많은 수의 몬스터가 존재하기 때문에 충돌한 몬스터를 검출하는 게 몬스터 리스트를 순회하는 작업보다 성능면에서 효율적임
        }
    }
}
