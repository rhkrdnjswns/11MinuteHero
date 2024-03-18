using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RStaff : WRangedWeapon //�����簡 ����ϴ� ����. (09/25)��Ƽ�� ��ų ���� �Ϸ� �� �����丵 �ÿ� Staff�� Ŭ���� �̸� �ٲ����
{
    [SerializeField] private AttackRadiusUtility attackRadiusUtility;
    [SerializeField] private float shotInterval; //����ü ���� �ֱ�

    private bool bShotDone;
    protected override void Awake()
    {
        base.Awake();
        string[] info = FindObjectOfType<CSVReader>().GetSkillNameAndDescription(eSkillType, id);
        if(info != null)
        {
            name = info[0];
            description = info[1];
        }
        damage = FindObjectOfType<CSVReader>().GetBaseDamage(eSkillType, id);
        coolTime = FindObjectOfType<CSVReader>().GetCoolTime(eSkillType, id);
    }
    public override bool GetBShotDone()
    {
        return bShotDone;
    }
    public override void InitSkill() //���� �ʱ�ȭ
    {
        base.InitSkill();
        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        SetCurrentDamage();
        rangedAttackUtility.ShotCount = 2 * level;
    }
    protected override IEnumerator Co_Shot()
    {
        bShotDone = false;
        Collider[] inRadiusMonsterArray = attackRadiusUtility.GetLayerInRadiusSortedByDistance(transform.root); //���� ���� ���� ����� ���� Ž��
        if (inRadiusMonsterArray.Length == 0)
        {
            bShotDone = true;
            yield break;
        }
        int monsterIndex = 0; //�� ����ü�� Ÿ���� �� ������ �ε���
        for (int i = 0; i < rangedAttackUtility.ShotCount; i++) //����ü ������ŭ �ݺ�
        {
            if (!rangedAttackUtility.IsValid())
            {
                rangedAttackUtility.CreateNewProjectile();
            }
            Projectile p = rangedAttackUtility.SummonProjectile();

            p.ShotProjectile(inRadiusMonsterArray[monsterIndex++].transform); //��ġ���� ������ ���� Ÿ�� ������ transform �Ѱ���

            if (monsterIndex >= inRadiusMonsterArray.Length) monsterIndex = 0; //�ݰ� ���� ���Ͱ� ����ü ������ ���� ����� ó��

            if (i < rangedAttackUtility.ShotCount - 1) yield return new WaitForSeconds(shotInterval); //������ ����ü �߻� �ÿ��� �� ���� �ϱ�
        }
        bShotDone = true;
    }
    private void OnDrawGizmos() //���信�� ���� �ݰ� ǥ�ø� ���� ����� �׸���
    {
        Gizmos.DrawWireSphere(transform.root.position, attackRadiusUtility.Radius);
    }
    public override void SetEvlotionCondition()
    {
        if (level == ConstDefine.SKILL_MAX_LEVEL && InGameManager.Instance.SkillManager.GetSkillLevel((int)ESkillPassiveID.IncreaseExpGain) > 0)
        {
            InGameManager.Instance.SkillManager.SetCanEvolution((int)ESkillEvolutionIndex.MagicMissile);
            bCanEvolution = true;
        }
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