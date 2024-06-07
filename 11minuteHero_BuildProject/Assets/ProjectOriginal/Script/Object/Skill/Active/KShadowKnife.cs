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
        //ÄðÅ¸ÀÓ ¾øÀ½
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
        WaitUntil bReturn = new WaitUntil(() => isReturn);
        while (true)
        {
            yield return null;
            Collider[] inRadiusMonsterArray = attackRadiusUtility.GetLayerInRadius(transform.root);
            if (inRadiusMonsterArray.Length == 0)
            {
                continue;
            }
#if UNITY_EDITOR
            AttackCount++;
#endif
            int monsterIndex = Random.Range(0, inRadiusMonsterArray.Length);
            Projectile p = rangedAttackUtility.SummonProjectile();
            p.ShotProjectile(inRadiusMonsterArray[monsterIndex].transform);

            yield return bReturn;

            isReturn = false;
        }
    }
}
