using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetItem : InteractiveItem
{
    protected override void Interaction()
    {
        for (int i = 0; i < InGameManager.Instance.ItemManager.ActivatedExpItemList.Count; i++)
        {
            InGameManager.Instance.ItemManager.ActivatedExpItemList[i].GetExpItem();
        }
        ReturnItem();
    }
}
