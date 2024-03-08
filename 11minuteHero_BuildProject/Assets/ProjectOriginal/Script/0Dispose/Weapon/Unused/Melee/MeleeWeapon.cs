using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon2
{
    [Range(0f,360f)]
    [SerializeField] protected float attackAngle; //공격 범위
    [SerializeField] protected float attackRadius; //공격 사거리

    protected List<Monster> inRangeMonsterList = new List<Monster>(); //사거리 내에 들어온 몬스터

    [SerializeField] private LayerMask layerMask;

    [SerializeField] protected GameObject particlePrefab; //공격 이펙트 프리팹
    protected ParticleSystem particleSystem; //공격 이펙트 파티클

    //protected override void Start()
    //{
    //    base.Start();
    //    CreateParticle();
    //}
    protected virtual void PlayParticle() //파티클 위치 조정 및 플레이
    {
        particleSystem.transform.position = InGameManager.Instance.Player.transform.position + (Vector3.up * 0.5f) + (transform.root.forward.normalized * 0.5f); //플레이어의 정면 방향으로
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
    protected bool CheckMonsterInAttackRange()
    {
        inRangeMonsterList.Clear();
        Collider[] inRadiusMonsterArray = Physics.OverlapSphere(transform.root.position, attackRadius, layerMask);
        if (inRadiusMonsterArray.Length == 0) return false; //배열 크기가 0이면 (원 내에 들어온 충돌체가 없으면) false 반환.

        foreach (var monster in inRadiusMonsterArray)
        {                                                         
            Vector3 targetDir = (monster.transform.position - transform.root.position).normalized; //타겟 방향 벡터 정규화.
            //Vector3.Dot()를 통해 플레이어랑 타겟의 각도를 구함.           
            float targetAngle = Mathf.Acos(Vector3.Dot(transform.root.forward, targetDir)) * Mathf.Rad2Deg; //Acos의 반환값이 호도(radian)이기 때문에, attackAngle과 비교를 위해
                                                                                                            //각으로 바꿔주기 위해 상수 
            if (targetAngle <= attackAngle * 0.5f) //양옆으로로 나뉘기 때문에 0.5 곱함. 바로보고있는 방향을 기준으로 양 옆으로 각이 펼쳐지기 때문에
            { //타겟이 공격 범위 내에 들어와있으면 list에 타겟의 Monster 추가.
                inRangeMonsterList.Add(monster.GetComponent<Monster>());
            }
        }
        return inRangeMonsterList.Count > 0? true : false;
    }
    public override void Attack()
    {
        PlayParticle(); //공격 이펙트 재생
        if (!CheckMonsterInAttackRange()) return; //범위 내에 몬스터가 없으면 return
        foreach (var monster in inRangeMonsterList)
        {
            monster.Hit(damage); //공격범위 내 몬스터들 타격
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.root.position, attackRadius);

        float lookingAngle = transform.root.eulerAngles.y;
        Vector3 rightDir = AngleToDirection(lookingAngle + attackAngle * 0.5f); //우측으로 각도를 벌림
        Vector3 leftDir = AngleToDirection(lookingAngle - attackAngle * 0.5f); //좌측으로 각도를 벌림 // 0.5를 곱해주는 이유는 총 공격 범위 각도를 좌,우측으로 나누기 때문임

        Debug.DrawRay(transform.root.position, rightDir * attackRadius, Color.red);
        Debug.DrawRay(transform.root.position, leftDir * attackRadius, Color.red);
        Debug.DrawRay(transform.root.position, transform.root.forward * attackRadius, Color.cyan);
    }
    //degree : 일반적으로 사용하는 원 한바퀴의 각도. 0 ~ 360도
    //radian : 호도, 1라디안이 57.3도 정도가 된다.
    private Vector3 AngleToDirection(float angle) //들어온 degree각을 radian각으로 변환한 후 삼각함수를 사용, 방향 벡터를 반환함.
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }
}
