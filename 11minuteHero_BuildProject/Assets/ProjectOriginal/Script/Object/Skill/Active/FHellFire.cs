using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FHellFire : AFireBall
{
    [SerializeField] private AttackRadiusUtility explosionAttackRadiusUtility;
    [SerializeField] private float explosionDamage;
    private float explosionBaseDamage;
    private float originRadius;
    protected override void Awake()
    {
        base.Awake();
        explosionBaseDamage = explosionDamage;
        originRadius = explosionAttackRadiusUtility.Radius;
        rangedAttackUtility.SetAttackRadiusUtility(explosionAttackRadiusUtility);
    }
    public override void InitSkill()
    {
        base.InitSkill();
        rangedAttackUtility.SetCount(penetrationCount);
        SetCurrentDamage();
    }
    protected override void SetCurrentDamage()
    {
        currentDamage = damage;
        rangedAttackUtility.SetDamage(currentDamage);
        foreach (var item in rangedAttackUtility.AllProjectileList)
        {
            item.GetComponent<PHellFire>().SetExplosionDamage(explosionDamage);
        }
    }
    public override void IncreaseDamage(float value)
    {
        increaseDamage += value; //현재 데미지 증가치 갱신
        damage += baseDamage * value / 100; //합연산 데미지 증가시킴
        explosionDamage += explosionBaseDamage * value / 100;
        SetCurrentDamage();
    }
    protected override void SetCurrentRange(float value)
    {
        explosionAttackRadiusUtility.Radius += originRadius * value / 100f;
        base.SetCurrentRange(value);
    }

}
