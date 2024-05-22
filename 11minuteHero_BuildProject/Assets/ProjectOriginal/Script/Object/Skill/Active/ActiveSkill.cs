using System.Collections;
using UnityEngine;
public abstract class ActiveSkill : Skill //액티브 스킬 클래스. 기믹 클래스 상속. 모든 액티브 스킬은 기능이 작동하기 때문에 이를 강제하기 위해 추상 클래스로 작성
{
    [SerializeField] protected bool bCanEvolution;
    //액티브 스킬 공통사항
    [SerializeField] protected float coolTime;
    [SerializeField] private float currentCoolTime;

    [SerializeField] private float baseDamage; 
    [SerializeField] private float secondDamage;
    [SerializeField] protected float increaseDamagePercentage;

    [SerializeField] protected float currentDamage; //스킬 레벨에 대응하는 공식에 따른 실제 공격 데미지

    protected WaitForSeconds coolTimeDelay;

    protected float coolTimeReducePercentage; //현재 적용중인 쿨타임 감소치 (아래 세 수치는 전부 퍼센테이지)

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
    protected abstract IEnumerator Co_ActiveSkillAction(); //액티브 스킬 작동 코루틴
    
    public virtual void ReduceCoolTime(float value) //쿨타임 감소 함수 (패시브 스킬 쿨타임 감소에서 호출함)
    {
        coolTimeReducePercentage += value; //현재 쿨타임 감소치 갱신
        currentCoolTime -= coolTime * value / 100; //쿨타임 감소시킴
        if (currentCoolTime < 0.1f) currentCoolTime = 0.1f;

        coolTimeDelay = new WaitForSeconds(currentCoolTime);
    }
    public void IncreaseDamage(float value) //데미지 증가 함수 (패시브 스킬 데미지 증가에서 호출함)
    {
        increaseDamagePercentage += value; //현재 데미지 증가치 갱신
        SetCurrentDamage(); 
    }
    public void DecreaseDamage(float value)
    {
        increaseDamagePercentage -= value;
        if (increaseDamagePercentage < 0) increaseDamagePercentage = 0;
        SetCurrentDamage();
    }
    public void IncreaseRange(float value) //공격범위 증가 함수 (패시브 스킬 공격범위 증가에서 호출함)
    {
        //스킬마다 범위 증가가 적용되는 오브젝트가 많이 달라서 주의해야함
        SetCurrentRange(value);
    }
    protected virtual void SetCurrentDamage() //실제 공격 데미지 갱신 함수(내부적으로 데미지값이 변동될 때마다 호출)
    {
        float increaseValue = (baseDamage * level * increaseDamagePercentage / 100) + (secondDamage * increaseDamagePercentage / 100);
        currentDamage = baseDamage * level + secondDamage + increaseValue;
    }
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
    }
    protected abstract void SetCurrentRange(float value); //각 스킬별 공격 범위 갱신 함수 

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
