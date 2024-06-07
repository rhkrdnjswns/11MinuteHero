using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterProjectile : Projectile
{
    private float activateTime;
    protected override IEnumerator Co_Shot()
    {
        float timer = 0;
        while(timer < activateTime)
        {
            timer += Time.deltaTime;
            transform.position += shotDirection * rangedAttackUtility.ProjectileSpeed * Time.deltaTime;
            yield return null;
        }
        rangedAttackUtility.ReturnProjectile(this);
    }
    public override void SetActivateTime(float time)
    {
        activateTime = time;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(ConstDefine.TAG_PLAYER))
        {
            InGameManager.Instance.Player.Hit(rangedAttackUtility.ProjectileDamage);
            rangedAttackUtility.ReturnProjectile(this); //이후 풀로 되돌림
        }
    }
}
