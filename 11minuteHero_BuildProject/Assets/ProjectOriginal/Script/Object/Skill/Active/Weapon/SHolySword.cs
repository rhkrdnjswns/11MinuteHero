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
        if (inRadiusMonsterArray.Length == 0) yield break; //���� �ȿ� ���� ������ ����

        foreach (var monster in inRadiusMonsterArray) //���� �� ���� �� ���� ������ ������ �ִ� ���� �˻�
        {
            Vector3 targetDir = (monster.transform.position - transform.root.position).normalized; //Ÿ�� ���� ���� ����ȭ.
            //Vector3.Dot()�� ���� �÷��̾�� Ÿ���� ������ ����.           
            float targetAngle = Mathf.Acos(Vector3.Dot(transform.root.forward, targetDir)) * Mathf.Rad2Deg; //Acos�� ��ȯ���� ȣ��(radian)�̱� ������, attackAngle�� �񱳸� ����
                                                                                                            //������ �ٲ��ֱ� ���� ����� ������
            if (targetAngle <= attackAngle * 0.5f) //�翷���η� ������ ������ 0.5 ����. �ٷκ����ִ� ������ �������� �� ������ ���� �������� ������
            {
                monster.GetComponent<Monster>().Hit(currentDamage); //���� ���� �ִ� ��� Ÿ��
                InGameManager.Instance.Player.RecoverHp(hpRecovery, EApplicableType.Value);
            }
            yield return null;
        }
    }
}
