using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon2
{
    [Range(0f,360f)]
    [SerializeField] protected float attackAngle; //���� ����
    [SerializeField] protected float attackRadius; //���� ��Ÿ�

    protected List<Monster> inRangeMonsterList = new List<Monster>(); //��Ÿ� ���� ���� ����

    [SerializeField] private LayerMask layerMask;

    [SerializeField] protected GameObject particlePrefab; //���� ����Ʈ ������
    protected ParticleSystem particleSystem; //���� ����Ʈ ��ƼŬ

    //protected override void Start()
    //{
    //    base.Start();
    //    CreateParticle();
    //}
    protected virtual void PlayParticle() //��ƼŬ ��ġ ���� �� �÷���
    {
        particleSystem.transform.position = InGameManager.Instance.Player.transform.position + (Vector3.up * 0.5f) + (transform.root.forward.normalized * 0.5f); //�÷��̾��� ���� ��������
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
    protected bool CheckMonsterInAttackRange()
    {
        inRangeMonsterList.Clear();
        Collider[] inRadiusMonsterArray = Physics.OverlapSphere(transform.root.position, attackRadius, layerMask);
        if (inRadiusMonsterArray.Length == 0) return false; //�迭 ũ�Ⱑ 0�̸� (�� ���� ���� �浹ü�� ������) false ��ȯ.

        foreach (var monster in inRadiusMonsterArray)
        {                                                         
            Vector3 targetDir = (monster.transform.position - transform.root.position).normalized; //Ÿ�� ���� ���� ����ȭ.
            //Vector3.Dot()�� ���� �÷��̾�� Ÿ���� ������ ����.           
            float targetAngle = Mathf.Acos(Vector3.Dot(transform.root.forward, targetDir)) * Mathf.Rad2Deg; //Acos�� ��ȯ���� ȣ��(radian)�̱� ������, attackAngle�� �񱳸� ����
                                                                                                            //������ �ٲ��ֱ� ���� ��� 
            if (targetAngle <= attackAngle * 0.5f) //�翷���η� ������ ������ 0.5 ����. �ٷκ����ִ� ������ �������� �� ������ ���� �������� ������
            { //Ÿ���� ���� ���� ���� ���������� list�� Ÿ���� Monster �߰�.
                inRangeMonsterList.Add(monster.GetComponent<Monster>());
            }
        }
        return inRangeMonsterList.Count > 0? true : false;
    }
    public override void Attack()
    {
        PlayParticle(); //���� ����Ʈ ���
        if (!CheckMonsterInAttackRange()) return; //���� ���� ���Ͱ� ������ return
        foreach (var monster in inRangeMonsterList)
        {
            monster.Hit(damage); //���ݹ��� �� ���͵� Ÿ��
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.root.position, attackRadius);

        float lookingAngle = transform.root.eulerAngles.y;
        Vector3 rightDir = AngleToDirection(lookingAngle + attackAngle * 0.5f); //�������� ������ ����
        Vector3 leftDir = AngleToDirection(lookingAngle - attackAngle * 0.5f); //�������� ������ ���� // 0.5�� �����ִ� ������ �� ���� ���� ������ ��,�������� ������ ������

        Debug.DrawRay(transform.root.position, rightDir * attackRadius, Color.red);
        Debug.DrawRay(transform.root.position, leftDir * attackRadius, Color.red);
        Debug.DrawRay(transform.root.position, transform.root.forward * attackRadius, Color.cyan);
    }
    //degree : �Ϲ������� ����ϴ� �� �ѹ����� ����. 0 ~ 360��
    //radian : ȣ��, 1������ 57.3�� ������ �ȴ�.
    private Vector3 AngleToDirection(float angle) //���� degree���� radian������ ��ȯ�� �� �ﰢ�Լ��� ���, ���� ���͸� ��ȯ��.
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }
}
