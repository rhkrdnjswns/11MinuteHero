using UnityEngine;

public class PauseButton : PopUpActiveButton
{
    protected override void BtnEvt()
    {
        base.BtnEvt();
        if (activateCanvas.activeSelf)
        {
            Time.timeScale = 0;
            InGameManager.Instance.bTimeStop = true;
        }
        else
        {
            Time.timeScale = 1;
            InGameManager.Instance.bTimeStop = false;
        }
    }
}
