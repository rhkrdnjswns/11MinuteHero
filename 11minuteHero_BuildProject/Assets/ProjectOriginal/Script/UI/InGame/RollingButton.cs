using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RollingButton : ButtonComponent
{
    [SerializeField] private Image coolTimeImage;
    [SerializeField] private float coolTime;
    private bool bCoolTime;
    private void Start()
    {
        coolTime = InGameManager.Instance.Player.DodgeCoolTime;
    }
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
