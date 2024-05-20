using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillChoiceUI : ButtonComponent //스킬 선택지 UI
{
    [SerializeField] private GameObject deco; //일반 스킬용 UI 디자인
    [SerializeField] private GameObject evolutionDeco; //진화 스킬용 UI 디자인
    [SerializeField] private Text typeText; //스킬 종류 텍스트
    [SerializeField] private Image iconFrame; //스킬 아이콘 프레임
    [SerializeField] private Image icon; //스킬 아이콘
    [SerializeField] private Text nameText; //스킬 이름
    [SerializeField] private Text descriptionText; //스킬 설명

    [SerializeField] private Sprite[] iconFrameArray; //스킬 종류별 아이콘 프레임

    private int index;
    public void SetSkillChoiceUI(int index, ESkillType eSkillType, Sprite icon, string name, string description)
    {
        this.index = index;
        
        if(eSkillType == ESkillType.Evolution)
        {
            deco.SetActive(false);
            evolutionDeco.SetActive(true);
            typeText.text = "진화 스킬";
        }
        else
        {
            if(eSkillType == ESkillType.None)
            {
                typeText.text = "아이템";
            }
            else if(eSkillType == ESkillType.Active)
            {
                typeText.text = "액티브 스킬";
            }
            else
            {
                typeText.text = "패시브 스킬";
            }
            evolutionDeco.SetActive(false);
            deco.SetActive(true);
        }

        iconFrame.sprite = iconFrameArray[eSkillType == ESkillType.None? 1 : (int)eSkillType];
        this.icon.sprite = icon;
        nameText.text = name;
        descriptionText.text = description;
    }
    protected override void BtnEvt()
    {
        InGameManager.Instance.SkillManager.SelectSkill(index);
    }
}
