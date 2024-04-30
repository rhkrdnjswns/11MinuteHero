using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMeteor : ActiveSkillUsingActiveObject //메테오 스킬 클래스
{
    [SerializeField] private int meteorCount; //떨어질 메테오 개수
    private int currentMeteorCount;

    [SerializeField] private float summonInteval = 0.2f; //소환 간격
    [SerializeField] private float sensingRadius;
    [SerializeField] private float explosionRadius;
    private float originExplosionRadius;

    private WaitForSeconds summonDelay;

    private Collider[] sensingCollisionArray = new Collider[1];
    public override void ActivateSkill()
    {
        base.ActivateSkill();

        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        base.UpdateSkillData();

        currentMeteorCount = meteorCount * level;
    }
    protected override void SetCurrentRange(float value)
    {
        base.SetCurrentRange(value);

        explosionRadius += originExplosionRadius * value / 100;
        activeObjectUtility.SetAttackRadius(explosionRadius);
    }
    private IEnumerator Co_SummonMeteor() //메테오 생성 코루틴
    {
        int num = Physics.OverlapSphereNonAlloc(transform.root.position, sensingRadius, sensingCollisionArray, ConstDefine.LAYER_MONSTER);
        if (num == 0)
        {
            yield break;
        }
#if UNITY_EDITOR
        AttackCount++;
#endif
        for (int i = 0; i < currentMeteorCount; i++) //현재 메테오 개수만큼 리스트에서 활성화
        {
            ActiveObject obj = activeObjectUtility.GetObject();
            obj.transform.position = GetRandomPos(); //반경 내 랜덤한 몬스터 위치로 설정

            obj.Activate();

            if (i < meteorCount - 1) yield return summonDelay; //마지막 메테오 소환 시에는 텀 없게하기
        }
    }
    protected override IEnumerator Co_ActiveSkillAction() //액티브 스킬 기능 코루틴
    {

        while (true)
        {
            yield return coolTimeDelay;
            yield return StartCoroutine(Co_SummonMeteor());
        }
    }
    private Vector3 GetRandomPos()
    {
        float xPos = Random.Range(-sensingRadius, sensingRadius + 1); //공격 반경만큼 랜덤한 x, z 위치 설정
        float yPos = Random.Range(-sensingRadius, sensingRadius + 1);
        Vector3 randomPos = new Vector3(transform.root.position.x + xPos, 0, transform.root.position.z + yPos);

        float distance = Vector3.Distance(randomPos, transform.root.position); //플레이어와 랜덤 설정된 위치 사이의 거리 체크
        Debug.Log(distance);
        if(distance > sensingRadius) //공격 반경보다 거리가 더 먼 경우
        {
            Vector3 direction = (transform.root.position - randomPos).normalized; //플레이어 방향으로 반경에서 벗어난 만큼 옮겨줌
            randomPos += direction * (distance - sensingRadius);
        }
        return randomPos;
    }
    protected override void InitActiveObject()
    {
        base.InitActiveObject();
        activeObjectUtility.SetAttackRadius(explosionRadius);
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseDamage) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.MeteorShower);
            bCanEvolution = true;
        }
    }
    protected override void ReadCSVData()
    {
        base.ReadCSVData();

        sensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 4);

        meteorCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 14);
        currentMeteorCount = meteorCount;

        summonInteval = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 19);
        summonDelay = new WaitForSeconds(summonInteval);

        explosionRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 27);
        originExplosionRadius = explosionRadius;
    }
}
