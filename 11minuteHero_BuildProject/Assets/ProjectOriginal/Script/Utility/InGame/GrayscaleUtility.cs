using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrayscaleUtility : MonoBehaviour
{
    private Material cameraMaterial;

    private float grayScale = 0f; // 0 = 기존 색상, 1에 가까워질수록 점점 흑백으로 변함. 1을 넘으면 색상 반전이 일어남

    private Coroutine grayscaleCoroutine;
    private void Awake()
    {
        cameraMaterial = new Material(Shader.Find("Custom/Grayscale")); //커스텀 쉐이더를 적용한 머티리얼 생성
    }

    //후처리 효과. src 이미지(현재 화면)를 dest 이미지로 교체
    void OnRenderImage(RenderTexture src, RenderTexture dest) //이미지 렌더링 시에 카메라 머티리얼의 렌더텍스쳐를 적용
    {
        cameraMaterial.SetFloat("_Grayscale", grayScale);
        Graphics.Blit(src, dest, cameraMaterial);
    }
    public void SetGrayscale(float time, float value)
    {
        if (grayscaleCoroutine != null)
        {
            StopCoroutine(grayscaleCoroutine);
        }
        grayscaleCoroutine = StartCoroutine(Co_SetGrayscale(time, value));
    }
    public void SetGrayscale(float time, float fadeTime, float value)
    {
        if(grayscaleCoroutine != null)
        {
            StopCoroutine(grayscaleCoroutine);
        }
        grayscaleCoroutine = StartCoroutine(Co_SetGrayscale(time, fadeTime, value));
    }

    private IEnumerator Co_SetGrayscale(float time, float value)
    {
        float timer = 0;
        InGameManager.Instance.bTimeStop = true;
        while (timer < time)
        {
            timer += Time.deltaTime;
            grayScale = Mathf.Lerp(0, value, timer / time);
            yield return null;
        }
        InGameManager.Instance.bTimeStop = false;
    }
    private IEnumerator Co_SetGrayscale(float time, float fadeTime, float value)
    {
        float timer = 0;
        InGameManager.Instance.bTimeStop = true;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            grayScale = Mathf.Lerp(0, value, timer / fadeTime);
            yield return null;
        }
        while(timer < time - fadeTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0;
        while(timer < fadeTime)
        {
            timer += Time.deltaTime;
            grayScale = Mathf.Lerp(value, 0, timer / fadeTime);
            yield return null;
        }
        InGameManager.Instance.bTimeStop = false;
    }
}
