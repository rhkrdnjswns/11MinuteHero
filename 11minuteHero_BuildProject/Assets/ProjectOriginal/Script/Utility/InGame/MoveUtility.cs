using UnityEngine;

public class MoveUtility //이동 기능 클래스. 이동을 하는 객체는 해당 클래스를 객체화하여 기능 사용.
{
    private Vector3 direction; //이동할 방향

    /// <summary>
    /// origin = 이동 함수를 호출한 오브젝트의 Transform,
    /// target = 이동할 위치
    /// 타겟과의 거리가 오차범위 이내면(목적지에 도착했다면) false 반환
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool Move(Transform origin, Vector3 target, float destinationRange, float moveSpeed) //몬스터 이동 함수
    {
        if (Vector3.Distance(target, origin.position) > destinationRange) //타겟과의 거리가 오차범위 내일 때까지 이동
        {
            direction = target - origin.position; //방향을 타겟 방향으로 설정.
            origin.forward = direction;
            origin.position += direction.normalized * moveSpeed * Time.deltaTime; //대각선 이동을 위한 벡터 정규화 및 이동속도만큼 이동.
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

        Vector3 camForward = Camera.main.transform.forward; //카메라의 앞,뒤,좌,우 방향벡터를 가져옴
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0; //y값을 0으로 고정해줌 (플레이어 회전축 고정)
        camRight.y = 0;

        Vector3 moveDirection = (direction.x * camRight.normalized) + (direction.z * camForward); //매개변수로 받은 방향벡터의 x,y = 조이스틱의 좌우, 앞뒤 정규화 벡터
        //카메라를 기준으로 움직여야 하기 때문에 메인카메라의 방향벡터를 곱해줌

        //플레이어가 입력받은 방향을 바라보도록 함
        InGameManager.Instance.Player.transform.forward = moveDirection;

        //방향 벡터를 정규화하여 속도가 일정하도록 해줌
        InGameManager.Instance.Player.transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
        return true;
    }
}
