using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SActive : Skill //��Ƽ�� ��ų Ŭ����. ��� Ŭ���� ���. ��� ��Ƽ�� ��ų�� ����� �۵��ϱ� ������ �̸� �����ϱ� ���� �߻� Ŭ������ �ۼ�
{
    [SerializeField] protected bool bCanEvolution;
    //��Ƽ�� ��ų �������
    [SerializeField] protected float coolTime;
    [SerializeField] protected float currentCoolTime;
    [SerializeField] protected float damage; //������ ���� �տ����� ���� ������
    [SerializeField] protected float currentDamage; //��ų ������ �����ϴ� ���Ŀ� ���� ���� ���� ������
    [SerializeField] protected float baseDamage; //������ ���� �нú꿡 ���� �տ����� �⺻ ������

    protected float coolTimeReduce; //���� �������� ��Ÿ�� ����ġ (�Ʒ� �� ��ġ�� ���� �ۼ�������)
    protected float increaseDamage; //���� �������� ������ ����ġ
    protected float increaseRange; //���� �������� ���� ����ġ

    protected virtual void Awake()
    {
        baseDamage = damage; //�⺻ �������� ��ų�� �ʱ� �������� ����
        currentCoolTime = coolTime;
    }
    public virtual void ReduceCoolTime(float value) //��Ÿ�� ���� �Լ� (�нú� ��ų ��Ÿ�� ���ҿ��� ȣ����)
    {
        coolTimeReduce += value; //���� ��Ÿ�� ����ġ ����
        currentCoolTime -= coolTime * value / 100; //��Ÿ�� ���ҽ�Ŵ
    }
    public virtual void IncreaseDamage(float value) //������ ���� �Լ� (�нú� ��ų ������ �������� ȣ����)
    {
        increaseDamage += value; //���� ������ ����ġ ����
        damage += baseDamage * value / 100; //�տ��� ������ ������Ŵ
        SetCurrentDamage(); 
    }
    public void IncreaseRange(float value) //���ݹ��� ���� �Լ� (�нú� ��ų ���ݹ��� �������� ȣ����)
    {
        //��ų���� ���� ������ ����Ǵ� ������Ʈ�� ���� �޶� �����ؾ���
        increaseRange += value;
        SetCurrentRange(value);
    }
    protected abstract void SetCurrentDamage(); //���� ���� ������ ���� �Լ�(���������� ���������� ������ ������ ȣ��)
    protected abstract void SetCurrentRange(float value); //�� ��ų�� ���� ���� ���� �Լ� 


}
