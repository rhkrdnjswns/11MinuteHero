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
        animator.SetTrigger(ConstDefine.TRIGGER_DIE); //사망 애니메이션
        InGameManager.Instance.KillCount++; //킬카운트 1 증가

        yield return dieAnimDelay;
        while (transform.position.y > -3) //몬스터가 땅 밑으로 사라지는 효과
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
