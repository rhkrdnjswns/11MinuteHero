using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionHellFire : ActiveFireBall
{
    private float explosionRadius;
    private float originRadius;

    protected override void SetCurrentDamage()
    {
        base.SetCurrentDamage();
        projectileUtility.SetExplosionDamage(currentDamage);
    }
    protected override void InitProjectile()
    {
        base.InitProjectile();

        projectileUtility.SetAttackRadius(explosionRadius);
    }
    protected override void SetCurrentRange(float value)
    {
        base.SetCurrentRange(value);
        explosionRadius += originRadius * value / 100f;
        projectileUtility.SetAttackRadius(explosionRadius);
    }
    protected override void ReadEvolutionCSVData()
    {
        base.ReadEvolutionCSVData();

        sensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 4);

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 11);
        currentShotCount = shotCount;

        activateTime = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 12);
        speed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 13);

        penetrationCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 15);
        currentPenetrationCount = penetrationCount;

        explosionRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 21);
        originRadius = explosionRadius;

    }

}
