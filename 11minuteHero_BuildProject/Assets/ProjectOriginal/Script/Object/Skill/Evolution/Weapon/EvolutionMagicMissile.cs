using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionMagicMissile : ActiveStaff
{
    public override void InitSkill()
    {
        base.InitSkill();

        InGameManager.Instance.Player.ChangeWeapon(this);
    }
}
