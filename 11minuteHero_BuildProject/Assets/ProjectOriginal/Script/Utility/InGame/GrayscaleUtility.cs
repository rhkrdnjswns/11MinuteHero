using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrayscaleUtility : MonoBehaviour
{
    private Material cameraMaterial;

    private float grayScale = 0f; // 0 = ���� ����, 1�� ����������� ���� ������� ����. 1�� ������ ���� ������ �Ͼ

    private Coroutine grayscaleCoroutine;
    private void Awake()
    {
        cameraMaterial = new Material(Shader.Find("Custom/Grayscale")); //Ŀ���� ���̴��� ������ ��Ƽ���� ����
    }

    //��ó�� ȿ��. src �̹���(���� ȭ��)�� dest �̹����� ��ü
    void OnRenderImage(RenderTexture src, RenderTexture dest) //�̹��� ������ �ÿ� ī�޶� ��Ƽ������ �����ؽ��ĸ� ����
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
