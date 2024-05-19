using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillLevelUI : ButtonComponent
{
    public Image icon;
    public Image[] levelArray;

    public int id;

    private bool isValid;

    public void ResetData()
    {
        icon.enabled = false;
        isValid = false;
        for (int i = 0; i < levelArray.Length; i++)
        {
            levelArray[i].color = Color.black;
        }
    }
    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
        icon.enabled = true;
        isValid = true;
    }

    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
        {
            levelArray[i].color = Color.white;
        }
    }
    public void SetEvolution()
    {
        for (int i = 0; i < levelArray.Length; i++)
        {
            levelArray[i].color = Color.red;
        }
    }
    protected override void BtnEvt()
    {
        if(isValid)
        {
            InGameManager.Instance.PossessedSkillUI.ChangeDescription(id);
        }
    }
}
