using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : NormalMonster
{
    protected override void ReadCSVData()
    {
        //base.ReadCSVData();
    }
    protected override IEnumerator Co_DieEvent()
    {
        overlappingAvoider.enabled = false;
        boxCollider.enabled = false;

        if (!animator.enabled) animator.enabled = true;
        animator.SetTrigger(ConstDefine.TRIGGER_DIE); //��� �ִϸ��̼�
        InGameManager.Instance.KillCount++; //ųī��Ʈ 1 ����

        yield return dieAnimDelay;
        while (transform.position.y > -3) //���Ͱ� �� ������ ������� ȿ��
        {
            transform.position += Vector3.down * 2 * Time.deltaTime;
            yield return null;
        }
        yield return dieAnimDelay;

        InGameManager.Instance.MonsterList.Remove(this);
        gameObject.SetActive(false);
    }
    protected override bool Move()
    {
        return base.Move();
    }
}
