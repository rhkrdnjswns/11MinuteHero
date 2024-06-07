using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMeteorShower : AMeteor
{
    [SerializeField] private int shotCount;
    public override void InitSkill()
    {
        base.InitSkill();

        meteorCount = shotCount;
    }
    protected override void SetCurrentDamage()
    {
        CurrentDamage = damage;
    }
}
