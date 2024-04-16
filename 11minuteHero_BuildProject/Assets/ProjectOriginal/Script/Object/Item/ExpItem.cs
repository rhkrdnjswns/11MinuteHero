using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : Item
{
    [SerializeField] private int exp;
    protected override void Interaction()
    {
        InGameManager.Instance.Player.IncreaseExp(exp);
        ReturnItem();
    }
    public override void GetExpItem()
    {
        StartCoroutine(Co_ItemAnimation(-(InGameManager.Instance.Player.transform.position - transform.position).normalized));
        isGet = true;
    }
}
