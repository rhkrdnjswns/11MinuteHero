using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FHolyWater : BFireBomb
{
    [SerializeField] private int shotCount;
    [SerializeField] private float distance;
    public override void InitSkill()
    {
        base.InitSkill();

        rangedAttackUtility.ShotCount = shotCount;
    }
    protected override void SetCurrentDamage()
    {
        currentDamage = damage;
        rangedAttackUtility.SetDamage(currentDamage);
    }
    protected override IEnumerator Co_AttackInRadius()
    {
        bShotDone = false;
        Projectile p;
        for (int i = 0; i < rangedAttackUtility.ShotCount; i++)
        {
            if (!rangedAttackUtility.IsValid())
            {
                rangedAttackUtility.CreateNewProjectile(bombAttackRadiusUtility);
                SetBombProjectile();
            }
            p = rangedAttackUtility.SummonProjectile();
            Vector3 angleDirection = transform.position + Quaternion.Euler(0, 72 * i, 0) * ((Vector3.forward + Vector3.right) * distance);
            p.ShotProjectile(angleDirection);

            if (i == rangedAttackUtility.ShotCount - 1) break;

            yield return new WaitForSeconds(shotInterval);
        }
        bShotDone = true;
    }
}
