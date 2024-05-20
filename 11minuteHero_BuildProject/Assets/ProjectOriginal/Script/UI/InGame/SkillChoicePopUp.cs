using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillChoicePopUp : MonoBehaviour //��ȭ ������ �˾�
{
    [SerializeField] private List<SkillChoiceUI> skillChoiceUIList = new List<SkillChoiceUI>();
    [SerializeField] private float fadeDuration;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite potionIcon;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public IEnumerator Co_SetSkillChoicePopUp()
    {
        Time.timeScale = 0;

        Skill skill;
        for (int i = 0; i < InGameManager.Instance.SkillManager.SelectedChoiceList.Count; i++)
        {
            skill = InGameManager.Instance.SkillManager.SelectedChoiceList[i];
            if(skill == null)
            {
                skillChoiceUIList[i].SetSkillChoiceUI(-1, ESkillType.None, potionIcon, "ü�� ȸ��", "ü���� 10% ȸ���մϴ�.");
            }
            else
            {
                skillChoiceUIList[i].SetSkillChoiceUI(i, skill.ESkillType, skill.Icon, skill.Name, skill.Description);
            }
        }
        gameObject.SetActive(true);

        yield return StartCoroutine(TweeningUtility.FadeIn(fadeDuration, canvasGroup));

        foreach (var item in skillChoiceUIList)
        {
            item.gameObject.SetActive(true);
            StartCoroutine(TweeningUtility.SetSize(0.3f, item.transform, Vector3.one * 0.25f, Vector3.one, Vector3.one * 1.1f));
        }
        canvasGroup.interactable = true;
    }
    public IEnumerator InActivePopUp()
    {
        canvasGroup.interactable = false;

        yield return StartCoroutine(TweeningUtility.FadeOut(fadeDuration, canvasGroup));

        Time.timeScale = 1;
        gameObject.SetActive(false);
        foreach (var item in skillChoiceUIList)
        {
            item.gameObject.SetActive(false);
        }
        canvasGroup.alpha = 1;
    }

}
