using UnityEngine;
using System.Collections;

public class CameraUtility : MonoBehaviour //카메라 기능 클래스
{
    private bool bFocus;
    private Vector3 offset = new Vector3(-7, 10, -7); //카메라 고정 위치
    private void Update() //카메라 위치 플레이어 + 오프셋으로 계속 갱신
    {
        if (bFocus) return;
        transform.position = InGameManager.Instance.Player.transform.position + offset;
    }
    public IEnumerator Co_FocusCam(float lerpTime, Vector3 start, Vector3 destination)
    {
        bFocus = true;
        InGameManager.Instance.Player.ECharacterActionable = ECharacterActionable.Unactionable;
        InGameManager.Instance.Player.Direction = Vector3.zero;

        float timer = 0;

        while (timer < lerpTime)
        {
            transform.position = Vector3.Lerp(start, destination, timer / lerpTime) + offset;
            timer += Time.deltaTime;
            yield return null;
        }
    }
    public IEnumerator Co_FocusCam(float lerpTime, float focusDelay, Vector3 destination)
    {
        bFocus = true;
        InGameManager.Instance.Player.ECharacterActionable = ECharacterActionable.Unactionable;
        InGameManager.Instance.Player.Direction = Vector3.zero;

        float timer = 0;

        while(timer < lerpTime)
        {
            transform.position = Vector3.Lerp(InGameManager.Instance.Player.transform.position, destination, timer / lerpTime) + offset;
            timer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(focusDelay);

        bFocus = false;
        InGameManager.Instance.Player.ECharacterActionable = ECharacterActionable.Actionable;

        transform.position = InGameManager.Instance.Player.transform.position + offset;
    }
    public void UnFocus()
    {
        bFocus = false;
        InGameManager.Instance.Player.ECharacterActionable = ECharacterActionable.Actionable;

        transform.position = InGameManager.Instance.Player.transform.position + offset;
    }
}
