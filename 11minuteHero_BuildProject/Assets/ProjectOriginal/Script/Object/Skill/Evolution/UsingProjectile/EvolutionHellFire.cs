using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionHellFire : ActiveFireBall
{
    [SerializeField] private float currentExplosionDamage;
    private float explosionBaseDamage;

    private float explosionRadius;
    private float originRadius;

    protected override void SetCurrentDamage()
    {
        base.SetCurrentDamage();
        currentExplosionDamage = explosionBaseDamage + (explosionBaseDamage * increaseDamagePercentage / 100);
        projectileUtility.SetExplosionDamage(currentExplosionDamage);
    }
    protected override void InitProjectile()
    {
        base.InitProjectile();
        originRadius = explosionRadius;
        projectileUtility.SetAttackRadius(explosionRadius);
    }
    protected override void SetCurrentRange(float value)
    {
        base.SetCurrentRange(value);
        explosionRadius += originRadius * value / 100f;
        projectileUtility.SetAttackRadius(explosionRadius);
    }

}
