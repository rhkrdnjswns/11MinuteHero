using UnityEngine;

public class CameraFollow : MonoBehaviour //카메라 기능 클래스
{
    private Vector3 offset = new Vector3(-7, 10, -7); //카메라 고정 위치
    private void Update() //카메라 위치 플레이어 + 오프셋으로 계속 갱신
    {
        transform.position = InGameManager.Instance.Player.transform.position + offset;
    }
}
