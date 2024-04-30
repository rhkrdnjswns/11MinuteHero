using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : BarImageUtility
{
    private Canvas canvas; //ü�¹ٰ� �׷��� ĵ����
    private Camera cam;  //ĵ������ ī�޶�
    private RectTransform rectParent; //ĵ������ ��ƮƮ������
    private RectTransform rect; //ü�¹��� ��ƮƮ������
    [SerializeField] private Transform target; //�ش� ü�¹��� Ÿ�� ������ Ʈ������
    
    public Transform Target { set => target = value; }
    private Vector3 offset = new Vector3(0, 1.5f, 0); //ü�¹� ��ġ ������
    public void StartFollow() //ü�¹ٰ� ���͸� ���� ����
    {
        StartCoroutine(Co_RelocateHpBar());
    }
    private IEnumerator Co_RelocateHpBar() //ü�¹� ���� ���� �ڷ�ƾ
    {
        while (true)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset); //���� ��ġ + offset�� ������ǥ�� ��ũ����ǥ�� ��ȯ�Ͽ� ������ 
            var localPos = Vector2.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, cam, out localPos); //��ũ�� ��ǥ�� ĵ������ ��ƮƮ������ ���������� ��ǥ�� ��ȯ�Ͽ� ��ȯ
            rect.localPosition = localPos; //ü�¹� ������������ ��ȯ���� ������ ����

            yield return null;
        }
    }
}
