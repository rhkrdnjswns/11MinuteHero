using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WRangedWeapon : AWeapon //���Ÿ� ����. (09/25)��Ƽ�� ��ų ���� ���� �� �����丵 �ÿ� RangedWeapon���� Ŭ���� �̸� ����
{
    [SerializeField] protected RangedAttackUtility rangedAttackUtility;
    [SerializeField] protected float secondDamage;
    private float secondBaseDamage;
    private Coroutine attackCoroutine;

    protected override void Awake()
    {
        base.Awake();
        rangedAttackUtility.Parent = transform;
        rangedAttackUtility.CreateNewProjectile();
        secondBaseDamage = secondDamage;
    }
    public override void IncreaseDamage(float value)
    {
        increaseDamage += value; //���� ������ ����ġ ����
        damage += baseDamage * value / 100; //�տ��� ������ ������Ŵ
        secondDamage += secondBaseDamage * value / 100;
        SetCurrentDamage();
    }
    protected override void SetCurrentDamage()
    {
        currentDamage = damage + secondDamage * level;
        rangedAttackUtility.SetDamage(currentDamage);
    }
    public override void Attack() //���� �Լ� �������̵�
    {
        attackCoroutine = StartCoroutine(Co_Shot());
    }
    protected abstract IEnumerator Co_Shot(); //����ü �߻� �ڷ�ƾ
    protected override void SetCurrentRange(float value)
    {
        rangedAttackUtility.IncreaseSize(value);
    }
    public virtual void DoMaxLevelEffect() //���� �ִ� ���� �޼� �� ������ Ư��ȿ�� �����Ǹ� ���� �Լ�
    {
        //Do Something in child
    }
}
