using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkillUsingActiveObject : ActiveSkill
{
    protected ActiveObjectUtility activeObjectUtility;

    [SerializeField] private GameObject activeObjectPrefab;
    [SerializeField] private int createCount;

    private void Awake()
    {
        activeObjectUtility = new ActiveObjectUtility(activeObjectPrefab, createCount, transform);

        InitActiveObject();
    }
    protected virtual void InitActiveObject()
    {
        activeObjectUtility.SetOwner();
    }
    protected override void SetCurrentDamage()
    {
        base.SetCurrentDamage();

        activeObjectUtility.SetDamage(currentDamage);
    }
    protected override void SetCurrentRange(float value)
    {
        float increaeValue = 1 * value / 100;
        activeObjectUtility.IncreaseSize(increaeValue);
    }
}
