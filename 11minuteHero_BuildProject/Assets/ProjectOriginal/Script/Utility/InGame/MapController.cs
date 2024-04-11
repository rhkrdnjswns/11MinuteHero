using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private const float DISTANCE_OFFSET = 20f; //센터와 멀어지는 거리
    private const float FLOOR_SIZE = 40f; //floor 하나의 사이즈 (고정값임)

    private Transform[] floorArray; //floor 배열
    [SerializeField] private Transform center; //플레이어와의 거리를 체크할 floor
    private Vector3 distance; //center와 player의 거리
    private float floorMoveDistance; //floor가 이동해야 하는 거리
    private float conditionDistanceToCenter; //center기준 floor를 이동시킬 조건 거리 
    private int sqrt; //스테이지 크기의 제곱근

    private void Awake()
    {
        floorArray = new Transform[transform.childCount]; //배열 초기화
        Debug.Log(transform.childCount);
        for(int i = 0; i < floorArray.Length; i++)
        {
            floorArray[i] = transform.GetChild(i).transform;
        }
        center = floorArray[floorArray.Length / 2]; //센터 설정

        sqrt = (int)Mathf.Sqrt(floorArray.Length); //스테이지 크기는 홀수 정사각형이기 때문에 (3x3, 5x5), 항상 정수로 딱 맞아 떨어짐

        floorMoveDistance = FLOOR_SIZE * sqrt; //스테이지 크기에 따른 floor의 이동 거리 초기화

        conditionDistanceToCenter = DISTANCE_OFFSET * (sqrt / 2); //스테이지 크기에 따른 floor 이동할 조건 거리 초기화
        /* floor를 관리하는 배열의 구조 (3x3 크기의 경우 예시)
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
    private void MoveMapX() //맵의 좌 우 이동
    {
        if (Mathf.Abs(distance.x) < conditionDistanceToCenter) return; //센터 floor와 일정 거리 이상 벌어졌을 때만 작동
        if (distance.x < -conditionDistanceToCenter) //오른쪽 이동
        {   
            for (int i = 0; i < floorArray.Length; i += sqrt) //가장 앞열 floor들을 가장 마지막열로 옮겨줌
            {
                var temp = floorArray[i];
                for(int j = 0; j < sqrt - 1; j++) //맵 크기에 맞게 배열 갱신
                {
                    floorArray[i + j] = floorArray[i + j + 1];
                }
                floorArray[i + sqrt - 1] = temp;
                temp.Translate(Vector3.right * floorMoveDistance); //실제 floor 오브젝트 위치 이동
            }
            center = floorArray[floorArray.Length / 2]; //센터 floor 갱신
            /* (3x3 크기의 경우 예시)
            * [1][2][0]
            * [4][5][3]
            * [7][8][6]
            * (실제 인덱스는 그대로임. 배열 요소를 저런 식으로 바꿨다는 것을 보여주기 위한 예시이다)
            * 이후 센터를 5로 다시 설정해줌.
            * 왼쪽 이동의 경우 반대로 적용
            */
        }
        else if(distance.x > conditionDistanceToCenter) //왼쪽 이동
        {
            for (int i = sqrt - 1; i < floorArray.Length; i += sqrt) //가장 뒷열 floor들을 가장 앞열로 옮겨줌
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
    private void MoveMapZ() //맵의 앞 뒤 이동
    {
        if (Mathf.Abs(distance.z) < conditionDistanceToCenter) return; //센터 floor와 일정 거리 이상 벌어졌을 때만 작동
        if (distance.z < -conditionDistanceToCenter) //앞으로 이동
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
            /* (3x3 크기의 경우 예시)
            * [6][7][8]
            * [0][1][2]
            * [3][4][5]
            * (실제 인덱스는 그대로임. 배열 요소를 저런 식으로 바꿨다는 것을 보여주기 위한 예시이다)
            * 이후 센터를 5로 다시 설정해줌.
            * 뒤쪽 이동의 경우 반대로 적용
            */
        }
        else if (distance.z > conditionDistanceToCenter) //뒤로 이동
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
