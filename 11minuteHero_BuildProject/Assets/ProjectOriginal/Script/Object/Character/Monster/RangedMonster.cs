using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonster : NormalMonster
{
    [SerializeField] protected RangedAttackUtility rangedAttackUtility;
    [SerializeField] protected float projectileActivateTime;
    [SerializeField] private float behaviorDelay;
    protected override void Awake()
    {
        base.Awake();
        rangedAttackUtility.Parent = transform;
        rangedAttackUtility.CreateNewProjectile();
        rangedAttackUtility.SetDamage(damage);
        rangedAttackUtility.SetActivateTime(projectileActivateTime);
    }
    protected override IEnumerator Co_Attack()
    {
        transform.LookAt(InGameManager.Instance.Player.transform);
        if (!rangedAttackUtility.IsValid())
        {
            rangedAttackUtility.CreateNewProjectile();
            rangedAttackUtility.SetActivateTime(projectileActivateTime);
        }
        overlappingAvoider.enabled = false;
        Projectile p = rangedAttackUtility.SummonProjectile(0.5f);
        p.SetShotDirection((InGameManager.Instance.Player.transform.position - transform.position).normalized);
        animator.SetTrigger(ConstDefine.TRIGGER_ATTACK);
        p.ShotProjectile();
        yield return new WaitForSeconds(behaviorDelay);
        overlappingAvoider.enabled = true;
        monsterState = EMonsterState.Chase;
    }

}
