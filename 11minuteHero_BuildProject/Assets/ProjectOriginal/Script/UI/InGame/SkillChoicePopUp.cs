using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillChoicePopUp : MonoBehaviour //강화 선택지 팝업
{
    [SerializeField] private List<SkillChoiceUI> skillChoiceUIList = new List<SkillChoiceUI>();
    [SerializeField] private float fadeDuration;
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void SetSkillChoicePopUp()
    {
        Time.timeScale = 0;
        Skill skill;
        for (int i = 0; i < InGameManager.Instance.SkillManager.SelectedChoiceList.Count; i++)
        {
            skill = InGameManager.Instance.SkillManager.SelectedChoiceList[i];
            skillChoiceUIList[i].SetSkillChoiceUI(i,skill.ESkillType, skill.Icon, skill.Name, skill.Description);
        }
        gameObject.SetActive(true);
        StartCoroutine(TweeningUtility.FadeIn(fadeDuration, canvasGroup));

        foreach (var item in skillChoiceUIList)
        {
            StartCoroutine(TweeningUtility.SetSize(0.5f, item.transform, Vector3.one * 0.25f, Vector3.one, Vector3.one * 1.1f));
        }
        canvasGroup.interactable = true;
    }
    public IEnumerator InActivePopUp()
    {
        canvasGroup.interactable = false;
        StartCoroutine(TweeningUtility.FadeOut(fadeDuration, canvasGroup));
        Time.timeScale = 1;
        yield return new WaitForSeconds(fadeDuration);
        gameObject.SetActive(false);
    }

}
