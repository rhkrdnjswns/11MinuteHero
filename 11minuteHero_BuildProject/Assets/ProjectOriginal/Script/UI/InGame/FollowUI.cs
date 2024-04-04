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
        Vector3 screenPos = Camera.main.WorldToScreenPoint(InGameManager.Instance.Player.transform.position + offset); //���� ��ġ + offset�� ������ǥ�� ��ũ����ǥ�� ��ȯ�Ͽ� ������ 
        var localPos = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectCanvas, screenPos, camera, out localPos); //��ũ�� ��ǥ�� ĵ������ ��ƮƮ������ ���������� ��ǥ�� ��ȯ�Ͽ� ��ȯ
        rect.localPosition = localPos; //ü�¹� ������������ ��ȯ���� ������ ����
        rect.localRotation = Quaternion.identity;
    }
}
