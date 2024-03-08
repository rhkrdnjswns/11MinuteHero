using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ax : MeleeWeapon
{
    private float recoveryPercentage = 1; //체력 회복 퍼센트
    private float recoveryDelay = 10; //체력 회복 딜레이

    //protected override void Start()
    //{
    //    base.Start();
    //    StartCoroutine(Co_Recovery());
    //}
    public override void Attack() //공격 함수 재정의
    {
       // if (isMaxLevel) InGameManager.Instance.Player.Recovery(damage * inRangeMonsterList.Count * recoveryPercentage / 100); //최대 레벨인 경우 입힌 피해량의 n%만큼 회복
    }
    private IEnumerator Co_Recovery() //체력 회복 코루틴
    {
        while (true)
        {
            yield return new WaitForSeconds(recoveryDelay);
            //InGameManager.Instance.Player.Recovery(recoveryPercentage, EApplicableType.Percentage);
        }
    }
}