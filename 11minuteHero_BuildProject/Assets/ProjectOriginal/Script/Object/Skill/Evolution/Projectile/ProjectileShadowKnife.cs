using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShadowKnife : ProjectileKnife
{
    private float secondSensingRadius;
    private bool isReturn;

    private System.Action SetIsReturn;
    public override void SetAction(System.Action action)
    {
        SetIsReturn += action;
    }
    public override void SetSensingRadius(float radius)
    {
        secondSensingRadius = radius;
    }
    protected override IEnumerator Co_Shot()
    {
        while (true)
        {
            if (bFinding) yield return finding;
            transform.forward = ((target.position + Vector3.up * 0.5f) - transform.position).normalized;
            transform.position += transform.forward * speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, target.position + Vector3.up * 0.5f) < 0.5f)
            {
                if(isReturn)
                {
                    owner.ReturnProjectile(this);
                    isReturn = false;
                    SetIsReturn();
                    yield break;
                }
                if (target.TryGetComponent(out Monster c))
                {
                    if(!c.IsDie)
                    {
                        c.Hit(damage);
#if UNITY_EDITOR
                        InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
#endif
                    }
                }
                FindNewTarget();
            }
            yield return null;
        }
    }
    protected override void FindNewTarget()
    {
        bFinding = true;
        int num = Physics.OverlapSphereNonAlloc(transform.position, sensingRadius, newTargetCollisionArray, ConstDefine.LAYER_MONSTER);
        if (num == 0 || num == 1 && newTargetCollisionArray[0].transform == target)
        {
            int numNearPlayer = Physics.OverlapSphereNonAlloc(InGameManager.Instance.Player.transform.position, secondSensingRadius, newTargetCollisionArray, ConstDefine.LAYER_MONSTER);
            if(numNearPlayer == 0 || numNearPlayer == 1 && newTargetCollisionArray[0].transform == target)
            {
                target = InGameManager.Instance.Player.transform;
                isReturn = true;
                bFinding = false;
                Debug.Log("플레이어한테 돌아감");
                return;
            }
            num = numNearPlayer;
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
