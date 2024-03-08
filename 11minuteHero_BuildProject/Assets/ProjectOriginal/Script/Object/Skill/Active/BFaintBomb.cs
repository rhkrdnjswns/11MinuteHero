using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFaintBomb : ABomb
{
    [SerializeField] private int shotCount;
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
}
