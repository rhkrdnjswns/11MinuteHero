using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// * �Ϲ� ���� AI�� ���¸ӽ����� ������
public class NormalMonster : Monster, IDebuffApplicable
{
    public enum EMonsterState
    {
        Chase = 0,
        Attack
    }

    private Coroutine stiffnessCoroutine;
    private Coroutine slowDownCoroutine;

    private float currentSlowDownValue;

    [SerializeField] protected EMonsterState monsterState;
    [SerializeField] protected float damage; //������

    [SerializeField] private float attackRange;

    protected SphereCollider overlappingAvoider; //���� ��ħ ���� �ݶ��̴�
    private BoxCollider boxCollider; //���� �浹ó�� �ݶ��̴�
    private DebuffList debuffList;

    private int returnIndex;

    private float distToPlayer;

    private WaitForSeconds dieAnimDelay;

    private bool isStiffness;
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
    protected override void Start()
    {
        base.Start();
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

        float timer = 0;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        rigidbody.velocity = Vector3.zero;
        overlappingAvoider.enabled = true;

        IsKnockBack = false;
        eCharacterActionable = ECharacterActionable.Actionable; //�ൿ �簳
    }
    protected override IEnumerator Co_KnockBack(float speed, float duration, Vector3 direction)
    {
        rigidbody.velocity = direction * speed;

        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
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

        if (!animator.enabled) animator.enabled = true;
        animator.SetTrigger(ConstDefine.TRIGGER_DIE); //��� �ִϸ��̼�
        InGameManager.Instance.KillCount++; //ųī��Ʈ 1 ����

        InGameManager.Instance.ItemManager.GetItem(transform.position, GetItemIDByWeight());

        while (transform.position.y > -1.5) //���Ͱ� �� ������ ������� ȿ��
        {
            transform.position += Vector3.down * 1 * Time.deltaTime;
            yield return null;
        }

        if (dieAnimDelay == null) dieAnimDelay = new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return dieAnimDelay;//�ִϸ��̼� ��� �ð���ŭ ������
        InGameManager.Instance.MonsterPool.ReturnMonster(this, returnIndex); //���� Ǯ�� �ǵ���   

        InGameManager.Instance.MonsterList.Remove(this);
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
        if (isStiffness) return;
        if (other.CompareTag(ConstDefine.TAG_PLAYER))
        {
            InGameManager.Instance.Player.Hit(damage);
        }
    }
    public void Stun(float duration)
    {
        SetStiffness(duration);
    }
    public void SlowDown(float value, float duration)
    {
        SetSlowDonw(value, duration);
    }
    private void SetSlowDonw(float value, float duration)
    {
        if(slowDownCoroutine != null)
        {
            currentSpeed += currentSlowDownValue;
            StopCoroutine(slowDownCoroutine);
        }

        if (value >= currentSlowDownValue)
        {
            currentSlowDownValue = value;
            currentSpeed -= value;
            slowDownCoroutine = StartCoroutine(Co_SlowDown(duration));
        }
    }
    private IEnumerator Co_SlowDown(float duration)
    {
        float timer = 0;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        currentSpeed += currentSlowDownValue;
    }
    public void SetStiffness(float sec)
    {
        isStiffness = true;
        eCharacterActionable = ECharacterActionable.Unactionable;
        animator.enabled = false;

        if(stiffnessCoroutine != null)
        {
            StopCoroutine(stiffnessCoroutine);
        }
        stiffnessCoroutine = StartCoroutine(Co_Stiffness(sec));
    }
    private IEnumerator Co_Stiffness(float sec)
    {
        float timer = 0;
        while(timer < sec)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        animator.enabled = true;
        isStiffness = false;
        eCharacterActionable = ECharacterActionable.Actionable;
    }
    private EItemID GetItemIDByWeight()
    {
        return EItemID.ExpRed;
    }
}
