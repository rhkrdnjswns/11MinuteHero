using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEarthSpell : AActiveSkill //대지의 마법서 스킬 클래스
{
    //대지의 마법서는 스킬 발동 후 시전 시간이 존재하기 때문에 단일 오브젝트로 사용할 수 없다.
    //쿨타임 후 발동하면 1.5초 후에 데미지를 입힘. 쿨타임 갱신은 발동 즉시 갱신됨

    [SerializeField] private GameObject earthSpellPrefab; //대지 마법 오브젝트 프리팹
    private Queue<EarthSpellObject> earthSpellQueue = new Queue<EarthSpellObject>();
    [SerializeField] private AttackRadiusUtility attackRadiusUtility;

    private List<EarthSpellObject> allEarthSpellList = new List<EarthSpellObject>();
    private float originRadius;
    [SerializeField] protected float secondDamage;
    private float secondBaseDamage;
    [SerializeField] private float createCount;
    [SerializeField] protected float slowDuration;
    [SerializeField] protected float slowPercentage;


    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < createCount; i++)
        {
            var obj = Instantiate(earthSpellPrefab);
            obj.transform.SetParent(transform);
            var e = obj.GetComponent<EarthSpellObject>().SetAttackRadiusUtility(attackRadiusUtility);
            earthSpellQueue.Enqueue(e);
            allEarthSpellList.Add(e);
        }
        originRadius = attackRadiusUtility.Radius;
        secondBaseDamage = secondDamage;
    }
    public override void IncreaseDamage(float value)
    {
        increaseDamage += value; //현재 데미지 증가치 갱신
        damage += baseDamage * value / 100; //합연산 데미지 증가시킴
        secondDamage += secondBaseDamage * value / 100;
        SetCurrentDamage();
    }
    public override void InitSkill()//오브젝트 위치 갱신 및 컴포넌트 참조 인스턴스화
    {
        base.InitSkill();

        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
    }
    protected override void SetCurrentDamage()
    {
        currentDamage = damage + secondDamage * level;
    }
    protected override void SetCurrentRange(float value)
    {
        foreach (var item in allEarthSpellList)
        {
            item.transform.localScale += (Vector3.one - Vector3.up) * (value / 100f);
        }
        attackRadiusUtility.Radius += originRadius * value / 100f;
    }
    protected override IEnumerator Co_ActiveSkillAction() //스킬 기능 코루틴 
    {
        while(true)
        {
            yield return new WaitForSeconds(currentCoolTime);
            var e = earthSpellQueue.Dequeue();
            e.transform.SetParent(null);
            e.ActivateSkill(transform, earthSpellQueue, currentDamage, slowDuration, slowPercentage);
        }
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseMoveSpeed) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.EarthImplosion);
            bCanEvolution = true;
        }
    }
}
