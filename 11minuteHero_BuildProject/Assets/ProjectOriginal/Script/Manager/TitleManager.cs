using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Text touchText;
    
    public Image backgroundImage;
    public CanvasGroup logoGroup;

    public float fadeTime;

    public float blinkCycle;

    private bool canTouch;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Co_BackgroundAnim());
    }

    // Update is called once per frame
    void Update()
    {
        if (!canTouch) return;
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.instance.LoadScene((int)SceneInfo.Main);
        }
        if (Input.touchCount > 0) //터치 시 호출
        {
            GameManager.instance.LoadScene((int)SceneInfo.Main);
        }
    }
    private IEnumerator Co_BlinkTouchText()
    {
        WaitForSeconds w = new WaitForSeconds(blinkCycle);
        canTouch = true;
        while (true)
        {
            StartCoroutine(TweeningUtility.FadeOut(blinkCycle, touchText));
            yield return w;
            StartCoroutine(TweeningUtility.FadeIn(blinkCycle, touchText));
            yield return w;
        }
    }
    private IEnumerator Co_BackgroundAnim()
    {
        float timer = 0;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            backgroundImage.color = Color.Lerp(Color.black, Color.white, timer / fadeTime);

            yield return null;
        }
        StartCoroutine(Co_LogoAnim());
    }
    private IEnumerator Co_LogoAnim()
    {
        float timer = 0;
        while(timer < fadeTime)
        {
            timer += Time.deltaTime;
            logoGroup.alpha = Mathf.Lerp(0, 1, timer / fadeTime);
            yield return null;
        }
        StartCoroutine(Co_BlinkTouchText());
    }
}
