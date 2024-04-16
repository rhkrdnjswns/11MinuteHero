using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetItem : InteractiveItem
{
    protected override void Interaction()
    {
        foreach (var item in InGameManager.Instance.ItemManager.ActivatedExpItemList)
        {
            item.GetExpItem();
        }
        ReturnItem();
    }
}
