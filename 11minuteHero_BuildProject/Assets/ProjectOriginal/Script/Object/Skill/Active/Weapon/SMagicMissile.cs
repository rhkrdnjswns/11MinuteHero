using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMagicMissile : RStaff
{
    [SerializeField] private int shotCount;
    public override void InitSkill()
    {
        base.InitSkill();

        InGameManager.Instance.Player.ChangeWeapon(this);
        rangedAttackUtility.ShotCount = shotCount;
    }
    protected override void SetCurrentDamage()
    {
        CurrentDamage = damage;
        rangedAttackUtility.SetDamage(CurrentDamage);
    }
}
