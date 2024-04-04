using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RollingButton : ButtonComponent
{
    [SerializeField] private Image coolTimeImage;
    [SerializeField] private float coolTime;
    private bool bCoolTime;
    protected override void BtnEvt()
    {
        if (bCoolTime || InGameManager.Instance.Player.ECharacterActionable == ECharacterActionable.Unactionable) return;

        InGameManager.Instance.Player.Dodge();
        bCoolTime = true;
        StartCoroutine(Co_UpdateCoolTime());
    }
    private IEnumerator Co_UpdateCoolTime()
    {
        float timer = coolTime;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            coolTimeImage.fillAmount = timer / coolTime;
            yield return null;
        }
        bCoolTime = false;
    }
}
