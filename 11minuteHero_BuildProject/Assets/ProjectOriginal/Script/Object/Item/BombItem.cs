using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombItem : InteractiveItem
{
    [SerializeField] private float damageForNormalMonster;
    [SerializeField] private float damageForBossPercentage;
    protected override void Interaction()
    {
        foreach (var item in InGameManager.Instance.MonsterList)
        {
            item.Hit(damageForNormalMonster);
        }
        if(InGameManager.Instance.CurrentBoss.IsActivate)
        {
            InGameManager.Instance.CurrentBoss.Hit(InGameManager.Instance.CurrentBoss.MaxHp * damageForBossPercentage / 100);
        }

        ReturnItem();
    }
}
