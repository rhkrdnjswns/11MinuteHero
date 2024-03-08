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
        if(!Physics.CheckSphere(transform.position + transform.forward * 3, 0.4f, LayerMask.GetMask("Obstacle"))) //�ڷ���Ʈ�� ��ġ�� ��ֹ��� �ִ��� �˻�
        {
            transform.position = transform.position + transform.forward * 3; //������ �ش� ��ġ�� �̵�
        }
        else
        {
            RaycastHit hit; //��ֹ��� �ִ� ��� ���̸� ���� ��ֹ��� ��� ��ġ ������ ������
            Physics.Raycast(transform.position, transform.forward, out hit, 3.4f, LayerMask.GetMask("Obstacle"));
            transform.position = hit.point + (transform.forward * -1 * 0.6f); //�÷��̾� ��ġ�� ��ֹ��� ��� ��ġ + ĳ������ �ݶ��̴��� ��������ŭ �޹������� �̵���Ŵ
        }
        AnimEvent_EndDodge();
        yield return null;
    }
    private void OnDrawGizmos() //������ �׽�Ʈ�� ���� ����� �׸���
    {       
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.forward * 3, 0.4f);
        Gizmos.DrawRay(transform.position, transform.forward * 3);
        Gizmos.DrawSphere(transform.position + Vector3.Cross(transform.forward, Vector3.up).normalized * -3, 1);
    }
}
