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
            Vector3 targetDir = (sensingCollisionArray[i].transform.position - transform.root.position).normalized; //Ÿ�� ���� ���� ����ȭ.
            //Vector3.Dot()�� ���� �÷��̾�� Ÿ���� ������ ����.           
            float targetAngle = Mathf.Acos(Vector3.Dot(transform.root.forward, targetDir)) * Mathf.Rad2Deg; //Acos�� ��ȯ���� ȣ��(radian)�̱� ������, attackAngle�� �񱳸� ����
                                                                                                            //������ �ٲ��ֱ� ���� ����� ������
            if (targetAngle <= attackAngle * 0.5f) //�翷���η� ������ ������ 0.5 ����. �ٷκ����ִ� ������ �������� �� ������ ���� �������� ������
            {
                sensingCollisionArray[i].GetComponent<Character>()?.Hit(currentDamage); //���� ���� �ִ� ��� Ÿ��
#if UNITY_EDITOR
                TotalDamage += currentDamage;
#endif
                InGameManager.Instance.Player.RecoverHp(hpRecovery, EApplicableType.Value);
            }
        }
    }
}
