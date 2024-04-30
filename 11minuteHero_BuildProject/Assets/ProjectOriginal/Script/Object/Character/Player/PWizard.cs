using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PWizard : CPlayer
{
    private bool bCanDodge; //회피 가능한지 체크 (전방 장애물 여부)

    public override bool Dodge()
    {
        bCanDodge = Physics.CheckSphere(transform.position + transform.forward * 3, 0.5f, LayerMask.GetMask("Obstacle", "BossArea"));
        if (!bCanDodge) return base.Dodge();
        else return false;
    }
    protected override IEnumerator Co_Dodge()
    {
        transform.position = transform.position + transform.forward * 3; //없으면 해당 위치로 이동
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
