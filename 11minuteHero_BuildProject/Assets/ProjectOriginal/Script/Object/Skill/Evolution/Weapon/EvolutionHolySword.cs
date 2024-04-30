using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionHolySword : ActiveSword
{
    [SerializeField] private float hpRecovery;
    public override void InitSkill()
    {
        base.InitSkill();
        InGameManager.Instance.Player.ChangeWeapon(this);
    }
    protected override void SetCurrentRange(float value)
    {
        float increaeValue = originRadius * value / 100;
        sensingRadius += increaeValue;
        particleSystem.transform.localScale += Vector3.one * (increaeValue * 0.5f);
    }
    protected override void AttackInRangeMonster()
    {
        int num = Physics.OverlapSphereNonAlloc(transform.root.position, sensingRadius, sensingCollisionArray, ConstDefine.LAYER_MONSTER);
        if (num == 0) return;
#if UNITY_EDITOR
        AttackCount++;
#endif
        for (int i = 0; i < num; i++)
        {
            Vector3 targetDir = (sensingCollisionArray[i].transform.position - transform.root.position).normalized; //타겟 방향 벡터 정규화.
            //Vector3.Dot()를 통해 플레이어랑 타겟의 각도를 구함.           
            float targetAngle = Mathf.Acos(Vector3.Dot(transform.root.forward, targetDir)) * Mathf.Rad2Deg; //Acos의 반환값이 호도(radian)이기 때문에, attackAngle과 비교를 위해
                                                                                                            //각으로 바꿔주기 위해 상수를 곱해줌
            if (targetAngle <= attackAngle * 0.5f) //양옆으로로 나뉘기 때문에 0.5 곱함. 바로보고있는 방향을 기준으로 양 옆으로 각이 펼쳐지기 때문에
            {
                sensingCollisionArray[i].GetComponent<Character>()?.Hit(currentDamage); //각도 내에 있는 경우 타격
#if UNITY_EDITOR
                TotalDamage += currentDamage;
#endif
                InGameManager.Instance.Player.RecoverHp(hpRecovery, EApplicableType.Value);
            }
        }
    }
}
