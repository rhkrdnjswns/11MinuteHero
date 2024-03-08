using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Text touchText;
    public float blinkCycle;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Co_BlinkTouchText());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SceneUtility.TransitionScene(SceneInfo.Loading));
        }
        if (Input.touchCount > 0) //터치 시 호출
        {
            StartCoroutine(SceneUtility.TransitionScene(SceneInfo.Loading));
        }
    }
    private IEnumerator Co_BlinkTouchText()
    {
        WaitForSeconds w = new WaitForSeconds(blinkCycle);
        while(true)
        {
            StartCoroutine(TweeningUtility.FadeOut(blinkCycle, touchText));
            yield return w;
            StartCoroutine(TweeningUtility.FadeIn(blinkCycle, touchText));
            yield return w;
        }
    }
}
