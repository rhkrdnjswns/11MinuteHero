using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityItem : InteractiveItem
{
    [SerializeField] private float duration;
    [SerializeField] private float speedIncreaseValue;
    [SerializeField] private float damageIncreasePercentage;
    protected override void Interaction()
    {
        InGameManager.Instance.Player.SetInvincibility(speedIncreaseValue, damageIncreasePercentage, duration, EApplicableType.Value, EApplicableType.Percentage);
        ReturnItem();
    }
}
