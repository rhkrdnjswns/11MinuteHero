using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectBack;
    private RectTransform handle;
    private float handleRange;

    [SerializeField] private GameObject[] focusObjectArray = new GameObject[4];

    private Vector2 touchPos;
    private bool isTouch;

    void Start()
    {
        rectBack = GetComponent<RectTransform>();
        handle = transform.Find("Handle").GetComponent<RectTransform>();
        handleRange = rectBack.rect.width * 0.5f; //���̽�ƽ ũ�� ���簢���̶� width ������ �����ص� ��

        Transform focus = transform.Find("Focus");
        for (int i = 0; i < 4; i++)
        {
            focusObjectArray[i] = focus.GetChild(i).gameObject;
        }

        StartCoroutine(Co_MoveJoystickOnTouch());
    }
    private IEnumerator Co_MoveJoystickOnTouch()
    {
        while(InGameManager.Instance.GameState != EGameState.GameOver)
        {
            yield return null;
            if (!isTouch) continue;
            Vector2 vec = new Vector2(touchPos.x - rectBack.position.x, touchPos.y - handle.rect.height);

            //�ڵ��� ���̽�ƽ ��� ������ ������ �ʵ��� ��������
            Vector2 handlePos = Vector2.ClampMagnitude(vec, handleRange);

            //�ǹ��� �������� �������� �����Ϸ��� anchoredPosition�� �����ϸ� ��
            handle.anchoredPosition = handlePos;

            Vector2 vecNormal = vec.normalized; //���̽�ƽ�� ��ġ�� ����ȭ�ؼ� ���� ����

            InGameManager.Instance.Player.Direction = new Vector3(vecNormal.x, 0, vecNormal.y);
            InGameManager.Instance.Player.IsMove = true;

            //���� �˻� �� ��Ŀ�� �¿���
            if (vecNormal.x > 0)
            {
                if (vecNormal.y > 0)
                {
                    ActiveFocus(1);
                }
                else
                {
                    ActiveFocus(2);
                }
            }
            else if (vecNormal.x < 0)
            {
                if (vecNormal.y > 0)
                {
                    ActiveFocus(0);
                }
                else
                {
                    ActiveFocus(3);
                }
            }
        }
    }
    private void OnTouch(Vector2 vecTouch)
    {
        if (InGameManager.Instance.GameState == EGameState.GameOver) return;
        Vector2 vec = new Vector2(vecTouch.x - rectBack.position.x, vecTouch.y - handle.rect.height);

        //�ڵ��� ���̽�ƽ ��� ������ ������ �ʵ��� ��������
        Vector2 handlePos = Vector2.ClampMagnitude(vec, handleRange);

        //�ǹ��� �������� �������� �����Ϸ��� anchoredPosition�� �����ϸ� ��
        handle.anchoredPosition = handlePos;

        Vector2 vecNormal = vec.normalized; //���̽�ƽ�� ��ġ�� ����ȭ�ؼ� ���� ����

        InGameManager.Instance.Player.Direction = new Vector3(vecNormal.x, 0, vecNormal.y);
        InGameManager.Instance.Player.IsMove = true;

        //���� �˻� �� ��Ŀ�� �¿���
        if (vecNormal.x > 0)
        {
            if(vecNormal.y > 0)
            {
                ActiveFocus(1);
            }
            else
            {
                ActiveFocus(2);
            }
        }
        else if (vecNormal.x < 0)
        {
            if (vecNormal.y > 0)
            {
                ActiveFocus(0);
            }
            else
            {
                ActiveFocus(3);
            }
        }
    }
    private void ActiveFocus(int index) //���̽�ƽ ��Ŀ�� ����
    {
        foreach (var item in focusObjectArray)
        {
            item.SetActive(false);
        }
        if (index > 3 || focusObjectArray[index].activeSelf) return; // ���̽�ƽ �ʱ�ȭ
        focusObjectArray[index].SetActive(true);
    }
    public void OnDrag(PointerEventData eventData)
    {
        touchPos = eventData.position;
       // OnTouch(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
        touchPos = eventData.position;
        // OnTouch(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        handle.anchoredPosition = Vector3.zero;
        InGameManager.Instance.Player.Direction = Vector3.zero;
        ActiveFocus(4);
    }
}
