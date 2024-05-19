using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ExitButton : ButtonComponent
{
    public Image fade;
    protected override void BtnEvt()
    {
        StartCoroutine(Co_Exit());
    }
    private IEnumerator Co_Exit()
    {
        fade.gameObject.SetActive(true);
        yield return StartCoroutine(TweeningUtility.FadeIn(1, fade));
        Time.timeScale = 1;
        StartCoroutine(SceneUtility.TransitionScene(SceneInfo.Main));
    }
}
