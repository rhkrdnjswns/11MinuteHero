using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup bossApperation;
    [SerializeField] private RectTransform bossDirectionRect;
    [SerializeField] private RectTransform directionArrowRect;
    [SerializeField] private BarImageUtility bossHpBar;
    [SerializeField] private GameObject playerHpBar;
    public IEnumerator Co_AppearBoss()
    {
        bossApperation.gameObject.SetActive(true);
        yield return StartCoroutine(TweeningUtility.Blink(0.5f, 2, 0.5f, bossApperation));
        bossApperation.gameObject.SetActive(false);
        playerHpBar.SetActive(false);
    }
    public void ActiveBossHpBar()
    {
        bossHpBar.transform.parent.gameObject.SetActive(true);
    }
    public void UpdateBossHpBar(float value)
    {
        bossHpBar.SetFillAmount(value);
    }
    public void SetBossDirectionArrow()
    {
        StartCoroutine(Co_UpdateArrow());
    }
    private IEnumerator Co_UpdateArrow()
    {
        while(true)
        {
            Vector2 bossScreenPos = Camera.main.WorldToScreenPoint(InGameManager.Instance.CurrentBoss.transform.position);
            if (bossScreenPos.x < -20 || bossScreenPos.x > Screen.width + 10 || bossScreenPos.y < -90 || bossScreenPos.y > Screen.height + 50)
            {
                bossDirectionRect.gameObject.SetActive(true);

                float x = Mathf.Clamp(bossScreenPos.x, 70, 1210);
                float y = Mathf.Clamp(bossScreenPos.y, 70, 650);
                bossDirectionRect.anchoredPosition = new Vector2(x, y);

                Vector2 angleDir = (bossDirectionRect.anchoredPosition - bossScreenPos).normalized;
                float angle = Mathf.Atan2(angleDir.y, angleDir.x) * Mathf.Rad2Deg;
                directionArrowRect.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                bossDirectionRect.gameObject.SetActive(false);
            }
            yield return null;
        }
    }
}
