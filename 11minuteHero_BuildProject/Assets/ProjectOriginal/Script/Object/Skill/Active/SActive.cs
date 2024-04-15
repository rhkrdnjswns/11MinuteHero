using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SActive : Skill //액티브 스킬 클래스. 기믹 클래스 상속. 모든 액티브 스킬은 기능이 작동하기 때문에 이를 강제하기 위해 추상 클래스로 작성
{
    [SerializeField] protected bool bCanEvolution;
    //액티브 스킬 공통사항
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
    [SerializeField] protected float damage; //데미지 증가 합연산을 받은 데미지
    [SerializeField] protected float currentDamage; //스킬 레벨에 대응하는 공식에 따른 실제 공격 데미지
    [SerializeField] protected float baseDamage; //데미지 증가 패시브에 따라 합연산할 기본 데미지

    protected WaitForSeconds coolTimeDelay;

    protected float coolTimeReduce; //현재 적용중인 쿨타임 감소치 (아래 세 수치는 전부 퍼센테이지)
    protected float increaseDamage; //현재 적용중인 데미지 증가치
    protected float increaseRange; //현재 적용중인 범위 증가치

#if UNITY_EDITOR
    public float Damage { get => currentDamage; }
    public string ActivateTime { get; private set; }
    public int AttackCount { get; set; }
    public float TotalDamage { get; set; }
    public override void InitSkill()
    {
        base.InitSkill();
        UnityEditor.EditorWindow.GetWindow<ForTest.DPSLogWindow>()?.AddSkill(this);
        ActivateTime = $"{(int)InGameManager.Instance.Timer / 60}분{(int)InGameManager.Instance.Timer % 60}초";
    }
#endif
    protected virtual void Awake()
    {
        baseDamage = damage; //기본 데미지를 스킬의 초기 데미지로 설정
        CurrentCoolTime = coolTime;
    }
    public virtual void ReduceCoolTime(float value) //쿨타임 감소 함수 (패시브 스킬 쿨타임 감소에서 호출함)
    {
        coolTimeReduce += value; //현재 쿨타임 감소치 갱신
        CurrentCoolTime -= coolTime * value / 100; //쿨타임 감소시킴
    }
    public virtual void IncreaseDamage(float value) //데미지 증가 함수 (패시브 스킬 데미지 증가에서 호출함)
    {
        increaseDamage += value; //현재 데미지 증가치 갱신
        damage += baseDamage * value / 100; //합연산 데미지 증가시킴
        SetCurrentDamage(); 
    }
    public void IncreaseRange(float value) //공격범위 증가 함수 (패시브 스킬 공격범위 증가에서 호출함)
    {
        //스킬마다 범위 증가가 적용되는 오브젝트가 많이 달라서 주의해야함
        increaseRange += value;
        SetCurrentRange(value);
    }
    protected abstract void SetCurrentDamage(); //실제 공격 데미지 갱신 함수(내부적으로 데미지값이 변동될 때마다 호출)
    protected abstract void SetCurrentRange(float value); //각 스킬별 공격 범위 갱신 함수 


}
