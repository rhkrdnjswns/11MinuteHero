using System.Collections;
using UnityEngine;
public abstract class ActiveSkill : Skill //��Ƽ�� ��ų Ŭ����. ��� Ŭ���� ���. ��� ��Ƽ�� ��ų�� ����� �۵��ϱ� ������ �̸� �����ϱ� ���� �߻� Ŭ������ �ۼ�
{
    [SerializeField] protected bool bCanEvolution;
    //��Ƽ�� ��ų �������
    [SerializeField] protected float coolTime;
    [SerializeField] private float currentCoolTime;

    [SerializeField] private float baseDamage; 
    [SerializeField] private float secondDamage;
    [SerializeField] protected float increaseDamagePercentage;

    [SerializeField] protected float currentDamage; //��ų ������ �����ϴ� ���Ŀ� ���� ���� ���� ������

    protected WaitForSeconds coolTimeDelay;

    protected float coolTimeReducePercentage; //���� �������� ��Ÿ�� ����ġ (�Ʒ� �� ��ġ�� ���� �ۼ�������)

#if UNITY_EDITOR
    public float Damage { get => currentDamage; }
    public float ActivateTime { get; private set; }
    public int AttackCount { get; set; }
    public float TotalDamage { get; set; }
#endif

    public override void ActivateSkill()
    {
        base.ActivateSkill();
#if UNITY_EDITOR
        UnityEditor.EditorWindow.GetWindow<ForTest.DPSLogWindow>()?.AddSkill(this);
        ActivateTime = InGameManager.Instance.Timer;
#endif
        StartCoroutine(Co_ActiveSkillAction());
        //InGameManager.Instance.DGameOver += StopAllCoroutines;

        SetCurrentDamage();
    }
    protected abstract IEnumerator Co_ActiveSkillAction(); //��Ƽ�� ��ų �۵� �ڷ�ƾ
    
    public virtual void ReduceCoolTime(float value) //��Ÿ�� ���� �Լ� (�нú� ��ų ��Ÿ�� ���ҿ��� ȣ����)
    {
        coolTimeReducePercentage += value; //���� ��Ÿ�� ����ġ ����
        currentCoolTime -= coolTime * value / 100; //��Ÿ�� ���ҽ�Ŵ
        if (currentCoolTime < 0.1f) currentCoolTime = 0.1f;

        coolTimeDelay = new WaitForSeconds(currentCoolTime);
    }
    public void IncreaseDamage(float value) //������ ���� �Լ� (�нú� ��ų ������ �������� ȣ����)
    {
        increaseDamagePercentage += value; //���� ������ ����ġ ����
        SetCurrentDamage(); 
    }
    public void DecreaseDamage(float value)
    {
        increaseDamagePercentage -= value;
        if (increaseDamagePercentage < 0) increaseDamagePercentage = 0;
        SetCurrentDamage();
    }
    public void IncreaseRange(float value) //���ݹ��� ���� �Լ� (�нú� ��ų ���ݹ��� �������� ȣ����)
    {
        //��ų���� ���� ������ ����Ǵ� ������Ʈ�� ���� �޶� �����ؾ���
        SetCurrentRange(value);
    }
    protected virtual void SetCurrentDamage() //���� ���� ������ ���� �Լ�(���������� ���������� ������ ������ ȣ��)
    {
        float increaseValue = (baseDamage * level * increaseDamagePercentage / 100) + (secondDamage * increaseDamagePercentage / 100);
        currentDamage = baseDamage * level + secondDamage + increaseValue;
    }
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
    }
    protected abstract void SetCurrentRange(float value); //�� ��ų�� ���� ���� ���� �Լ� 

    protected override sealed void ReadCSVData()
    {
        base.ReadCSVData();

        if (eSkillType == ESkillType.Evolution)
        {
            ReadEvolutionCSVData();
        }
        else
        {
            ReadActiveCSVData();
        }
    }
    protected virtual void ReadActiveCSVData()
    {
        baseDamage = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 5);
        secondDamage = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 6);
        coolTime = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 7);
        currentCoolTime = coolTime;
        coolTimeDelay = new WaitForSeconds(currentCoolTime);
    }
    protected virtual void ReadEvolutionCSVData()
    {
        baseDamage = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 5);
        coolTime = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 6);
        currentCoolTime = coolTime;
        coolTimeDelay = new WaitForSeconds(currentCoolTime);
    }
}
