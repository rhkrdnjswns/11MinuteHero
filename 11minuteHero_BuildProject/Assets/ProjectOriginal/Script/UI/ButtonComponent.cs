using UnityEngine;
using UnityEngine.UI;

public class ButtonComponent : MonoBehaviour
{
    protected Button button;

    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(BtnEvt);
    }

    protected virtual void BtnEvt()
    {
        Debug.Log("버튼 눌림");
    }
    
}
