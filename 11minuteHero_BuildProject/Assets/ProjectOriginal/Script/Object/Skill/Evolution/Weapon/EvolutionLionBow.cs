using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionLionBow : ActiveBow
{
    private Coroutine speedUpCoroutine;
    private int speedUpCount;
    private bool isSpeedUp;
    [SerializeField] private float speedUpValue;
    [SerializeField] private float speedUpDuration;
    public override void InitSkill()
    {
        base.InitSkill();

        InGameManager.Instance.Player.ChangeWeapon(this);
        arrowAngle = GetArrowAngle();
        projectileUtility.SetAction(SetSpeedUp);
    }
    private void SetSpeedUp() //플레이어 스피드 증가 함수
    {
        if(!isSpeedUp)
        {
            isSpeedUp = true;
        }

        if(speedUpCoroutine != null) //스피드 증가가 끝나기 전에 다시 호출된 경우
        {
            StopCoroutine(speedUpCoroutine);
        }
        speedUpCoroutine = StartCoroutine(Co_SetSpeed()); //코루틴 실행

        if (speedUpCount < 5) speedUpCount++; //현재 스피드 증가를 몇 번 받았는지 체크
    }
    private IEnumerator Co_SetSpeed()
    {
        if (speedUpCount < 5) //스피드 증가 카운트가 5회 이상이 되면 스피드는 더 이상 증가하지 않고 지속 시간만 갱신됨.
        {
            InGameManager.Instance.Player.IncreaseSpeed(speedUpValue, EApplicableType.Value);
        }

        float timer = speedUpDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        //타이머가 0초가 되기 전에 코루틴이 정지하면 아래 로직은 실행되지 않는다.

        InGameManager.Instance.Player.DecreaseSpeed(speedUpValue * speedUpCount, EApplicableType.Value);//이동속도 원래대로 초기화
        speedUpCount = 0;
        isSpeedUp = false;
    }
}
