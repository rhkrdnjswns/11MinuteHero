using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WSword : AWeapon
{
    [Range(0f, 360f)]
    [SerializeField] protected float attackAngle; //���� ����

    [SerializeField] protected AttackRadiusUtility attackRadiusUtility;

    [SerializeField] private GameObject particlePrefab; //���� ����Ʈ ������
    private ParticleSystem particleSystem; //���� ����Ʈ ��ƼŬ
    protected ParticleSystem.MainModule main;

    private float originRadius; //�нú� ���� ���� ����� ���� ���� ������ ���� �⺻ ������

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
        Gizmos.DrawWireSphere(transform.root.position, attackRadiusUtility.Radius);

        float lookingAngle = transform.root.eulerAngles.y;
        Vector3 rightDir = AngleToDirection(lookingAngle + attackAngle * 0.5f); //�������� ������ ����
        Vector3 leftDir = AngleToDirection(lookingAngle - attackAngle * 0.5f); //�������� ������ ���� // 0.5�� �����ִ� ������ �� ���� ���� ������ ��,�������� ������ ������

        Debug.DrawRay(transform.root.position, rightDir * attackRadiusUtility.Radius, Color.red);
        Debug.DrawRay(transform.root.position, leftDir * attackRadiusUtility.Radius, Color.red);
        Debug.DrawRay(transform.root.position, transform.root.forward * attackRadiusUtility.Radius, Color.cyan);
    }
    protected virtual IEnumerator Co_AttackInRangeMonster() //���� �� ���� ������ ���� ���� üũ
    {
        Collider[] inRadiusMonsterArray = attackRadiusUtility.GetLayerInRadius(transform.root);
        if (inRadiusMonsterArray.Length == 0) yield break; //���� �ȿ� ���� ������ ����

        foreach (var monster in inRadiusMonsterArray) //���� �� ���� �� ���� ������ ������ �ִ� ���� �˻�
        {
            Vector3 targetDir = (monster.transform.position - transform.root.position).normalized; //Ÿ�� ���� ���� ����ȭ.
            //Vector3.Dot()�� ���� �÷��̾�� Ÿ���� ������ ����.           
            float targetAngle = Mathf.Acos(Vector3.Dot(transform.root.forward, targetDir)) * Mathf.Rad2Deg; //Acos�� ��ȯ���� ȣ��(radian)�̱� ������, attackAngle�� �񱳸� ����
                                                                                                            //������ �ٲ��ֱ� ���� ����� ������
            if (targetAngle <= attackAngle * 0.5f) //�翷���η� ������ ������ 0.5 ����. �ٷκ����ִ� ������ �������� �� ������ ���� �������� ������
            { 
                monster.GetComponent<Character>().Hit(currentDamage); //���� ���� �ִ� ��� Ÿ��
            }
            yield return null;
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
}
