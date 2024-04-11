using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private const float DISTANCE_OFFSET = 20f; //���Ϳ� �־����� �Ÿ�
    private const float FLOOR_SIZE = 40f; //floor �ϳ��� ������ (��������)

    private Transform[] floorArray; //floor �迭
    [SerializeField] private Transform center; //�÷��̾���� �Ÿ��� üũ�� floor
    private Vector3 distance; //center�� player�� �Ÿ�
    private float floorMoveDistance; //floor�� �̵��ؾ� �ϴ� �Ÿ�
    private float conditionDistanceToCenter; //center���� floor�� �̵���ų ���� �Ÿ� 
    private int sqrt; //�������� ũ���� ������

    private void Awake()
    {
        floorArray = new Transform[transform.childCount]; //�迭 �ʱ�ȭ
        Debug.Log(transform.childCount);
        for(int i = 0; i < floorArray.Length; i++)
        {
            floorArray[i] = transform.GetChild(i).transform;
        }
        center = floorArray[floorArray.Length / 2]; //���� ����

        sqrt = (int)Mathf.Sqrt(floorArray.Length); //�������� ũ��� Ȧ�� ���簢���̱� ������ (3x3, 5x5), �׻� ������ �� �¾� ������

        floorMoveDistance = FLOOR_SIZE * sqrt; //�������� ũ�⿡ ���� floor�� �̵� �Ÿ� �ʱ�ȭ

        conditionDistanceToCenter = DISTANCE_OFFSET * (sqrt / 2); //�������� ũ�⿡ ���� floor �̵��� ���� �Ÿ� �ʱ�ȭ
        /* floor�� �����ϴ� �迭�� ���� (3x3 ũ���� ��� ����)
         * [0][1][2]
         * [3][4][5]
         * [6][7][8]
         */
    }
    private void Update()
    {
        distance = center.position - InGameManager.Instance.Player.transform.position;
        MoveMapX();
        MoveMapZ();
    }
    private void MoveMapX() //���� �� �� �̵�
    {
        if (Mathf.Abs(distance.x) < conditionDistanceToCenter) return; //���� floor�� ���� �Ÿ� �̻� �������� ���� �۵�
        if (distance.x < -conditionDistanceToCenter) //������ �̵�
        {   
            for (int i = 0; i < floorArray.Length; i += sqrt) //���� �տ� floor���� ���� ���������� �Ű���
            {
                var temp = floorArray[i];
                for(int j = 0; j < sqrt - 1; j++) //�� ũ�⿡ �°� �迭 ����
                {
                    floorArray[i + j] = floorArray[i + j + 1];
                }
                floorArray[i + sqrt - 1] = temp;
                temp.Translate(Vector3.right * floorMoveDistance); //���� floor ������Ʈ ��ġ �̵�
            }
            center = floorArray[floorArray.Length / 2]; //���� floor ����
            /* (3x3 ũ���� ��� ����)
            * [1][2][0]
            * [4][5][3]
            * [7][8][6]
            * (���� �ε����� �״����. �迭 ��Ҹ� ���� ������ �ٲ�ٴ� ���� �����ֱ� ���� �����̴�)
            * ���� ���͸� 5�� �ٽ� ��������.
            * ���� �̵��� ��� �ݴ�� ����
            */
        }
        else if(distance.x > conditionDistanceToCenter) //���� �̵�
        {
            for (int i = sqrt - 1; i < floorArray.Length; i += sqrt) //���� �޿� floor���� ���� �տ��� �Ű���
            {
                var temp = floorArray[i];
                for (int j = 0; j < sqrt - 1; j++)
                {
                    floorArray[i - j] = floorArray[i - j - 1];
                }
                floorArray[i - (sqrt - 1)] = temp;
                temp.Translate(Vector3.left * floorMoveDistance);
            }
            center = floorArray[floorArray.Length / 2];
        }
    }
    private void MoveMapZ() //���� �� �� �̵�
    {
        if (Mathf.Abs(distance.z) < conditionDistanceToCenter) return; //���� floor�� ���� �Ÿ� �̻� �������� ���� �۵�
        if (distance.z < -conditionDistanceToCenter) //������ �̵�
        {
            for (int i = floorArray.Length - sqrt; i < floorArray.Length; i++)
            {
                var temp = floorArray[i];
                for (int j = 0; j < sqrt - 1; j++)
                {
                    floorArray[i - sqrt * j] = floorArray[i - sqrt * (j + 1)];
                }
                floorArray[i - sqrt * (sqrt - 1)] = temp;
                temp.Translate(Vector3.forward * floorMoveDistance);
            }
            center = floorArray[floorArray.Length / 2];
            /* (3x3 ũ���� ��� ����)
            * [6][7][8]
            * [0][1][2]
            * [3][4][5]
            * (���� �ε����� �״����. �迭 ��Ҹ� ���� ������ �ٲ�ٴ� ���� �����ֱ� ���� �����̴�)
            * ���� ���͸� 5�� �ٽ� ��������.
            * ���� �̵��� ��� �ݴ�� ����
            */
        }
        else if (distance.z > conditionDistanceToCenter) //�ڷ� �̵�
        {
            for (int i = 0; i < sqrt; i++)
            {
                var temp = floorArray[i];
                for (int j = 0; j < sqrt - 1; j++)
                {
                    floorArray[i + sqrt * j] = floorArray[i + sqrt * (j + 1)];
                }
                floorArray[i + sqrt * (sqrt - 1)] = temp;
                temp.Translate(Vector3.back * floorMoveDistance);
            }
            center = floorArray[floorArray.Length / 2];
        }
    }
}
