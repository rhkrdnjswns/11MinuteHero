using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// * �Ϲ� ���� AI�� ���¸ӽ����� ������
public class Monster : Character
{
    public enum EMonsterState
    {
        Chase = 0,
        Attack
    }

    [SerializeField] protected EMonsterState monsterState;
    [SerializeField] protected float damage; //������

    [SerializeField] private int rewardExp;
    [SerializeField] private float attackRange;

    [SerializeField] private GameObject damageUIPrefab;

    private DamageUIContainer damageUIContainer = new DamageUIContainer();
    protected SphereCollider overlappingAvoider; //���� ��ħ ���� �ݶ��̴�
    private BoxCollider boxCollider; //���� �浹ó�� �ݶ��̴�
    private DebuffList debuffList;

    private int returnIndex;

    private float distToPlayer;
    public float DistToPlayer { get => distToPlayer; }
    public DebuffList DebuffList { get => debuffList; set => debuffList = value; }

    public int ReturnIndex { set => returnIndex = value; }
    private IEnumerator Co_StateMachine() //�Ϲ� ���� ���¸ӽ�
    {
        while (!IsDie)
        {
            switch (monsterState)
            {
                case EMonsterState.Chase:
                    animator.SetBool(ConstDefine.BOOL_ISMOVE, Move());
                    break;
                case EMonsterState.Attack:
                    yield return StartCoroutine(Co_Attack());
                    break;
                default:
                    Debug.LogError("��ȿ���� ���� ���� �����Դϴ�.");
                    yield break;
            }
            yield return null;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        debuffList = new DebuffList(this);

        overlappingAvoider = transform.Find("OverlappingAvoider").GetComponent<SphereCollider>();
        boxCollider = GetComponent<BoxCollider>();
    }
    //���͸� ���� ���� �� �� ���� ȣ��
    public void InitDamageUIContainer()
    {
        damageUIContainer.InitDamageUIContainer(transform, 20, damageUIPrefab);
    }
    public void ResetMonster() //���� ���� �� �ʱ�ȭ
    {
        currentHp = maxHp;
        currentSpeed = speed;

        StartCoroutine(Co_StateMachine()); //���� ���� �ӽ� ����
        StartCoroutine(Co_UpdatePositionData()); //���� ��ġ ���� �ڷ�ƾ ����

        IsDie = false; //��ȣ�ۿ� ��ȿ�ϵ��� �ʱ�ȭ
        eCharacterActionable = ECharacterActionable.Actionable;
        overlappingAvoider.enabled = true;
        boxCollider.enabled = true;
    }
    public override void Hit(float damage) //���� �ǰ� �Լ�
    {
        base.Hit(damage);
        damageUIContainer.ActiveDamageUI(damage);
        if (currentHp <= 0) StartCoroutine(Co_DieEvent()); //�׾����� ��� �ڷ�ƾ ����
    }
    public override void KnockBack(float speed, float duration)
    {
        overlappingAvoider.enabled = false; //���� ��ħ���� �ݶ��̴��� ���� (rigidbody.velocity�� 0�̶� �ڿ� �ٸ� ���Ͱ� �ִ� ��� �з����� ����)

        base.KnockBack(speed, duration);
    }
    protected override IEnumerator Co_KnockBack(float speed, float duration)
    {
        rigidbody.velocity = transform.forward * -1 * speed;

        yield return new WaitForSeconds(duration); //���� ó��
        rigidbody.velocity = Vector3.zero;
        overlappingAvoider.enabled = true;

        IsKnockBack = false;
        eCharacterActionable = ECharacterActionable.Actionable; //�ൿ �簳
    }
    protected override IEnumerator Co_KnockBack(float speed, float duration, Vector3 direction)
    {
        rigidbody.velocity = direction * speed;

        yield return new WaitForSeconds(duration); //���� ó��
        rigidbody.velocity = Vector3.zero;
        overlappingAvoider.enabled = true;

        IsKnockBack = false;
        eCharacterActionable = ECharacterActionable.Actionable; //�ൿ �簳
    }
    private IEnumerator Co_UpdatePositionData()
    {
        while(true)
        {
            if (Vector3.Distance(transform.position, InGameManager.Instance.Player.transform.position) > ConstDefine.REPOSITION_DISTANCE) //���� ���ġ
            {
                transform.Translate(Vector3.forward * ConstDefine.REPOSITION_VALUE);
            }
            rigidbody.velocity = Vector3.zero;
            yield return null;
        }
    }
    private IEnumerator Co_DieEvent() //��� ȿ�� �ڷ�ƾ
    {
        IsDie = true; //��ȣ�ۿ� ���� �ʰ� ó��
        overlappingAvoider.enabled = false;
        boxCollider.enabled = false;

        animator.SetTrigger(ConstDefine.TRIGGER_DIE); //��� �ִϸ��̼�
        InGameManager.Instance.Player.IncreaseExp(rewardExp); //��� �ִϸ��̼ǰ� ���� �÷��̾� ����ġ ����
        InGameManager.Instance.KillCount++; //ųī��Ʈ 1 ����

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);//�ִϸ��̼� ��� �ð���ŭ ������

        while (transform.position.y > -1.5) //���Ͱ� �� ������ ������� ȿ��
        {
            transform.position += Vector3.down * 1 * Time.deltaTime;
            yield return null;
        }
        InGameManager.Instance.MonsterPool.ReturnMonster(this, returnIndex); //���� Ǯ�� �ǵ���     
    }
    protected override bool Move()
    {
        if (!base.Move() || IsDie) return false;
        if (Vector3.Distance(InGameManager.Instance.Player.transform.position, transform.position) > attackRange) //���� ��Ÿ����� �̵�
        {
            transform.forward = (InGameManager.Instance.Player.transform.position - transform.position).normalized; //�÷��̾ �ٶ󺸴� ���� ���� ����
            transform.position += transform.forward.normalized * currentSpeed * Time.deltaTime; //�밢�� �̵��� ���� ���� ����ȭ �� �̵��ӵ���ŭ �̵�.
        }
        else
        {
            monsterState = EMonsterState.Attack;
            return false;
        }
        return true;
    }
    protected virtual IEnumerator Co_Attack()
    {
        monsterState = EMonsterState.Chase;
        yield return null;
    }
    /*  -------�浹üũ�� �÷��̾ �ƴ϶� ���Ͱ� �ϴ� ����---------
     *  1. ���͸��� �������� �ٸ�.
     *  2. �÷��̾ �浹üũ�� �ϸ� ������ �������� �˾ƾ� ��.
     *  3. �׷� ���Ϳ� �浹�� ������ �浹ü�� GetComponent�� ���ؼ� �������� �����;���
     *  4. ��������� �浹�� ������ GetComponent�� ȣ���ؾ� �ϴϱ� �ξ� �� ��ȿ�����̶�� ��������.
     *  5. �ٵ� ���� ���� �þ�� �浹 üũ�� ���� ����ŭ �ؾ��ϴϱ� ������� ���ؼ� ���� �� ������ ������ �Ǵ��ϰ� �ٲ�� �� ��
     */
    private void OnTriggerStay(Collider other) //�÷��̾���� �浹 üũ
    {
        if (other.CompareTag(ConstDefine.TAG_PLAYER))
        {
            InGameManager.Instance.Player.Hit(damage);
        }
    }
}
