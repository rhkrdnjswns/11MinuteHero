using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ax : MeleeWeapon
{
    private float recoveryPercentage = 1; //ü�� ȸ�� �ۼ�Ʈ
    private float recoveryDelay = 10; //ü�� ȸ�� ������

    //protected override void Start()
    //{
    //    base.Start();
    //    StartCoroutine(Co_Recovery());
    //}
    public override void Attack() //���� �Լ� ������
    {
       // if (isMaxLevel) InGameManager.Instance.Player.Recovery(damage * inRangeMonsterList.Count * recoveryPercentage / 100); //�ִ� ������ ��� ���� ���ط��� n%��ŭ ȸ��
    }
    private IEnumerator Co_Recovery() //ü�� ȸ�� �ڷ�ƾ
    {
        while (true)
        {
            yield return new WaitForSeconds(recoveryDelay);
            //InGameManager.Instance.Player.Recovery(recoveryPercentage, EApplicableType.Percentage);
        }
    }
}