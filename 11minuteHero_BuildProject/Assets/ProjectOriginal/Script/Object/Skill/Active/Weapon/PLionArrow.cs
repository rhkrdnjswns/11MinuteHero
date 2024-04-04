using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLionArrow : PPenetrationProjectile
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER))
        {
            other.GetComponent<Character>().Hit(rangedAttackUtility.ProjectileDamage); //Monster 클래스를 추출하여 데미지 연산
            currentCount--;
            InGameManager.Instance.Player.Weapon.SetSpeedUp();

            if (currentCount > 0) return;
            currentCount = count; //관통 횟수를 전부 소모한 경우 투사체 회수
            rangedAttackUtility.ReturnProjectile(this);
        }
    }
}
