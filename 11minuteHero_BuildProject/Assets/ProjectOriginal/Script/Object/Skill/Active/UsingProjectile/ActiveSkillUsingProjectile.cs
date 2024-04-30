using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkillUsingProjectile : ActiveSkill
{
    protected ProjectileUtility projectileUtility;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int createCount;
    [SerializeField] protected int shotCount;
    [SerializeField] protected int currentShotCount;
    [SerializeField] protected float speed;

    private string targetTag = "Monster";
    private void Awake()
    {
        projectileUtility = new ProjectileUtility(projectilePrefab, createCount, transform);

        InitProjectile();
    }
    protected virtual void InitProjectile()
    {
        currentShotCount = shotCount;

        projectileUtility.SetOwner();
        projectileUtility.SetSpeed(speed);
        projectileUtility.SetTargetTag(targetTag);
    }
    protected override void SetCurrentDamage()
    {
        base.SetCurrentDamage();
        projectileUtility.SetDamage(currentDamage);
    }
    protected override void SetCurrentRange(float value)
    {
        float increaseValue = 1 * value / 100;
        projectileUtility.IncreaseSize(increaseValue);
    }
}
