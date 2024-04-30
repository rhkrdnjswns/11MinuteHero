using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RollingButton : ButtonComponent
{
    [SerializeField] private Image coolTimeImage;
    private bool bCoolTime;
    protected override void BtnEvt()
    {
        if (bCoolTime) return;

        if(InGameManager.Instance.Player.Dodge())
        {
            bCoolTime = true;
            StartCoroutine(Co_UpdateCoolTime());
        }
    }
    private IEnumerator Co_UpdateCoolTime()
    {
        float timer = InGameManager.Instance.Player.DodgeCoolTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            coolTimeImage.fillAmount = timer / InGameManager.Instance.Player.DodgeCoolTime;
            yield return null;
        }
        bCoolTime = false;
    }
}
