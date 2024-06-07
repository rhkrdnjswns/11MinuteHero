using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGolemProjectile : Projectile
{
    protected float distance; //투사체가 날아가는 거리

    public override void SetDistance(float distance)
    {
        this.distance = distance;
    }
    protected override IEnumerator Co_Shot()
    {
        Vector3 summonPos = rangedAttackUtility.Parent.position;
        summonPos.y = 0.5f;
        while(Vector3.Distance(summonPos, transform.position) < distance)
        {
            transform.position += shotDirection.normalized * rangedAttackUtility.ProjectileSpeed * Time.deltaTime;
            yield return null;
        }
        rangedAttackUtility.Parent.GetComponent<BRedGolem>().SummonStone(transform.position);
        rangedAttackUtility.ReturnProjectile(this);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstDefine.TAG_PLAYER))
        {
            InGameManager.Instance.Player.Hit(rangedAttackUtility.ProjectileDamage);
        }
    }
}
