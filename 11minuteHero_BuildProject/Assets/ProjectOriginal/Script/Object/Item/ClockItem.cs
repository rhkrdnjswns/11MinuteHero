using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockItem : InteractiveItem
{
    [SerializeField] private float stiffnessTime;
    protected override void Interaction()
    {
        for (int i = 0; i < InGameManager.Instance.MonsterList.Count; i++)
        {
            InGameManager.Instance.MonsterList[i].SetStiffness(stiffnessTime);
        }
        for (int i = 0; i < InGameManager.Instance.ItemManager.ActivatedItemList.Count; i++)
        {
            InGameManager.Instance.ItemManager.ActivatedItemList[i].StopAnim(stiffnessTime);
        }
        InGameManager.Instance.GrayscaleUtility.SetGrayscale(stiffnessTime, 0.5f, 0.5f);
        ReturnItem();
    }
}
