using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMeteor : ActiveSkillUsingActiveObject //���׿� ��ų Ŭ����
{
    [SerializeField] private int meteorCount; //������ ���׿� ����

    [SerializeField] private float summonInteval = 0.2f; //��ȯ ����
    [SerializeField] private float sensingRadius;
    [SerializeField] private float explosionRadius;
    private float originExplosionRadius;

    private bool bActionDone = true;
    private WaitForSeconds summonDelay;

    private Collider[] sensingCollisionArray = new Collider[1];
    public override void InitSkill() //��� �ʱ�ȭ �� ���׿� ����
    {
        base.InitSkill();

        originExplosionRadius = explosionRadius;
        summonDelay = new WaitForSeconds(summonInteval);
        UpdateSkillData();
    }
    protected override void UpdateSkillData()
    {
        base.UpdateSkillData();
        meteorCount = level;
    }
    protected override void SetCurrentRange(float value)
    {
        base.SetCurrentRange(value);

        explosionRadius += originExplosionRadius * value / 100;
        activeObjectUtility.SetAttackRadius(explosionRadius);
    }
    private IEnumerator Co_SummonMeteor() //���׿� ���� �ڷ�ƾ
    {
        int num = Physics.OverlapSphereNonAlloc(transform.root.position, sensingRadius, sensingCollisionArray, ConstDefine.LAYER_MONSTER);
        if (num == 0)
        {
            yield break;
        }
#if UNITY_EDITOR
        AttackCount++;
#endif
        bActionDone = false;

        for (int i = 0; i < meteorCount; i++) //���� ���׿� ������ŭ ����Ʈ���� Ȱ��ȭ
        {
            ActiveObject obj = activeObjectUtility.GetObject();
            obj.transform.position = GetRandomPos(); //�ݰ� �� ������ ���� ��ġ�� ����

            obj.Activate();

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
        float xPos = Random.Range(-sensingRadius, sensingRadius + 1); //���� �ݰ游ŭ ������ x, z ��ġ ����
        float yPos = Random.Range(-sensingRadius, sensingRadius + 1);
        Vector3 randomPos = new Vector3(transform.root.position.x + xPos, 0, transform.root.position.z + yPos);

        float distance = Vector3.Distance(randomPos, transform.root.position); //�÷��̾�� ���� ������ ��ġ ������ �Ÿ� üũ
        Debug.Log(distance);
        if(distance > sensingRadius) //���� �ݰ溸�� �Ÿ��� �� �� ���
        {
            Vector3 direction = (transform.root.position - randomPos).normalized; //�÷��̾� �������� �ݰ濡�� ��� ��ŭ �Ű���
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
}