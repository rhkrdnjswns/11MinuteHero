using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 originPos;
    private Text text;
    public bool bAnimFinish;
    // Start is called before the first frame update
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponent<Text>();
        originPos = rectTransform.localPosition;
    }
    private void OnEnable()
    {
        rectTransform.localPosition = originPos;
        StartCoroutine(Co_TextAnim());
    }
    private IEnumerator Co_TextAnim()
    {
        float timer = 1f;

        while(timer > 0)
        {
            timer -= 0.01f;
            yield return null;
            text.color = new Color(1, 1, 0, timer);
            rectTransform.localPosition = new Vector3(0, rectTransform.localPosition.y + 1);
        }
        gameObject.SetActive(false);
    }
}
