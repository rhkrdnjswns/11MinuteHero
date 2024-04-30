using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionMagicMissile : ActiveStaff
{
    public override void ActivateSkill()
    {
        base.ActivateSkill();

        InGameManager.Instance.Player.ChangeWeapon(this);
    }
}
