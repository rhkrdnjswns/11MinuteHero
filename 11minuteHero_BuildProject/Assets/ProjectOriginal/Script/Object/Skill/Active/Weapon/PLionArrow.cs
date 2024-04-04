using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLionArrow : PPenetrationProjectile
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER))
        {
            other.GetComponent<Character>().Hit(rangedAttackUtility.ProjectileDamage); //Monster Ŭ������ �����Ͽ� ������ ����
            currentCount--;
            InGameManager.Instance.Player.Weapon.SetSpeedUp();

            if (currentCount > 0) return;
            currentCount = count; //���� Ƚ���� ���� �Ҹ��� ��� ����ü ȸ��
            rangedAttackUtility.ReturnProjectile(this);
        }
    }
}
