using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameBasicUIManager : MonoBehaviour
{
    [SerializeField] private Text timerHourText;
    [SerializeField] private Text timerMinuteText;
    [SerializeField] private Text killCountText;
    [SerializeField] private Text levelUpText;
    [SerializeField] private Image fadeImage;
    [SerializeField] private BarImageUtility playerHpBar;
    [SerializeField] private BarImageUtility playerExpBar;

    public Text TimerHourText { get => timerHourText; set => timerHourText = value; }
    public Text TimerMinuteText { get => timerMinuteText; set => timerMinuteText = value; }
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
    public void InActiveExpBar()
    {
        playerExpBar.gameObject.SetActive(false);
    }
}
