using System.Collections;
using UnityEngine;

public class PWarrior : CPlayer
{
    private bool isRush;
    private WarriorRush warriorRush;
    protected override void Awake()
    {
        base.Awake();

        warriorRush = GetComponentInChildren<WarriorRush>();
    }

    protected override IEnumerator Co_Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(weapon.CoolTime);
            if (!isDodge && eCharacterActionable == ECharacterActionable.Actionable)
            {
                animator.SetTrigger(ConstDefine.TRIGGER_MELEE_ATTACK);
                weapon.Attack();
            }
        }
    }

    protected override IEnumerator Co_Dodge()
    {
        animator.SetTrigger(ConstDefine.TRIGGER_Dodge);
        Vector3 dir = transform.forward;
        dir.y = 0;
        yield return new WaitUntil(() => isRush);
        while (isRush)
        {
            yield return InGameManager.Instance.waitForFixedUpdate;
            rigidbody.velocity = dir * currentSpeed * 4;
        }
        rigidbody.velocity = Vector3.zero; //�̵� ����
    }
    public void AnimEvent_Rush()
    {
        isRush = true;
        warriorRush.Activate(isRush);
    }
    public void AnimEvent_EndRush()
    {
        isRush = false;
        warriorRush.Activate(isRush);
    }

}