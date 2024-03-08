using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDevilSword : ASwordSkill
{
    protected override void SetCurrentDamage()
    {
        currentDamage = damage;
    }
}
