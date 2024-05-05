using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : ButtonComponent
{
    private GameObject focus;
    private Text nameText;
    private Image icon;
    private int index;

    private System.Action<int> ShowCharacterData;
    public void SetFocusOff()
    {
        focus.SetActive(false);
    }
    public void SetFocusOn()
    {
        focus.SetActive(true);
    }
    public void InitDatas(int index, string name, Sprite sprite, System.Action<int> showData)
    {
        focus = transform.GetChild(2).gameObject;
        nameText = transform.GetChild(1).GetComponent<Text>();
        icon = transform.GetChild(0).GetComponent<Image>();

        this.index = index;
        nameText.text = name;
        icon.sprite = sprite;

        ShowCharacterData += showData;
    }
    protected override void BtnEvt()
    {
        ShowCharacterData(index);
    }
}
