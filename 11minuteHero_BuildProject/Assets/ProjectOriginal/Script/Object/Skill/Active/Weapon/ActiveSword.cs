using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActiveSword : ActiveSkill
{
    [Range(0f, 360f)]
    [SerializeField] protected float attackAngle; //���� ����
    [SerializeField] protected float sensingRadius; //���� �ݰ�

    [SerializeField] private GameObject particlePrefab; //���� ����Ʈ ������
    protected ParticleSystem particleSystem; //���� ����Ʈ ��ƼŬ

    protected Collider[] sensingCollisionArray = new Collider[30];
    protected float originRadius; //�нú� ���� ���� ����� ���� ���� ������ ���� �⺻ ������

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
    private void PlayParticle() //��ƼŬ ��ġ ���� �� �÷���
    {
        particleSystem.transform.position = InGameManager.Instance.Player.transform.position +
            (Vector3.up * 0.5f) + (transform.root.forward * 0.5f); //�÷��̾��� ���� ��������
                                                                              //������ 0.5 ������                                                                                                                                                                                                                
        particleSystem.transform.rotation = InGameManager.Instance.Player.transform.rotation;

        particleSystem.Play();
    }
    private void CreateParticle() //��ƼŬ ������Ʈ ���� �� ���۷��� �ʱ�ȭ
    {
        GameObject obj = Instantiate(particlePrefab);
        obj.transform.SetParent(GameObject.Find(ConstDefine.NAME_FIELD).transform);
        particleSystem = obj.GetComponent<ParticleSystem>();
    }
    private void OnDrawGizmos() //���信 ���� ���� ǥ�ø� ���� ����� �׸���
    {
        Gizmos.DrawWireSphere(transform.root.position, sensingRadius);

        float lookingAngle = transform.root.eulerAngles.y;
        Vector3 rightDir = AngleToDirection(lookingAngle + attackAngle * 0.5f); //�������� ������ ����
        Vector3 leftDir = AngleToDirection(lookingAngle - attackAngle * 0.5f); //�������� ������ ���� // 0.5�� �����ִ� ������ �� ���� ���� ������ ��,�������� ������ ������

        Debug.DrawRay(transform.root.position, rightDir * sensingRadius, Color.red);
        Debug.DrawRay(transform.root.position, leftDir * sensingRadius, Color.red);
        Debug.DrawRay(transform.root.position, transform.root.forward * sensingRadius, Color.cyan);
    }
    protected virtual void AttackInRangeMonster() //���� �� ���� ������ ���� ���� üũ
    {
        int num = Physics.OverlapSphereNonAlloc(transform.root.position, sensingRadius, sensingCollisionArray, ConstDefine.LAYER_MONSTER);
        if (num == 0) return;

#if UNITY_EDITOR
        AttackCount++;
#endif
        for (int i = 0; i < num; i++)
        {
            Vector3 targetDir = (sensingCollisionArray[i].transform.position - transform.root.position).normalized; //Ÿ�� ���� ���� ����ȭ.
            //Vector3.Dot()�� ���� �÷��̾�� Ÿ���� ������ ����.           
            float targetAngle = Mathf.Acos(Vector3.Dot(transform.root.forward, targetDir)) * Mathf.Rad2Deg; //Acos�� ��ȯ���� ȣ��(radian)�̱� ������, attackAngle�� �񱳸� ����
                                                                                                            //������ �ٲ��ֱ� ���� ����� ������
            if (targetAngle <= attackAngle * 0.5f) //�翷���η� ������ ������ 0.5 ����. �ٷκ����ִ� ������ �������� �� ������ ���� �������� ������
            {
                sensingCollisionArray[i].GetComponent<Character>()?.Hit(currentDamage); //���� ���� �ִ� ��� Ÿ��
#if UNITY_EDITOR
                TotalDamage += currentDamage;
#endif
            }
        }
    }
    //degree : �Ϲ������� ����ϴ� �� �ѹ����� ����. 0 ~ 360��
    //radian : ȣ��, 1������ 57.3�� ������ �ȴ�.
    private Vector3 AngleToDirection(float angle) //���� degree���� radian������ ��ȯ�� �� �ﰢ�Լ��� ���, ���� ���͸� ��ȯ��.
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
