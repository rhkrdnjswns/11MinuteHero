using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionHolyWater : ActiveFireBomb
{
    [SerializeField] private float distance;
    protected override IEnumerator Co_AttackInRadius()
    {
#if UNITY_EDITOR
        AttackCount++;
#endif
        for (int i = 0; i < currentShotCount; i++)
        {
            if (!projectileUtility.IsValid())
            {
                InitProjectile();
            }
            Projectile p = projectileUtility.GetProjectile();
            Vector3 angleDirection = transform.position + Quaternion.Euler(0, 360 / 5 * i, 0) * ((Vector3.forward + Vector3.right) * distance);
            p.SetMotion(angleDirection);
            p.ShotProjectile();

            if (i == currentShotCount - 1) break;

            yield return shotDelay;
        }
    }
    protected override void ReadEvolutionCSVData()
    {
        base.ReadEvolutionCSVData();

        sensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 4);
        dotInterval = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 7);

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 11);
        currentShotCount = shotCount;

        shotInterval = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 14);
        shotDelay = new WaitForSeconds(shotInterval);

        distance = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 20);

        dotAreaRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 22);
        originRadius = dotAreaRadius;

        dotDuration = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 23);
    }
}
