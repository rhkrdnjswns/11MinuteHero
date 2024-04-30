using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : BarImageUtility
{
    private Canvas canvas; //체력바가 그려질 캔버스
    private Camera cam;  //캔버스의 카메라
    private RectTransform rectParent; //캔버스의 렉트트랜스폼
    private RectTransform rect; //체력바의 렉트트랜스폼
    [SerializeField] private Transform target; //해당 체력바의 타겟 몬스터의 트랜스폼
    
    public Transform Target { set => target = value; }
    private Vector3 offset = new Vector3(0, 1.5f, 0); //체력바 위치 조정값
    public void StartFollow() //체력바가 몬스터를 추적 시작
    {
        StartCoroutine(Co_RelocateHpBar());
    }
    private IEnumerator Co_RelocateHpBar() //체력바 몬스터 추적 코루틴
    {
        while (true)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset); //몬스터 위치 + offset의 월드좌표를 스크린좌표로 변환하여 가져옴 
            var localPos = Vector2.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, cam, out localPos); //스크린 좌표를 캔버스의 렉트트랜스폼 로컬포지션 좌표로 변환하여 반환
            rect.localPosition = localPos; //체력바 로컬포지션을 반환받은 값으로 지정

            yield return null;
        }
    }
}
