using UnityEngine;

public class CameraFollow : MonoBehaviour //ī�޶� ��� Ŭ����
{
    private Vector3 offset = new Vector3(-7, 10, -7); //ī�޶� ���� ��ġ
    private void Update() //ī�޶� ��ġ �÷��̾� + ���������� ��� ����
    {
        transform.position = InGameManager.Instance.Player.transform.position + offset;
    }
}
