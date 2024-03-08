using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFireStorm : AWisp
{
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
        StartCoroutine(Co_SetWispByCount());
    }
    protected override void SetCurrentDamage()
    {
        currentDamage = damage;
    }
    protected override void SetCurrentRange(float value)
    {
        foreach (var item in wispList)
        {
            foreach (var child in item.GetComponentsInChildren<Transform>())
            {
                child.localScale += Vector3.one * (value / 100f);
                child.localPosition += Vector3.up * 0.05f;
            }
        }
    }
}
