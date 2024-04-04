using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WSword : AWeapon
{
    [Range(0f, 360f)]
    [SerializeField] protected float attackAngle; //공격 범위

    [SerializeField] protected AttackRadiusUtility attackRadiusUtility;

    [SerializeField] private GameObject particlePrefab; //공격 이펙트 프리팹
    private ParticleSystem particleSystem; //공격 이펙트 파티클
    protected ParticleSystem.MainModule main;

    private float originRadius; //패시브 범위 증가 적용시 공격 범위 증가를 위한 기본 사이즈

    protected override void Awake()
    {
        base.Awake();
        CreateParticle();
        main = particleSystem.main;
        originRadius = attackRadiusUtility.Radius;
    }
    public override void InitSkill()
    {
        base.InitSkill();

        SetCurrentDamage();
    }
    public override void Attack()
    {
        PlayParticle();
        StartCoroutine(Co_AttackInRangeMonster());
    }
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
        attackRadiusUtility.Radius += 0.2f;
        main.startSize = attackRadiusUtility.Radius * 2;
    }
    protected override void SetCurrentDamage()
    {
        currentDamage = damage * level;
    }
    protected override void SetCurrentRange(float value)
    {
        attackRadiusUtility.Radius += originRadius * value / 100;
        main.startSize = attackRadiusUtility.Radius * 2;
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
        Gizmos.DrawWireSphere(transform.root.position, attackRadiusUtility.Radius);

        float lookingAngle = transform.root.eulerAngles.y;
        Vector3 rightDir = AngleToDirection(lookingAngle + attackAngle * 0.5f); //우측으로 각도를 벌림
        Vector3 leftDir = AngleToDirection(lookingAngle - attackAngle * 0.5f); //좌측으로 각도를 벌림 // 0.5를 곱해주는 이유는 총 공격 범위 각도를 좌,우측으로 나누기 때문임

        Debug.DrawRay(transform.root.position, rightDir * attackRadiusUtility.Radius, Color.red);
        Debug.DrawRay(transform.root.position, leftDir * attackRadiusUtility.Radius, Color.red);
        Debug.DrawRay(transform.root.position, transform.root.forward * attackRadiusUtility.Radius, Color.cyan);
    }
    protected virtual IEnumerator Co_AttackInRangeMonster() //공격 시 공격 범위에 들어온 몬스터 체크
    {
        Collider[] inRadiusMonsterArray = attackRadiusUtility.GetLayerInRadius(transform.root);
        if (inRadiusMonsterArray.Length == 0) yield break; //범위 안에 몬스터 없으면 종료

        foreach (var monster in inRadiusMonsterArray) //범위 안 몬스터 중 공격 가능한 각도에 있는 몬스터 검사
        {
            Vector3 targetDir = (monster.transform.position - transform.root.position).normalized; //타겟 방향 벡터 정규화.
            //Vector3.Dot()를 통해 플레이어랑 타겟의 각도를 구함.           
            float targetAngle = Mathf.Acos(Vector3.Dot(transform.root.forward, targetDir)) * Mathf.Rad2Deg; //Acos의 반환값이 호도(radian)이기 때문에, attackAngle과 비교를 위해
                                                                                                            //각으로 바꿔주기 위해 상수를 곱해줌
            if (targetAngle <= attackAngle * 0.5f) //양옆으로로 나뉘기 때문에 0.5 곱함. 바로보고있는 방향을 기준으로 양 옆으로 각이 펼쳐지기 때문에
            { 
                monster.GetComponent<Character>().Hit(currentDamage); //각도 내에 있는 경우 타격
            }
            yield return null;
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
}
