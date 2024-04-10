using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PKnife : Projectile
{
    protected float bounceCount;
    protected float currentCount;
    protected AttackRadiusUtility attackRadiusUtility; //폭탄 폭발 반경 참조
    protected Transform target;
    protected bool bFinding;
    public override void SetCount(int count)
    {
        bounceCount = count;
    }
    public override void SetAttackRadiusUtility(AttackRadiusUtility attackRadiusUtility)
    {
        this.attackRadiusUtility = attackRadiusUtility;
    }
    public override void ShotProjectile(Transform target)
    {
        currentCount = bounceCount;
        this.target = target;
        transform.forward = ((target.position + Vector3.up * 0.5f) - transform.position).normalized;
        base.ShotProjectile();
    }
    protected override IEnumerator Co_Shot()
    {
        while(true)
        {
            if (bFinding) yield return new WaitUntil(() => !bFinding);
            transform.forward = ((target.position + Vector3.up * 0.5f) - transform.position).normalized;
            transform.position += transform.forward * rangedAttackUtility.ProjectileSpeed * Time.deltaTime;
            if(Vector3.Distance(transform.position, target.position + Vector3.up * 0.5f) < 0.5f)
            {
                if (!target.GetComponent<Character>().IsDie)
                {
                    target.GetComponent<Character>().Hit(rangedAttackUtility.ProjectileDamage);
                }
                if (currentCount <= 0) rangedAttackUtility.ReturnProjectile(this);
                FindNewTarget();
                currentCount--;
            }
            yield return null;
        }
    }
    protected override void OnTriggerEnter(Collider other) //투사체 충돌 처리
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER)) //몬스터와 부딪힌 경우
        {
            other.GetComponent<Character>().Hit(rangedAttackUtility.ProjectileDamage); //Monster 클래스를 추출하여 데미지 연산
            if (currentCount < 0) rangedAttackUtility.ReturnProjectile(this);
            else
            {
                FindNewTarget();
                currentCount--;
            }
        }
    }
    protected virtual void FindNewTarget()
    {
        bFinding = true;
        Collider[] InRangeArray = attackRadiusUtility.GetLayerInRadius(transform);
        if(InRangeArray.Length == 0 || InRangeArray.Length == 1 && InRangeArray[0].transform == target)
        {
            bFinding = false;
            rangedAttackUtility.ReturnProjectile(this);
            return;
        }
       // Debug.Log("기존 타겟 : " + target.name);
        for (int i = 0; i < InRangeArray.Length; i++) //범위 내에서 현재 타겟이었던 몬스터 제외
        {
            if (InRangeArray[i].transform == target) //현재 타겟이었던 몬스터 인덱스에 배열 맨 뒤 몬스터 넣어줌
            {
               // Debug.Log("중복 타겟 : " + InRangeArray[i].name + ", 교체할 타겟 : " + InRangeArray[InRangeArray.Length - 1].name);
                InRangeArray[i] = InRangeArray[InRangeArray.Length - 1];
             //   Debug.Log("갱신된 타겟 : " + InRangeArray[i].name);
                break;
            }
        }

        int index = Random.Range(0, InRangeArray.Length - 1); //0 ~ 맨 뒤 - 1 만큼만 랜덤하게 선택해서 맨 뒤 몬스터 선택지가 두 개가 되지 않게 처리
        target = InRangeArray[index].transform;
       // Debug.Log("최종 선택된 타겟 : " + target.name);
        bFinding = false;
    }
}
