using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUI : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Camera camera;
    [SerializeField] private RectTransform rectCanvas;
    [SerializeField] private RectTransform rect;

    private void LateUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(InGameManager.Instance.Player.transform.position + offset); //몬스터 위치 + offset의 월드좌표를 스크린좌표로 변환하여 가져옴 
        var localPos = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectCanvas, screenPos, camera, out localPos); //스크린 좌표를 캔버스의 렉트트랜스폼 로컬포지션 좌표로 변환하여 반환
        rect.localPosition = localPos; //체력바 로컬포지션을 반환받은 값으로 지정
        rect.localRotation = Quaternion.identity;
    }
}
