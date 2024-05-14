using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonster : NormalMonster
{
    private ProjectileUtility projectileUtility;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int projectileCreateCount;
    [SerializeField] protected float projectileActivateTime;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float behaviorDelay;

    private WaitForSeconds delay;
    public override void InitMonsterData()
    {
        base.InitMonsterData();

        projectileUtility = new ProjectileUtility(projectilePrefab, projectileCreateCount, transform);

        projectileUtility.SetOwner();
        projectileUtility.SetDamage(damage);
        projectileUtility.SetSpeed(projectileSpeed);
        projectileUtility.SetActivateTime(projectileActivateTime);

        delay = new WaitForSeconds(behaviorDelay);
    }
    protected override IEnumerator Co_Attack()
    {
        transform.LookAt(InGameManager.Instance.Player.transform);

        if(!projectileUtility.IsValid())
        {
            projectileUtility.SetOwner();
            projectileUtility.SetDamage(damage);
            projectileUtility.SetSpeed(projectileSpeed);
            projectileUtility.SetActivateTime(projectileActivateTime);
        }
        overlappingAvoider.enabled = false;

        Projectile p = projectileUtility.GetProjectile();
        p.transform.position += Vector3.up * 0.5f;
        p.SetShotDirection((InGameManager.Instance.Player.transform.position - transform.position).normalized);
        animator.SetTrigger(ConstDefine.TRIGGER_ATTACK);
        p.ShotProjectile();

        yield return delay;
        overlappingAvoider.enabled = true;
        monsterState = EMonsterState.Chase;
    }

}
