using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillChoiceUI : ButtonComponent //��ų ������ UI
{
    [SerializeField] private GameObject deco; //�Ϲ� ��ų�� UI ������
    [SerializeField] private GameObject evolutionDeco; //��ȭ ��ų�� UI ������
    [SerializeField] private Text typeText; //��ų ���� �ؽ�Ʈ
    [SerializeField] private Image iconFrame; //��ų ������ ������
    [SerializeField] private Image icon; //��ų ������
    [SerializeField] private Text nameText; //��ų �̸�
    [SerializeField] private Text descriptionText; //��ų ����

    [SerializeField] private Sprite[] iconFrameArray; //��ų ������ ������ ������

    private int index;
    public void SetSkillChoiceUI(int index, ESkillType eSkillType, Sprite icon, string name, string description)
    {
        this.index = index;
        
        if(eSkillType == ESkillType.Evolution)
        {
            deco.SetActive(false);
            evolutionDeco.SetActive(true);
            typeText.text = "��ȭ ��ų";
        }
        else
        {
            if(eSkillType == ESkillType.None)
            {
                typeText.text = "������";
            }
            else if(eSkillType == ESkillType.Active)
            {
                typeText.text = "��Ƽ�� ��ų";
            }
            else
            {
                typeText.text = "�нú� ��ų";
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
