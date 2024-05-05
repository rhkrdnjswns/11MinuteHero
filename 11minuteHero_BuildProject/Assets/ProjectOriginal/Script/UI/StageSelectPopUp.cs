using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelectPopUp : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    public RectTransform contentRect;
    public GameObject staegInfoPrefab;

    private StageInfoButton[] stageInfoArray;

    public GameObject[] stageArray;
    public Sprite[] stageIconArray;
    public string[] stageNameArray;
    public string[] stageDescriptionArray;

    public float requiredDragValueMin;
    public float scrollDuration;

    private bool isScrolling;

    public int stageIndex;
    private float dragValue;
    public float scrollValue = 400;
    private void Awake()
    {
        stageInfoArray = new StageInfoButton[stageArray.Length];

        for (int i = 0; i < stageArray.Length; i++)
        {
            GameObject obj = Instantiate(staegInfoPrefab);
            obj.transform.SetParent(contentRect);
            obj.transform.localScale = Vector3.one;

            stageInfoArray[i] = obj.GetComponent<StageInfoButton>();
            stageInfoArray[i].Init(stageIconArray[i], stageNameArray[i], stageDescriptionArray[i]);

            if (i >= 1)
            {
                stageInfoArray[i].Rect.sizeDelta = new Vector2(300, 250);
                stageInfoArray[i].IconImage.color = new Color(0.1f, 0.1f, 0.1f);
                stageInfoArray[i].StageNameText.gameObject.SetActive(false);
            }
        }
    }
    public void BtnEvt_ChangeStage(bool isLeft)
    {
        if (isScrolling) return;

        if (isLeft)
        {
            if (stageIndex == stageArray.Length - 1) return;
            stageIndex++;

            StartCoroutine(Co_MoveContentRect(Vector3.left));
            StartCoroutine(Co_FocusStage(stageIndex - 1));
        }
        else
        {
            if (stageIndex == 0) return;

            stageIndex--;

            StartCoroutine(Co_MoveContentRect(Vector3.right));
            StartCoroutine(Co_FocusStage(stageIndex + 1));
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragValue = eventData.position.x;
    }
    public void OnDrag(PointerEventData eventData)
    {
        
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isScrolling) return;

        if (dragValue - eventData.position.x > requiredDragValueMin)
        {
            if (stageIndex == stageArray.Length - 1) return;

            stageIndex++;

            StartCoroutine(Co_MoveContentRect(Vector3.left));
            StartCoroutine(Co_FocusStage(stageIndex - 1));
        }
        else if (dragValue - eventData.position.x < -requiredDragValueMin)
        {
            if (stageIndex == 0) return;

            stageIndex--;

            StartCoroutine(Co_MoveContentRect(Vector3.right));
            StartCoroutine(Co_FocusStage(stageIndex + 1));
        }
    }
    public void BtnEvt_StartStage()
    {
        if (isScrolling) return;
        MainManager.instance.StartStage(stageIndex);
    }
    private IEnumerator Co_MoveContentRect(Vector3 direction)
    {
        isScrolling = true;

        Vector3 pos = contentRect.localPosition;
        float timer = 0;
        while(timer < scrollDuration)
        {
            timer += Time.deltaTime;
            contentRect.localPosition = Vector3.Lerp(pos, pos + direction * scrollValue, timer / scrollDuration);
            yield return null;
        }
        isScrolling = false;
    }
    private IEnumerator Co_FocusStage(int previousIndex)
    {
        float timer = 0;

        stageInfoArray[previousIndex].IsFocus = false;
        stageInfoArray[previousIndex].StageNameText.gameObject.SetActive(false);

        Vector2 size = stageInfoArray[stageIndex].Rect.sizeDelta;
        Color color = stageInfoArray[stageIndex].IconImage.color;

        Vector2 previousSize = stageInfoArray[previousIndex].Rect.sizeDelta;
        Color previousColor = stageInfoArray[previousIndex].IconImage.color;

        while (timer < scrollDuration)
        {
            timer += Time.deltaTime;
            stageInfoArray[stageIndex].Rect.sizeDelta = Vector2.Lerp(size, new Vector2(400, 350), timer / scrollDuration);
            stageInfoArray[stageIndex].IconImage.color = Color.Lerp(color, Color.white, timer / scrollDuration);

            stageInfoArray[previousIndex].Rect.sizeDelta = Vector2.Lerp(previousSize, new Vector2(300, 250), timer / scrollDuration);
            stageInfoArray[previousIndex].IconImage.color = Color.Lerp(previousColor, new Color(0.1f,0.1f,0.1f), timer / scrollDuration);
            yield return null;
        }

        stageInfoArray[stageIndex].IsFocus = true;
        stageInfoArray[stageIndex].StageNameText.gameObject.SetActive(true);
    }
}
