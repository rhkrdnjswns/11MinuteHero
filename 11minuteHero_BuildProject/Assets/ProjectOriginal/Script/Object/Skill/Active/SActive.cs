using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SActive : Skill //��Ƽ�� ��ų Ŭ����. ��� Ŭ���� ���. ��� ��Ƽ�� ��ų�� ����� �۵��ϱ� ������ �̸� �����ϱ� ���� �߻� Ŭ������ �ۼ�
{
    [SerializeField] protected bool bCanEvolution;
    //��Ƽ�� ��ų �������
    [SerializeField] protected float coolTime;
    private float currentCoolTime;
    protected float CurrentCoolTime
    {
        get
        {
            return currentCoolTime;
        }
        set
        {
            currentCoolTime = value;
            coolTimeDelay = new WaitForSeconds(currentCoolTime);
        }
    }
    [SerializeField] protected float damage; //������ ���� �տ����� ���� ������
    [SerializeField] protected float currentDamage; //��ų ������ �����ϴ� ���Ŀ� ���� ���� ���� ������
    [SerializeField] protected float baseDamage; //������ ���� �нú꿡 ���� �տ����� �⺻ ������

    protected WaitForSeconds coolTimeDelay;

    protected float coolTimeReduce; //���� �������� ��Ÿ�� ����ġ (�Ʒ� �� ��ġ�� ���� �ۼ�������)
    protected float increaseDamage; //���� �������� ������ ����ġ
    protected float increaseRange; //���� �������� ���� ����ġ

#if UNITY_EDITOR
    public float Damage { get => currentDamage; }
    public string ActivateTime { get; private set; }
    public int AttackCount { get; set; }
    public float TotalDamage { get; set; }
    public override void InitSkill()
    {
        base.InitSkill();
        UnityEditor.EditorWindow.GetWindow<ForTest.DPSLogWindow>()?.AddSkill(this);
        ActivateTime = $"{(int)InGameManager.Instance.Timer / 60}��{(int)InGameManager.Instance.Timer % 60}��";
    }
#endif
    protected virtual void Awake()
    {
        baseDamage = damage; //�⺻ �������� ��ų�� �ʱ� �������� ����
        CurrentCoolTime = coolTime;
    }
    public virtual void ReduceCoolTime(float value) //��Ÿ�� ���� �Լ� (�нú� ��ų ��Ÿ�� ���ҿ��� ȣ����)
    {
        coolTimeReduce += value; //���� ��Ÿ�� ����ġ ����
        CurrentCoolTime -= coolTime * value / 100; //��Ÿ�� ���ҽ�Ŵ
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
