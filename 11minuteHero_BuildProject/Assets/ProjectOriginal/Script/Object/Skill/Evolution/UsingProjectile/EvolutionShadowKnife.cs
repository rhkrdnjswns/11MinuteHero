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
            int monsterIndex = Random.Range(0, sensingCollisionArray.Length);

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
        projectileUtility.SetAction(() => isReturn = true);
    }
}
