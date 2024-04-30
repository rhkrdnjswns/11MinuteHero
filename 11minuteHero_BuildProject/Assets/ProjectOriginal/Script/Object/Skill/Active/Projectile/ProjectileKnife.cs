using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileKnife : Projectile
{
    protected float bounceCount;
    protected float currentCount;
    protected float sensingRadius;

    protected Transform target;
    protected bool bFinding;

    protected WaitUntil finding;

    protected Collider[] newTargetCollisionArray = new Collider[50];
    public override void SetBounceCount(int count)
    {
        bounceCount = count;
    }
    public override void SetTargetTransform(Transform transform)
    {
        target = transform;
        this.transform.forward = ((target.position + Vector3.up * 0.5f) - transform.position).normalized;
    }
    public override void SetAttackRadius(float radius) //attackRadius로 감지범위 설정 (진화스킬에 두 번째 감지범위가 있어서)
    {
        sensingRadius = radius;
    }
    protected override IEnumerator Co_Shot()
    {
        currentCount = bounceCount;
        while(true)
        {
            if(bFinding) yield return finding;

            transform.forward = ((target.position + Vector3.up * 0.5f) - transform.position).normalized;
            transform.position += transform.forward * speed * Time.deltaTime;

            if(Vector3.Distance(transform.position, target.position + Vector3.up * 0.5f) < 0.5f)
            {
                if (target.TryGetComponent(out Character c))
                {
                    if(!c.IsDie)
                    {
                        c.Hit(damage);
#if UNITY_EDITOR
                        InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
#endif
                    }
                }

                if (currentCount <= 0) owner.ReturnProjectile(this);

                FindNewTarget();
                currentCount--;
            }
            yield return null;
        }
    }
    protected virtual void FindNewTarget()
    {
        bFinding = true;
        int num = Physics.OverlapSphereNonAlloc(transform.position, sensingRadius, newTargetCollisionArray, ConstDefine.LAYER_MONSTER);
        if(num == 0 || num == 1 && newTargetCollisionArray[0].transform == target)
        {
            bFinding = false;
            owner.ReturnProjectile(this);
            return;
        }
       // Debug.Log("기존 타겟 : " + target.name);
        for (int i = 0; i < num; i++) //범위 내에서 현재 타겟이었던 몬스터 제외
        {
            if (newTargetCollisionArray[i].transform == target) //현재 타겟이었던 몬스터 인덱스에 배열 맨 뒤 몬스터 넣어줌
            {
                // Debug.Log("중복 타겟 : " + InRangeArray[i].name + ", 교체할 타겟 : " + InRangeArray[InRangeArray.Length - 1].name);
                newTargetCollisionArray[i] = newTargetCollisionArray[num - 1];
             //   Debug.Log("갱신된 타겟 : " + InRangeArray[i].name);
                break;
            }
        }

        int index = Random.Range(0, num - 1); //0 ~ 맨 뒤 - 1 만큼만 랜덤하게 선택해서 맨 뒤 몬스터 선택지가 두 개가 되지 않게 처리
        target = newTargetCollisionArray[index].transform;
       // Debug.Log("최종 선택된 타겟 : " + target.name);
        bFinding = false;
    }
}
