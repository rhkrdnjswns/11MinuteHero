using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoButton : ButtonComponent
{
    private GameObject lockIconObj;
    private Image iconImage;
    private Text stageNameText;
    private Text stageDescriptionText;
    private RectTransform rect;

    public Image IconImage { get => iconImage; set => iconImage = value; }
    public Text StageNameText { get => stageNameText; set => stageNameText = value; }
    public RectTransform Rect { get => rect; set => rect = value; }
    public bool IsFocus { get; set; }

    public void SetIsLock(bool isLock)
    {
        lockIconObj.SetActive(isLock);
    }
    public void Init(Sprite sprite, string name, string description)
    {
        iconImage = GetComponent<Image>();
        stageNameText = transform.GetChild(0).GetComponentInChildren<Text>();
        stageDescriptionText = stageNameText.transform.GetChild(0).GetComponent<Text>();
        rect = GetComponent<RectTransform>();
        lockIconObj = transform.GetChild(1).gameObject;

        iconImage.sprite = sprite;
        stageNameText.text = name;
        stageDescriptionText.text = description;
    }
    protected override void BtnEvt()
    {
        if (!IsFocus) return;

        Debug.Log(stageNameText.text + " º±≈√");
    }
}
