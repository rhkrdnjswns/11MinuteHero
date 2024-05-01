using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionRenotoros : ActiveMagicShield
{
    private byte returnCount;
    public byte ReturnCount { get => returnCount; set => returnCount = value; }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        WaitUntil bReturn = new WaitUntil(() => returnCount == 2);
        while (true)
        {
            yield return coolTimeDelay;
            ShotProjectile();
            yield return bReturn;
        }
    }
    private void ShotProjectile()
    {
        returnCount = 0;
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
            p.SetShotDirection(GetDirection(i));

            p.ShotProjectile();
        }
    }
    protected override Vector3 GetDirection(int i)
    {
        if (i % 2 == 0)
        {
            return Vector3.right + Vector3.forward;
        }
        else
        {
            return Vector3.left + Vector3.back;
        }
    }
    protected override void InitProjectile()
    {
        base.InitProjectile();
        projectileUtility.SetAction(() => returnCount++);
    }

    protected override void ReadEvolutionCSVData()
    {
        base.ReadEvolutionCSVData();

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 11);
        currentShotCount = shotCount;

        activateTime = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 12);

        speed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 13);

        rotateSpeed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 16);

        knockBackSpeed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 34);
        knockBackDuration = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 35);
    }
}
