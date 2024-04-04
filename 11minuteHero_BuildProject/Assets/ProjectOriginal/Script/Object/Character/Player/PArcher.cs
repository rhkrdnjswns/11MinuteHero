using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PArcher : CPlayer
{
    protected override IEnumerator Co_Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(weapon.CoolTime);
            if (!isDodge && eCharacterActionable == ECharacterActionable.Actionable)
            {
                weapon.Attack();
            }
            yield return null;
        }
    }
    protected override IEnumerator Co_Dodge()
    {
        animator.SetTrigger(ConstDefine.TRIGGER_Dodge);
        Vector3 dir = transform.forward;
        dir.y = 0;
        while (isDodge)
        {
            yield return null;
            rigidbody.velocity = dir * currentSpeed * 2f;
            //transform.position += dir * currentSpeed * 2f * Time.deltaTime;
        }
        rigidbody.velocity = Vector3.zero; //이동 정지
    }
}
