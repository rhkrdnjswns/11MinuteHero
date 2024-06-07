using UnityEngine;

public class PauseButton : PopUpActiveButton
{
    protected override void BtnEvt()
    {
        base.BtnEvt();
        if (activateCanvas.activeSelf) Time.timeScale = 0;
        else Time.timeScale = 1;
    }
}
