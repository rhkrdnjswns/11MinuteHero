using UnityEngine;

public class PopUpActiveButton : ButtonComponent
{
    [SerializeField] protected GameObject activateCanvas;
    protected override void BtnEvt()
    {
        activateCanvas.SetActive(!activateCanvas.activeSelf);
    }
}
