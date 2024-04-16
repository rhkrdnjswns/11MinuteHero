using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : InteractiveItem
{
    [SerializeField] private float recoverValue; 
    protected override void Interaction()
    {
        InGameManager.Instance.Player.RecoverHp(recoverValue, EApplicableType.Value);
        ReturnItem();
    }
}
