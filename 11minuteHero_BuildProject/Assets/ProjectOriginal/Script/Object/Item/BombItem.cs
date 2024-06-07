using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombItem : InteractiveItem
{
    [SerializeField] private float damageForNormalMonster;
    [SerializeField] private float damageForBossPercentage;
    protected override void Interaction()
    {
        for (int i = 0; i < InGameManager.Instance.MonsterList.Count; i++)
        {
            InGameManager.Instance.MonsterList[i].Hit(damageForNormalMonster);
        }
        if(InGameManager.Instance.CurrentBoss.IsActivate)
        {
            InGameManager.Instance.CurrentBoss.Hit(InGameManager.Instance.CurrentBoss.MaxHp * damageForBossPercentage / 100);
        }

        ReturnItem();
    }
}
