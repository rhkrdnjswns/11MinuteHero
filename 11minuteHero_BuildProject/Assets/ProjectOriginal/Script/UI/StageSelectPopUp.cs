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
    public int difficultyIndex;
    private float dragValue;
    public float scrollValue = 400;

    public Image difficultyButtonImage;
    public Text difficultyButtonText;

    public GameObject difficultyParent;
    private void Awake()
    {
        stageInfoArray = new StageInfoButton[stageArray.Length];
        //스테이지 클리어 데이터 불러오기

        for (int j = 0; j < stageArray.Length; j++)
        {
            GameObject obj = Instantiate(staegInfoPrefab);
            obj.transform.SetParent(contentRect);
            obj.transform.localScale = Vector3.one;

            stageInfoArray[j] = obj.GetComponent<StageInfoButton>();
            stageInfoArray[j].Init(stageIconArray[j], stageNameArray[j], stageDescriptionArray[j]);
            stageInfoArray[j].SetIsLock(false);
            if (j >= 1)
            {
                stageInfoArray[j].Rect.sizeDelta = new Vector2(300, 250);
                stageInfoArray[j].IconImage.color = new Color(0.1f, 0.1f, 0.1f);
                stageInfoArray[j].StageNameText.gameObject.SetActive(false);
                stageInfoArray[j].SetIsLock(true);
            }
        }
        BtnEvt_SelectDifficulty(0);
    }
    public void BtnEvt_ShowDifficulties()
    {
        difficultyParent.SetActive(!difficultyParent.activeSelf);
    }
    public void BtnEvt_SelectDifficulty(int index)
    {
        difficultyIndex = index;
        difficultyButtonImage.sprite = difficultyParent.transform.GetChild(index).GetComponent<Image>().sprite;
        difficultyButtonText.text = difficultyParent.transform.GetChild(index).GetComponentInChildren<Text>().text;
        difficultyParent.SetActive(false);
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
        if (stageIndex > 0) return;
        MainManager.instance.StartStage(stageIndex, difficultyIndex);
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
