using System.Collections;
using UnityEngine;

public class ActiveStaff : ActiveSkillUsingProjectile //�����簡 ����ϴ� ����. (09/25)��Ƽ�� ��ų ���� �Ϸ� �� �����丵 �ÿ� Staff�� Ŭ���� �̸� �ٲ����
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

        int monsterIndex = 0; //�� ����ü�� Ÿ���� �� ������ �ε���
        for (int i = 0; i < currentShotCount; i++) //����ü ������ŭ �ݺ�
        {
            if (!projectileUtility.IsValid())
            {
                InitProjectile();
            }
            Projectile p = projectileUtility.GetProjectile();

            p.SetTargetTransform(sensingCollisionArray[monsterIndex++].transform);

            p.ShotProjectile(); //��ġ���� ������ ���� Ÿ�� ������ transform �Ѱ���

            if (monsterIndex >= num) monsterIndex = 0; //�ݰ� ���� ���Ͱ� ����ü ������ ���� ����� ó��

            if (monsterIndex >= maxTargetCount) monsterIndex = 0; //�ݰ� �� Ÿ�� ������ �ִ� ���� ���� ������ ��� �ε��� �ʱ�ȭ

            if (i < currentShotCount - 1) yield return shotDelay; //������ ����ü �߻� �ÿ��� �� ���� �ϱ�
        }
    }
    private void OnDrawGizmos() //���信�� ���� �ݰ� ǥ�ø� ���� ����� �׸���
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

#region -- ��� ���� --
//private List<Monster> GetMonsterList() //������ ���� ����Ʈ�� ��ȯ
//{
//    //���� ����ü ���� �ʵ� �� ���� ������ ���� ��� ������ ������ ���� �ʵ� �� ������ ��. �ݴ�� ����ü�� ��
//    int monsterCount = InGameManager.Instance.MonsterList.Count < rangedAttackUtility.ShotCount? InGameManager.Instance.MonsterList.Count : rangedAttackUtility.ShotCount;

//    if (monsterCount == 0) return null; //�ʵ� ���� ���Ͱ� ���� ��� null ����

//    //�ʵ� �� ���� ����Ʈ�� �÷��̾���� �Ÿ��� ���� ������ �� ������ ���� ����ŭ ĳ��
//    var nearMonsterList = InGameManager.Instance.MonsterList.OrderBy(o => o.DistToPlayer).Take(monsterCount).ToList();

//    //���� �� ������ ���� ����ŭ ĳ���� ����Ʈ���� ���� �ݰ� ���� �ִ� ���͸� �ٽ� ĳ��
//    var inRangeList = nearMonsterList.Where(w => Mathf.Sqrt(w.DistToPlayer) <= attackRadius).ToList();

//    if (inRangeList.Count == 0) //�ݰ� ���� �ִ� ���� ���� ���� ��ȯ ó��
//    {
//        return nearMonsterList;
//    }
//    else
//    {
//        return inRangeList;
//    }
//}
//private bool FindTargetMonster() //���� ���� ���� ���Ͱ� �����ϴ� ���� �׷��� ���� ���� ����
//{
//    Collider[] inRadiusMonsterArray = Physics.OverlapSphere(transform.root.position, attackRadius, layerMask); //���� �ݰ游ŭ ���� �׸�

//    if (inRadiusMonsterArray.Length == 0) return FindNearMonsters(); //���� ���� ���� ���Ͱ� ������ �ʵ� ������ Ž��

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
//private bool FindNearMonsters() //����� ���� Ž��
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
//protected override void SummonProjectile() //����ü ���� �Լ�
//{
//    base.SummonProjectile();
//    nearMonsterQueue.Clear(); //Ÿ�� ����Ʈ �ʱ�ȭ
//    bKnockBackList.Clear();

//    if (!FindTargetMonster()) return; //Ÿ�� ���� Ž��

//    int monsterIndex = 0; //�� ����ü�� Ÿ���� �� ������ �ε���
//    for (int i = 0; i < count; i++) //����ü ������ŭ �ݺ�
//    {
//        if (projectileQueue.Count <= 0)//Ǯ�� ����ü�� ������ ���� ����
//        {
//            CreateNewProjectile(projectileCreateCount);
//            SetProjectile();
//        }
//        Projectile p = projectileQueue.Dequeue();

//        p.transform.SetParent(spawnPoint); //Ǯ���� ���� ����ü ��ġ �ʱ�ȭ.
//        Vector3 randPos = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f)); //����ü�� ���ĺ����� �ʰ� �÷��̾� �ֺ� ������ ��ġ���� ����
//        p.transform.position = InGameManager.Instance.Player.transform.position + randPos;
//        //p.transform.localRotation = Quaternion.Euler(90, 0, 0);

//        p.transform.SetParent(activatedProjectileParent); //���� �θ� �ٲ���. �ȹٲ��ָ� ������ġ�� ���ӵǼ� �÷��̾� �̵��� ����ü�� �����

//        p.gameObject.SetActive(true);

//        p.ShotProjectile(nearMonsterQueue.Dequeue()); //����ü �߻� �Լ�. Ÿ�� ���� ����Ʈ�� monsterIndex��° ���� ������ �Ѱ���

//        if (monsterIndex >= nearMonsterQueue.Count) monsterIndex = 0; //Ÿ�� ���� ����Ʈ���� 3������ �ִµ�, ����ü�� 4�� �̻��� ��츦 ���� ó��
//    }
//}
#endregion