using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KShadowKnife : AKnife
{
    [SerializeField] private int shotCount;
    private bool isReturn;

    public bool IsReturn { set => isReturn = value; }
    public override void ReduceCoolTime(float value)
    {
        //쿨타임 없음
    }
    protected override void Awake()
    {
        base.Awake();

        foreach (var item in rangedAttackUtility.AllProjectileList)
        {
            item.GetComponent<PShadowKnife>().SetShadowKnifeReference(attackRadiusUtility, this);
        }
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        while (true)
        {
            yield return null;
            Collider[] inRadiusMonsterArray = attackRadiusUtility.GetLayerInRadius(transform.root);
            if (inRadiusMonsterArray.Length == 0)
            {
                continue;
            }
            int monsterIndex = Random.Range(0, inRadiusMonsterArray.Length);
            Projectile p = rangedAttackUtility.SummonProjectile();
            p.ShotProjectile(inRadiusMonsterArray[monsterIndex].transform);
            yield return new WaitUntil(() => isReturn);
            isReturn = false;
        }
    }
}
