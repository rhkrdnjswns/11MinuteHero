using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActiveSword : ActiveSkill
{
    [Range(0f, 360f)]
    [SerializeField] protected float attackAngle; //공격 범위
    [SerializeField] protected float sensingRadius; //공격 반경

    [SerializeField] private GameObject particlePrefab; //공격 이펙트 프리팹
    protected ParticleSystem particleSystem; //공격 이펙트 파티클

    protected Collider[] sensingCollisionArray = new Collider[30];
    protected float originRadius; //패시브 범위 증가 적용시 공격 범위 증가를 위한 기본 사이즈

    private float increaseRangeValue = 0.2f;
    public override void InitSkill()
    {
        base.InitSkill();

        CreateParticle();
    }
    private void Attack()
    {
        InGameManager.Instance.Player.Animator.SetTrigger(ConstDefine.TRIGGER_MELEE_ATTACK);
        PlayParticle();
        AttackInRangeMonster();
    }
    protected override void UpdateSkillData()
    {
        base.UpdateSkillData();

        sensingRadius += increaseRangeValue;
        particleSystem.transform.localScale += Vector3.one * (increaseRangeValue * 0.7f);
    }
    protected override void SetCurrentRange(float value)
    {
        float increaeValue = originRadius * value / 100;
        sensingRadius += increaeValue;
        particleSystem.transform.localScale += Vector3.one * (increaeValue * 0.7f);
    }
    private void PlayParticle() //파티클 위치 조정 및 플레이
    {
        particleSystem.transform.position = InGameManager.Instance.Player.transform.position +
            (Vector3.up * 0.5f) + (transform.root.forward * 0.5f); //플레이어의 정면 방향으로
                                                                              //앞으로 0.5 더해줌                                                                                                                                                                                                                
        particleSystem.transform.rotation = InGameManager.Instance.Player.transform.rotation;

        particleSystem.Play();
    }
    private void CreateParticle() //파티클 오브젝트 생성 및 레퍼런스 초기화
    {
        GameObject obj = Instantiate(particlePrefab);
        obj.transform.SetParent(GameObject.Find(ConstDefine.NAME_FIELD).transform);
        particleSystem = obj.GetComponent<ParticleSystem>();
    }
    private void OnDrawGizmos() //씬뷰에 공격 범위 표시를 위한 기즈모 그리기
    {
        Gizmos.DrawWireSphere(transform.root.position, sensingRadius);

        float lookingAngle = transform.root.eulerAngles.y;
        Vector3 rightDir = AngleToDirection(lookingAngle + attackAngle * 0.5f); //우측으로 각도를 벌림
        Vector3 leftDir = AngleToDirection(lookingAngle - attackAngle * 0.5f); //좌측으로 각도를 벌림 // 0.5를 곱해주는 이유는 총 공격 범위 각도를 좌,우측으로 나누기 때문임

        Debug.DrawRay(transform.root.position, rightDir * sensingRadius, Color.red);
        Debug.DrawRay(transform.root.position, leftDir * sensingRadius, Color.red);
        Debug.DrawRay(transform.root.position, transform.root.forward * sensingRadius, Color.cyan);
    }
    protected virtual void AttackInRangeMonster() //공격 시 공격 범위에 들어온 몬스터 체크
    {
        int num = Physics.OverlapSphereNonAlloc(transform.root.position, sensingRadius, sensingCollisionArray, ConstDefine.LAYER_MONSTER);
        if (num == 0) return;

#if UNITY_EDITOR
        AttackCount++;
#endif
        for (int i = 0; i < num; i++)
        {
            Vector3 targetDir = (sensingCollisionArray[i].transform.position - transform.root.position).normalized; //타겟 방향 벡터 정규화.
            //Vector3.Dot()를 통해 플레이어랑 타겟의 각도를 구함.           
            float targetAngle = Mathf.Acos(Vector3.Dot(transform.root.forward, targetDir)) * Mathf.Rad2Deg; //Acos의 반환값이 호도(radian)이기 때문에, attackAngle과 비교를 위해
                                                                                                            //각으로 바꿔주기 위해 상수를 곱해줌
            if (targetAngle <= attackAngle * 0.5f) //양옆으로로 나뉘기 때문에 0.5 곱함. 바로보고있는 방향을 기준으로 양 옆으로 각이 펼쳐지기 때문에
            {
                sensingCollisionArray[i].GetComponent<Character>()?.Hit(currentDamage); //각도 내에 있는 경우 타격
#if UNITY_EDITOR
                TotalDamage += currentDamage;
#endif
            }
        }
    }
    //degree : 일반적으로 사용하는 원 한바퀴의 각도. 0 ~ 360도
    //radian : 호도, 1라디안이 57.3도 정도가 된다.
    private Vector3 AngleToDirection(float angle) //들어온 degree각을 radian각으로 변환한 후 삼각함수를 사용, 방향 벡터를 반환함.
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }
    public override void SetEvlotionCondition()
    { 
        if(level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseHp) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.HolySword);
            bCanEvolution = true;
        }
    }
    protected override IEnumerator Co_ActiveSkillAction()
    {
        while(true)
        {
            yield return coolTimeDelay;
            if (InGameManager.Instance.Player.IsDodge) continue;
            Attack();
        }
    }
    protected override void ReadCSVData()
    {
        base.ReadCSVData();

        if (eSkillType == ESkillType.Evolution) return;
        sensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 10);
        increaseRangeValue = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 11);
        attackAngle = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 12);

        originRadius = sensingRadius;
    }
}
