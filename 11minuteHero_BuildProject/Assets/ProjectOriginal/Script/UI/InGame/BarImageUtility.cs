using UnityEngine.UI;
using UnityEngine;
public class BarImageUtility : MonoBehaviour //값에 따라 fill 값이 변경되는 Bar UI에 사용할 클래스
{
    [SerializeField] protected Image barImage;
    [SerializeField] protected Text fillText;
    private void Awake()
    {
        barImage = GetComponent<Image>();
        if (transform.childCount > 0) fillText = GetComponentInChildren<Text>();
    }
    public void SetFillAmount(float value)
    {
        barImage.fillAmount = value;
    }
    /// <summary>
    /// 바 이미지 하위에 텍스트가 존재하고 바 이미지 값에 따라 변하는 경우 사용
    /// </summary>
    /// <param name="value"></param>
    /// <param name="text"></param>
    public void SetFillAmount(float value, string text)
    {
        barImage.fillAmount = value;
        fillText.text = text;
    }
}
