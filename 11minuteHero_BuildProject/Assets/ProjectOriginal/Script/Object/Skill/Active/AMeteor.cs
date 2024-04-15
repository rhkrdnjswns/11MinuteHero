using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMeteor : AActiveSkill //���׿� ��ų Ŭ����
{
    protected int meteorCount; //������ ���׿� ����
    [SerializeField] private GameObject meteorPrefab; //���׿� ������
    [SerializeField] private Queue<MeteorObject> meteorQueue = new Queue<MeteorObject>(); //���׿� Ǯ
    [SerializeField] private float summonInteval = 0.2f; //��ȯ ����

    [SerializeField] private AttackRadiusUtility attackRadiusUtility; //�� ���� ����
    [SerializeField] private AttackRadiusUtility meteorAttackRadiusUtility; //���׿� ���� ���� 
    [SerializeField] private int createCount;
    private bool bActionDone;
    
    private List<MeteorObject> allMeteorList = new List<MeteorObject>(); //��� ���׿� ���� (���׿��� ��ť�� ���¿��� �нú� ��ų ȿ�� ���� �� ������ ����)
    private float originRadius; //�⺻ ���׿� ���� ����

    private WaitForSeconds summonDelay;
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < createCount; i++) //���׿� �̸� ���� (�нú� ���� �ޱ� ���� ó��)
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
    public override void InitSkill() //��� �ʱ�ȭ �� ���׿� ����
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
    private IEnumerator Co_SummonMeteor() //���׿� ���� �ڷ�ƾ
    {
        bActionDone = false;
        Collider[] inRadiusMonsterArray = attackRadiusUtility.GetLayerInRadius(transform.root); //�ݰ� �� ���͵��� ����
        if (inRadiusMonsterArray.Length == 0)
        {
            bActionDone = true;
            yield break;
        }
#if UNITY_EDITOR
        AttackCount++;
#endif
        for (int i = 0; i < meteorCount; i++) //���� ���׿� ������ŭ ����Ʈ���� Ȱ��ȭ
        {
            var m = meteorQueue.Dequeue();
            m.transform.SetParent(null);
            m.transform.position = GetRandomPos(); //�ݰ� �� ������ ���� ��ġ�� ����

            m.ActivateSkill(transform, meteorQueue, currentDamage); //���׿� ��ȯ
            if (i < meteorCount - 1) yield return summonDelay; //������ ���׿� ��ȯ �ÿ��� �� �����ϱ�
        }
        bActionDone = true;
    }
    protected override IEnumerator Co_ActiveSkillAction() //��Ƽ�� ��ų ��� �ڷ�ƾ
    {
        WaitUntil actionDone = new WaitUntil(() => bActionDone);
        while (true)
        {
            yield return coolTimeDelay;
            StartCoroutine(Co_SummonMeteor());
            yield return actionDone; //��� ���׿��� ��ȯ�� ������ ���
        }
    }
    private Vector3 GetRandomPos()
    {
        float xPos = Random.Range(-attackRadiusUtility.Radius, attackRadiusUtility.Radius + 1); //���� �ݰ游ŭ ������ x, z ��ġ ����
        float yPos = Random.Range(-attackRadiusUtility.Radius, attackRadiusUtility.Radius + 1);
        Vector3 randomPos = new Vector3(transform.root.position.x + xPos, 0, transform.root.position.z + yPos);

        float distance = Vector3.Distance(randomPos, transform.root.position); //�÷��̾�� ���� ������ ��ġ ������ �Ÿ� üũ
        Debug.Log(distance);
        if(distance > attackRadiusUtility.Radius) //���� �ݰ溸�� �Ÿ��� �� �� ���
        {
            Vector3 direction = (transform.root.position - randomPos).normalized; //�÷��̾� �������� �ݰ濡�� ��� ��ŭ �Ű���
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
