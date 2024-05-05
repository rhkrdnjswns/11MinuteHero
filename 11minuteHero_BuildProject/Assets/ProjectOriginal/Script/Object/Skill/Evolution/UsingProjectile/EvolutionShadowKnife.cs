using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionShadowKnife : ActiveKnife
{
    private bool isReturn;
    protected override IEnumerator Co_ActiveSkillAction()
    {
        WaitUntil bReturn = new WaitUntil(() => isReturn);
        while (true)
        {
            yield return null;
            int num = Physics.OverlapSphereNonAlloc(transform.root.position, sensingRadius, sensingCollisionArray, ConstDefine.LAYER_MONSTER);
            if (num == 0) continue;

#if UNITY_EDITOR
            AttackCount++;
#endif
            int monsterIndex = Random.Range(0, num);

            Projectile p = projectileUtility.GetProjectile();
            p.SetTargetTransform(sensingCollisionArray[monsterIndex].transform);

            p.ShotProjectile();

            yield return bReturn;

            isReturn = false;
        }
    }
    protected override void InitProjectile()
    {
        base.InitProjectile();
        projectileUtility.SetSensingRadius(sensingRadius);
        projectileUtility.SetAction(() => isReturn = true);
    }
    protected override void ReadEvolutionCSVData()
    {
        base.ReadEvolutionCSVData();

        sensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 4);

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 11);
        currentShotCount = shotCount;

        speed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 13);

        projectileSensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 31);
        originRadius = projectileSensingRadius;
    }
}
