using System.Collections;
using UnityEngine;

public class ActiveStaff : ActiveSkillUsingProjectile //마법사가 사용하는 무기. (09/25)액티브 스킬 제작 완료 후 리팩토링 시에 Staff로 클래스 이름 바꿔야함
{
    [SerializeField] protected float shotInterval;
    [SerializeField] protected int maxTargetCount;
    [SerializeField] protected float sensingRadius;

    private Collider[] sensingCollisionArray = new Collider[50];
    protected WaitForSeconds shotDelay;
    public override void ActivateSkill()
    {
        base.ActivateSkill();

        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        base.UpdateSkillData();
        currentShotCount = shotCount * level;
    }
    private IEnumerator Co_Shot()
    {
        int num = Physics.OverlapSphereNonAlloc(transform.root.position, sensingRadius, sensingCollisionArray, ConstDefine.LAYER_MONSTER);
        if (num == 0) yield break;

#if UNITY_EDITOR
        AttackCount++;
#endif
        AttackInRangeUtility.QuickSortCollisionArray(sensingCollisionArray, 0, num - 1);

        int monsterIndex = 0; //각 투사체의 타겟이 될 몬스터의 인덱스
        for (int i = 0; i < currentShotCount; i++) //투사체 개수만큼 반복
        {
            if (!projectileUtility.IsValid())
            {
                InitProjectile();
            }
            Projectile p = projectileUtility.GetProjectile();

            p.SetTargetTransform(sensingCollisionArray[monsterIndex++].transform);

            p.ShotProjectile(); //위치정보 참조를 위해 타겟 몬스터의 transform 넘겨줌

            if (monsterIndex >= num) monsterIndex = 0; //반경 내의 몬스터가 투사체 수보다 적은 경우의 처리

            if (monsterIndex >= maxTargetCount) monsterIndex = 0; //반경 내 타격 가능한 최대 몬스터 수에 도달한 경우 인덱스 초기화

            if (i < currentShotCount - 1) yield return shotDelay; //마지막 투사체 발사 시에는 텀 없게 하기
        }
    }
    private void OnDrawGizmos() //씬뷰에서 공격 반경 표시를 위한 기즈모 그리기
    {
        Gizmos.DrawWireSphere(transform.root.position, sensingRadius);
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseExpGain) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.MagicMissile);
            bCanEvolution = true;
        }
    }

    protected override IEnumerator Co_ActiveSkillAction()
    {
        while(true)
        {
            yield return coolTimeDelay;
            if (InGameManager.Instance.Player.IsDodge) continue;

            yield return StartCoroutine(Co_Shot());
        }
    }
    protected override void ReadActiveCSVData()
    {
        base.ReadActiveCSVData();

        sensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 4);
        maxTargetCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 9);

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 14);
        currentShotCount = shotCount;

        speed = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 18);

        shotInterval = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 19);
        shotDelay = new WaitForSeconds(shotInterval);
    }
}

#region -- 폐기 로직 --
//private List<Monster> GetMonsterList() //공격할 몬스터 리스트를 반환
//{
//    //현재 투사체 수가 필드 위 몬스터 수보다 많은 경우 공격할 몬스터의 수는 필드 위 몬스터의 수. 반대는 투사체의 수
//    int monsterCount = InGameManager.Instance.MonsterList.Count < rangedAttackUtility.ShotCount? InGameManager.Instance.MonsterList.Count : rangedAttackUtility.ShotCount;

//    if (monsterCount == 0) return null; //필드 위에 몬스터가 없는 경우 null 리턴

//    //필드 위 몬스터 리스트를 플레이어와의 거리에 따라 정렬한 후 공격할 몬스터 수만큼 캐싱
//    var nearMonsterList = InGameManager.Instance.MonsterList.OrderBy(o => o.DistToPlayer).Take(monsterCount).ToList();

//    //정렬 후 공격할 몬스터 수만큼 캐싱한 리스트에서 공격 반경 내에 있는 몬스터를 다시 캐싱
//    var inRangeList = nearMonsterList.Where(w => Mathf.Sqrt(w.DistToPlayer) <= attackRadius).ToList();

//    if (inRangeList.Count == 0) //반경 내에 있는 몬스터 수에 따른 반환 처리
//    {
//        return nearMonsterList;
//    }
//    else
//    {
//        return inRangeList;
//    }
//}
//private bool FindTargetMonster() //공격 범위 내에 몬스터가 존재하는 경우와 그렇지 않은 경우로 나뉨
//{
//    Collider[] inRadiusMonsterArray = Physics.OverlapSphere(transform.root.position, attackRadius, layerMask); //공격 반경만큼 원을 그림

//    if (inRadiusMonsterArray.Length == 0) return FindNearMonsters(); //공격 범위 내에 몬스터가 없으면 필드 위에서 탐색

//    foreach (var item in inRadiusMonsterArray)
//    {
//        nearMonsterQueue.Enqueue(item.GetComponent<Monster>());
//    }
//    int monsterCount = inRadiusMonsterArray.Length < count ? inRadiusMonsterArray.Length : count;
//    for (int i = 0; i < monsterCount; i++)
//    {
//        bKnockBackList.Add(false);
//    }
//    return true;
//}
//private bool FindNearMonsters() //가까운 몬스터 탐색
//{
//    if (InGameManager.Instance.MonsterList.Count == 0) return false;

//    foreach (var item in InGameManager.Instance.MonsterList)
//    {
//        nearMonsterQueue.Enqueue(item);
//    }
//    int monsterCount = InGameManager.Instance.MonsterList.Count < count ? InGameManager.Instance.MonsterList.Count : count;
//    for (int i = 0; i < monsterCount; i++)
//    {
//        bKnockBackList.Add(false);
//    }
//    return true;
//}
//protected override void SummonProjectile() //투사체 생성 함수
//{
//    base.SummonProjectile();
//    nearMonsterQueue.Clear(); //타겟 리스트 초기화
//    bKnockBackList.Clear();

//    if (!FindTargetMonster()) return; //타겟 몬스터 탐색

//    int monsterIndex = 0; //각 투사체의 타겟이 될 몬스터의 인덱스
//    for (int i = 0; i < count; i++) //투사체 개수만큼 반복
//    {
//        if (projectileQueue.Count <= 0)//풀에 투사체가 없으면 새로 생성
//        {
//            CreateNewProjectile(projectileCreateCount);
//            SetProjectile();
//        }
//        Projectile p = projectileQueue.Dequeue();

//        p.transform.SetParent(spawnPoint); //풀에서 꺼낸 투사체 위치 초기화.
//        Vector3 randPos = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f)); //투사체가 겹쳐보이지 않게 플레이어 주변 랜덤한 위치에서 생성
//        p.transform.position = InGameManager.Instance.Player.transform.position + randPos;
//        //p.transform.localRotation = Quaternion.Euler(90, 0, 0);

//        p.transform.SetParent(activatedProjectileParent); //이후 부모를 바꿔줌. 안바꿔주면 생성위치에 종속되서 플레이어 이동을 투사체가 따라옴

//        p.gameObject.SetActive(true);

//        p.ShotProjectile(nearMonsterQueue.Dequeue()); //투사체 발사 함수. 타겟 몬스터 리스트의 monsterIndex번째 몬스터 참조를 넘겨줌

//        if (monsterIndex >= nearMonsterQueue.Count) monsterIndex = 0; //타겟 몬스터 리스트에는 3마리만 있는데, 투사체는 4개 이상인 경우를 위한 처리
//    }
//}
#endregion