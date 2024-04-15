using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMeteor : AActiveSkill //메테오 스킬 클래스
{
    protected int meteorCount; //떨어질 메테오 개수
    [SerializeField] private GameObject meteorPrefab; //메테오 프리팹
    [SerializeField] private Queue<MeteorObject> meteorQueue = new Queue<MeteorObject>(); //메테오 풀
    [SerializeField] private float summonInteval = 0.2f; //소환 간격

    [SerializeField] private AttackRadiusUtility attackRadiusUtility; //적 감지 범위
    [SerializeField] private AttackRadiusUtility meteorAttackRadiusUtility; //메테오 피해 범위 
    [SerializeField] private int createCount;
    private bool bActionDone;
    
    private List<MeteorObject> allMeteorList = new List<MeteorObject>(); //모든 메테오 참조 (메테오가 디큐된 상태에서 패시브 스킬 효과 적용 시 참조를 위해)
    private float originRadius; //기본 메테오 피해 범위

    private WaitForSeconds summonDelay;
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < createCount; i++) //메테오 미리 생성 (패시브 적용 받기 위한 처리)
        {
            var obj = Instantiate(meteorPrefab);
            obj.transform.SetParent(transform);
            MeteorObject m = obj.GetComponent<MeteorObject>().SetAttackRadiusUtility(meteorAttackRadiusUtility);
            meteorQueue.Enqueue(m);
            allMeteorList.Add(m);
        }
        originRadius = meteorAttackRadiusUtility.Radius;
        summonDelay = new WaitForSeconds(summonInteval);
    }
    public override void InitSkill() //기믹 초기화 시 메테오 생성
    {
        base.InitSkill();

        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
        meteorCount = level;
    }
    protected override void SetCurrentDamage()
    {
        currentDamage = damage * level;
    }
    protected override void SetCurrentRange(float value)
    {
        foreach (var item in allMeteorList)
        {
            item.transform.localScale += Vector3.one * (value / 100f);
        }
        meteorAttackRadiusUtility.Radius += originRadius * value / 100;
    }
    private IEnumerator Co_SummonMeteor() //메테오 생성 코루틴
    {
        bActionDone = false;
        Collider[] inRadiusMonsterArray = attackRadiusUtility.GetLayerInRadius(transform.root); //반경 내 몬스터들을 얻어옴
        if (inRadiusMonsterArray.Length == 0)
        {
            bActionDone = true;
            yield break;
        }
#if UNITY_EDITOR
        AttackCount++;
#endif
        for (int i = 0; i < meteorCount; i++) //현재 메테오 개수만큼 리스트에서 활성화
        {
            var m = meteorQueue.Dequeue();
            m.transform.SetParent(null);
            m.transform.position = GetRandomPos(); //반경 내 랜덤한 몬스터 위치로 설정

            m.ActivateSkill(transform, meteorQueue, currentDamage); //메테오 소환
            if (i < meteorCount - 1) yield return summonDelay; //마지막 메테오 소환 시에는 텀 없게하기
        }
        bActionDone = true;
    }
    protected override IEnumerator Co_ActiveSkillAction() //액티브 스킬 기능 코루틴
    {
        WaitUntil actionDone = new WaitUntil(() => bActionDone);
        while (true)
        {
            yield return coolTimeDelay;
            StartCoroutine(Co_SummonMeteor());
            yield return actionDone; //모든 메테오가 소환될 때까지 대기
        }
    }
    private Vector3 GetRandomPos()
    {
        float xPos = Random.Range(-attackRadiusUtility.Radius, attackRadiusUtility.Radius + 1); //공격 반경만큼 랜덤한 x, z 위치 설정
        float yPos = Random.Range(-attackRadiusUtility.Radius, attackRadiusUtility.Radius + 1);
        Vector3 randomPos = new Vector3(transform.root.position.x + xPos, 0, transform.root.position.z + yPos);

        float distance = Vector3.Distance(randomPos, transform.root.position); //플레이어와 랜덤 설정된 위치 사이의 거리 체크
        Debug.Log(distance);
        if(distance > attackRadiusUtility.Radius) //공격 반경보다 거리가 더 먼 경우
        {
            Vector3 direction = (transform.root.position - randomPos).normalized; //플레이어 방향으로 반경에서 벗어난 만큼 옮겨줌
            randomPos += direction * (distance - attackRadiusUtility.Radius);
        }
        return randomPos;
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseDamage) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.MeteorShower);
            bCanEvolution = true;
        }
    }
}
