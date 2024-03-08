using UnityEngine.UI;
using UnityEngine;
public class BarImageUtility : MonoBehaviour //���� ���� fill ���� ����Ǵ� Bar UI�� ����� Ŭ����
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
    /// �� �̹��� ������ �ؽ�Ʈ�� �����ϰ� �� �̹��� ���� ���� ���ϴ� ��� ���
    /// </summary>
    /// <param name="value"></param>
    /// <param name="text"></param>
    public void SetFillAmount(float value, string text)
    {
        barImage.fillAmount = value;
        fillText.text = text;
    }
}
