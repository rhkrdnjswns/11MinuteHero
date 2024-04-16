using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockItem : InteractiveItem
{
    [SerializeField] private float stiffnessTime;
    protected override void Interaction()
    {
        foreach (var item in InGameManager.Instance.MonsterList)
        {
            item.SetStiffness(stiffnessTime);
        }
        foreach (var item in InGameManager.Instance.ItemManager.ActivatedItemList)
        {
            item.StopAnim(stiffnessTime);
        }
        InGameManager.Instance.GrayscaleUtility.SetGrayscale(stiffnessTime, 0.5f, 0.5f);
        ReturnItem();
    }
}
