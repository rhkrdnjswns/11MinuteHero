using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHolySword : WSword
{
    [SerializeField] private float hpRecovery;
    public override void InitSkill()
    {
        base.InitSkill();

        main.startSize = attackRadiusUtility.Radius * 2;
        InGameManager.Instance.Player.ChangeWeapon(this);
    }
    protected override IEnumerator Co_AttackInRangeMonster()
    {
        Collider[] inRadiusMonsterArray = attackRadiusUtility.GetLayerInRadius(transform.root);
        if (inRadiusMonsterArray.Length == 0) yield break; //범위 안에 몬스터 없으면 종료

        foreach (var monster in inRadiusMonsterArray) //범위 안 몬스터 중 공격 가능한 각도에 있는 몬스터 검사
        {
            Vector3 targetDir = (monster.transform.position - transform.root.position).normalized; //타겟 방향 벡터 정규화.
            //Vector3.Dot()를 통해 플레이어랑 타겟의 각도를 구함.           
            float targetAngle = Mathf.Acos(Vector3.Dot(transform.root.forward, targetDir)) * Mathf.Rad2Deg; //Acos의 반환값이 호도(radian)이기 때문에, attackAngle과 비교를 위해
                                                                                                            //각으로 바꿔주기 위해 상수를 곱해줌
            if (targetAngle <= attackAngle * 0.5f) //양옆으로로 나뉘기 때문에 0.5 곱함. 바로보고있는 방향을 기준으로 양 옆으로 각이 펼쳐지기 때문에
            {
                monster.GetComponent<Monster>().Hit(currentDamage); //각도 내에 있는 경우 타격
                InGameManager.Instance.Player.RecoverHp(hpRecovery, EApplicableType.Value);
            }
            yield return null;
        }
    }
}
