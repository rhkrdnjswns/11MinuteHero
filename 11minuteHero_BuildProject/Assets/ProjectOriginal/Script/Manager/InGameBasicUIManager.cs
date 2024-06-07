using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameBasicUIManager : MonoBehaviour
{
    [SerializeField] private Text timerSecondText;
    [SerializeField] private Text timerMinuteText;
    [SerializeField] private GameObject timerSecondHeader;
    [SerializeField] private GameObject timerMinuteHeader;

    [SerializeField] private Text killCountText;
    [SerializeField] private Text levelUpText;
    [SerializeField] private Image fadeImage;
    [SerializeField] private BarImageUtility playerHpBar;
    [SerializeField] private BarImageUtility playerExpBar;

    public Text KillCountText { get => killCountText; set => killCountText = value; }
    public BarImageUtility PlayerHpBar { get => playerHpBar; }
    public BarImageUtility PlayerExpBar { get => playerExpBar; }

    private IEnumerator Start()
    {
        InGameManager.Instance.DLevelUp += SetLevelUpTextAnim;
        yield return StartCoroutine(TweeningUtility.FadeOut(2f, fadeImage));
        fadeImage.gameObject.SetActive(false);
    }
    private void SetLevelUpTextAnim()
    {
        StartCoroutine(TweeningUtility.FadeOut(1f, levelUpText));
    }
    public void SetTimer(int minute, int second)
    {
        if (minute > 9)
        {
            timerMinuteHeader.SetActive(false);
        }
        if (second > 9 && timerSecondHeader.activeSelf)
        {
            timerSecondHeader.SetActive(false);
        }
        else if(second < 10 && !timerSecondHeader.activeSelf)
        {
            timerSecondHeader.SetActive(true);
        }
        timerSecondText.text = second.ToString();
        timerMinuteText.text = minute.ToString();
    }
    public void InActiveExpBar()
    {
        playerExpBar.gameObject.SetActive(false);
    }
}
