using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private RectTransform rect;

    private Vector3 offset = new Vector3(0, 1.5f, 0);
    public void SetDamageUI(string value, Vector3 pos, RectTransform rectCanvas, Camera cam, DamageUIContainer container)
    {
        text.fontSize = 14;
        text.color = Color.white;
        text.text = value;
        gameObject.SetActive(true);

        StartCoroutine(Co_Follow(pos, rectCanvas, cam, container));
        StartCoroutine(TweeningUtility.FadeOut(0.5f, text));
        StartCoroutine(TweeningUtility.SetFontSize(0.5f, text, text.fontSize, 10));
    }
    private IEnumerator Co_Follow(Vector3 pos, RectTransform rectCanvas, Camera cam, DamageUIContainer container)
    {
        float timer = 0;
        while(timer < 0.5f)
        {
            timer += Time.deltaTime;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(pos + offset); //���� ��ġ + offset�� ������ǥ�� ��ũ����ǥ�� ��ȯ�Ͽ� ������ 
            var localPos = Vector2.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectCanvas, screenPos, cam, out localPos); //��ũ�� ��ǥ�� ĵ������ ��ƮƮ������ ���������� ��ǥ�� ��ȯ�Ͽ� ��ȯ
            rect.localPosition = localPos; //ü�¹� ������������ ��ȯ���� ������ ����
            rect.localRotation = Quaternion.identity;

            yield return null;
        }
        container.ReturnDamageUI(this);
    }
}
