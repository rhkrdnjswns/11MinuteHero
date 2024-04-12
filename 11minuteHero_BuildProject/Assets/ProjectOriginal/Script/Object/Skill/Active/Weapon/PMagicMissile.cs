using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMagicMissile : Projectile //마법사가 사용하는 투사체
{
    private int[] sideArray = {-5, -4, -3, 3, 4, 5 }; //곡선의 컨트롤 포인트 배열 (음수는 왼쪽, 양수는 오른쪽)
    private int side; // 현재 투사체가 그릴 곡선의 컨트롤 포인트

    private float timer; //투사체가 목표까지 도달하는 시간

    private Transform target; //투사체의 타겟(곡선 끝지점)
    
    private Vector3 summonPos; //투사체가 생성된 위치(곡선 시작지점)
    private Vector3 controllPoint; //곡선 컨트롤 포인트

    private Vector3 targetPosByOffset = Vector3.up * 0.5f; //투사체의 높이에 맞춘 타겟 위치
  
    public override void ShotProjectile(Transform target) //투사체 날아가는 함수 재정의. 몬스터 참조를 받음
    {
        this.target = target; //타겟 트랜스폼 얻어옴
        side = sideArray[Random.Range(0, sideArray.Length)];
        timer = 0; //시간 측정 초기화

        summonPos = InGameManager.Instance.Player.transform.position; //곡선 시작지점
        summonPos.y = 0.5f;
        //곡선 제어점. 타겟으로의 방향벡터의 내적에 side값을 곱해 좌 / 우 랜덤하게 포인트를 지정해 곡선을 휘게 함
        controllPoint = InGameManager.Instance.Player.transform.position + Vector3.Cross(target.position - InGameManager.Instance.Player.transform.position, Vector3.up).normalized * side;

        base.ShotProjectile();
    }
    public override void IncreaseSize(float value)
    {
        foreach (var item in GetComponentsInChildren<Transform>())
        {
            item.localScale += Vector3.one * (value / 100f);
        }
    }
    protected override IEnumerator Co_Shot() //투사체 날아가는 코루틴 재정의.
    {
        while (true)
        {
            timer += Time.deltaTime * 1.8f; //타이머 측정. timer = 베지어곡선 공식의 스칼라. 0 -> 1까지 커짐
            targetPosByOffset.x = target.position.x;
            targetPosByOffset.z = target.position.z;

            //베지어 곡선 공식으로 해당 포인트들 위에서 그려지는 곡선의 timer만큼의 지점을 반환함
            transform.position = CalculateBezierPoint(summonPos, controllPoint, targetPosByOffset, timer > 1 ? 1 : timer);

            //타겟 위치에 도착했지만 히트하지 않은 경우의 처리
            if (Vector3.Distance(transform.position, targetPosByOffset) <= 0.1) rangedAttackUtility.ReturnProjectile(this);

            //타겟이 사망한 경우의 처리
            if (!target.gameObject.activeSelf) rangedAttackUtility.ReturnProjectile(this);

            yield return null;
        }
    }
    private Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0 + 2 * u * t * p1 + tt * p2;
        //모든 투사체 생성 위치 y값이 0.5라서 해주는 처리
        p.y = 0.5f;

        return p;
    }
}
