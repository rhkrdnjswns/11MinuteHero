using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMagicArrow : ACrystalStaff
{
    protected override void SetCurrentDamage()
    {
        CurrentDamage = damage;
        rangedAttackUtility.SetDamage(CurrentDamage);
    }
}
