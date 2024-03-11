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
        handleRange = rectBack.rect.width * 0.5f; //조이스틱 크기 정사각형이라 width 값으로 설정해도 됨

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

            //핸들이 조이스틱 배경 밖으로 나가지 않도록 제한해줌
            Vector2 handlePos = Vector2.ClampMagnitude(vec, handleRange);

            //피벗을 기준으로 포지션을 설정하려면 anchoredPosition를 변경하면 됨
            handle.anchoredPosition = handlePos;

            Vector2 vecNormal = vec.normalized; //조이스틱의 위치를 정규화해서 방향 설정

            InGameManager.Instance.Player.Direction = new Vector3(vecNormal.x, 0, vecNormal.y);
            InGameManager.Instance.Player.IsMove = true;

            //방향 검사 후 포커스 온오프
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

        //핸들이 조이스틱 배경 밖으로 나가지 않도록 제한해줌
        Vector2 handlePos = Vector2.ClampMagnitude(vec, handleRange);

        //피벗을 기준으로 포지션을 설정하려면 anchoredPosition를 변경하면 됨
        handle.anchoredPosition = handlePos;

        Vector2 vecNormal = vec.normalized; //조이스틱의 위치를 정규화해서 방향 설정

        InGameManager.Instance.Player.Direction = new Vector3(vecNormal.x, 0, vecNormal.y);
        InGameManager.Instance.Player.IsMove = true;

        //방향 검사 후 포커스 온오프
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
    private void ActiveFocus(int index) //조이스틱 포커스 관리
    {
        foreach (var item in focusObjectArray)
        {
            item.SetActive(false);
        }
        if (index > 3 || focusObjectArray[index].activeSelf) return; // 조이스틱 초기화
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
