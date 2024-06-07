using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BThunderStrike : ABattleBluntWeapon
{
    [SerializeField] private int shotCount;
    public override void InitSkill() //초기화 함수 재정의
    {
        base.InitSkill();
        rangedAttackUtility.ShotCount = shotCount;

        SetCurrentDamage();
    }
    protected override void SetCurrentDamage()
    {
        CurrentDamage = damage;
        rangedAttackUtility.SetDamage(CurrentDamage);
    }
}
