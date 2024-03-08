using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweeningUtility //트위닝 코루틴의 시간 체크는 Time.unscaledDeltaTime을 사용해서 정지 시에도 작동하도록 적용
{
    //trasform의 사이즈를 start ~ end까지 time동안 조정
    public static IEnumerator SetSize(float time, Transform transform, Vector3 startSize, Vector3 endSize)
    {
        float timer = 0;

        while (timer < time)
        {
            transform.localScale = Vector3.Lerp(startSize, endSize, timer / time);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }
    //trasform의 사이즈를 start ~ bounceSize까지 time의 80%동안 조정 후 사이즈를 bounceSize ~ endSize까지 time의 20%동안 조정
    public static IEnumerator SetSize(float time, Transform transform, Vector3 startSize, Vector3 endSize, Vector3 bounceSize)
    {
        float timer = 0;

        float currentTime = time * 80f / 100f;

        while(timer < currentTime)
        {
            transform.localScale = Vector3.Lerp(startSize, bounceSize, timer / currentTime);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        while(timer < time)
        {
            transform.localScale = Vector3.Lerp(bounceSize, endSize, timer / time);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

    }
    //캔버스 그룹의 알파값 1~0으로 조정
    public static IEnumerator FadeOut(float time, CanvasGroup canvasGroup)
    {
        float timer = time;
        while (timer > 0)
        {
            timer -= ConstDefine.FRAME_DELAY;
            canvasGroup.alpha = timer / time;
            yield return null;
        }
    }
    public static IEnumerator FadeOut(float time, Image image)
    {
        float timer = time;
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, timer / time);
            yield return null;
        }
    }
    public static IEnumerator FadeOut(float time, Text text)
    {
        float timer = time;
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, timer / time);
            yield return null;
        }
    }
    public static IEnumerator FadeIn(float time, CanvasGroup canvasGroup)
    {
        float timer = 0;
        while (timer < time)
        {
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = timer / time;
            yield return null;
        }
    }
    public static IEnumerator FadeIn(float time, Image image)
    {
        float timer = 0;
        while (timer < time)
        {
            timer += Time.unscaledDeltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, timer / time);
            yield return null;
        }
    }
    public static IEnumerator FadeIn(float time, Text text)
    {
        float timer = 0;
        while (timer < time)
        {
            timer += Time.unscaledDeltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, timer / time);
            yield return null;
        }
    }
    
    public static IEnumerator UpAndDown(float time, float velocity, Transform transform)
    {
        float timer = 0;
        while (true)
        {
            while (timer < time)
            {
                timer += Time.unscaledDeltaTime;
                transform.position += transform.up * velocity * Time.deltaTime;
                yield return null;
            }
            while (timer > 0)
            {
                timer -= Time.unscaledDeltaTime;
                transform.position += transform.up * -velocity * Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
    }
    public static IEnumerator ReductionFontSize(float time, Text text, int originSize, int targetSize)
    {
        float timer = time;
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            text.fontSize = (int)Mathf.Lerp(targetSize, originSize, timer / time);
            yield return null;
        }
    }
}
