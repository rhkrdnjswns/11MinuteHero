using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PWizard : CPlayer
{
    protected override IEnumerator Co_Attack()
    {
        float timer = 0;
        while (true)
        {
            timer = 0;
            while (timer < weapon.CoolTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            if (!isDodge && eCharacterActionable == ECharacterActionable.Actionable)
            {
                weapon.Attack();
                yield return new WaitUntil(() => weapon.GetBShotDone());
            }
            yield return null;
        }
    }

    protected override IEnumerator Co_Dodge()
    {
        if(!Physics.CheckSphere(transform.position + transform.forward * 3, 0.4f, LayerMask.GetMask("Obstacle"))) //텔레포트할 위치에 장애물이 있는지 검사
        {
            transform.position = transform.position + transform.forward * 3; //없으면 해당 위치로 이동
        }
        else
        {
            RaycastHit hit; //장애물이 있는 경우 레이를 쏴서 장애물과 닿는 위치 정보를 가져옴
            Physics.Raycast(transform.position, transform.forward, out hit, 3.4f, LayerMask.GetMask("Obstacle"));
            transform.position = hit.point + (transform.forward * -1 * 0.6f); //플레이어 위치를 장애물과 닿는 위치 + 캐릭릭터 콜라이더의 반지름만큼 뒷방향으로 이동시킴
        }
        AnimEvent_EndDodge();
        yield return null;
    }
    private void OnDrawGizmos() //에디터 테스트를 위한 기즈모 그리기
    {       
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.forward * 3, 0.4f);
        Gizmos.DrawRay(transform.position, transform.forward * 3);
        Gizmos.DrawSphere(transform.position + Vector3.Cross(transform.forward, Vector3.up).normalized * -3, 1);
    }
}
