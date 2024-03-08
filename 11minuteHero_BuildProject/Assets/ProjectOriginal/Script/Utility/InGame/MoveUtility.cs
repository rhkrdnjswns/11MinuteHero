using UnityEngine;

public class MoveUtility //�̵� ��� Ŭ����. �̵��� �ϴ� ��ü�� �ش� Ŭ������ ��üȭ�Ͽ� ��� ���.
{
    private Vector3 direction; //�̵��� ����

    /// <summary>
    /// origin = �̵� �Լ��� ȣ���� ������Ʈ�� Transform,
    /// target = �̵��� ��ġ
    /// Ÿ�ٰ��� �Ÿ��� �������� �̳���(�������� �����ߴٸ�) false ��ȯ
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool Move(Transform origin, Vector3 target, float destinationRange, float moveSpeed) //���� �̵� �Լ�
    {
        if (Vector3.Distance(target, origin.position) > destinationRange) //Ÿ�ٰ��� �Ÿ��� �������� ���� ������ �̵�
        {
            direction = target - origin.position; //������ Ÿ�� �������� ����.
            origin.forward = direction;
            origin.position += direction.normalized * moveSpeed * Time.deltaTime; //�밢�� �̵��� ���� ���� ����ȭ �� �̵��ӵ���ŭ �̵�.
        }
        else
        {
            return false;
        }
        return true;
    }
    public bool Move(Vector3 direction, float moveSpeed)
    {
        if (direction == Vector3.zero) return false;

        Vector3 camForward = Camera.main.transform.forward; //ī�޶��� ��,��,��,�� ���⺤�͸� ������
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0; //y���� 0���� �������� (�÷��̾� ȸ���� ����)
        camRight.y = 0;

        Vector3 moveDirection = (direction.x * camRight.normalized) + (direction.z * camForward); //�Ű������� ���� ���⺤���� x,y = ���̽�ƽ�� �¿�, �յ� ����ȭ ����
        //ī�޶� �������� �������� �ϱ� ������ ����ī�޶��� ���⺤�͸� ������

        //�÷��̾ �Է¹��� ������ �ٶ󺸵��� ��
        InGameManager.Instance.Player.transform.forward = moveDirection;

        //���� ���͸� ����ȭ�Ͽ� �ӵ��� �����ϵ��� ����
        InGameManager.Instance.Player.transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
        return true;
    }
}
