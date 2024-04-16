using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveItem : Item
{
    protected override IEnumerator Co_ItemAnimation(Vector3 direction)
    {
        if (upAndDownAnim != null)
        {
            StopCoroutine(upAndDownAnim);
        }
        transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
        direction.y = 0; //���� ������ y���� 0���� �ؼ� y�� �̵� ����
        float timer = 0;

        while (timer < 0.5f)//�÷��̾� ���� �������� ��ġ�� �������� ƨ��� ���� �ִϸ��̼� ����
        {
            timer += Time.deltaTime;
            transform.position += direction * 4 * Time.deltaTime;
            yield return null;
        }
        while (Vector3.Distance(transform.position, InGameManager.Instance.Player.transform.position + Vector3.up * 0.3f) > 0.2f) //�÷��̾� ��ġ�� �������� ���� �ִϸ��̼� ����
        {
            timer += Time.deltaTime;
            transform.position += (InGameManager.Instance.Player.transform.position + Vector3.up * 0.3f - transform.position).normalized * (8 + timer) * Time.deltaTime;
            yield return null;
        }
        Interaction();
    }
}
