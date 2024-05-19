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

    public int mobScale;
    public int id;

    private int expGreenWeight;
    private int expGreenWeightDecrease;
    private int expBlueWeight;
    private int expBlueWeightIncrease;
    private int expBlueWeightDecrease;
    private int expRedWeight;
    private int expRedWeightIncrease;
    private int expPurpleWeight;
    private int expPurpleWeightIncrease;

    private float currentDamage;

    private float hpIncrease;
    private float damageIncrease;
    private float speedIncrease;

    private Coroutine stiffnessCoroutine;
    private Coroutine slowDownCoroutine;

    private float currentSlowDownValue;

    [SerializeField] protected EMonsterState monsterState;
    [SerializeField] protected float damage; //������

    [SerializeField] private float attackRange;

    protected SphereCollider overlappingAvoider; //���� ��ħ ���� �ݶ��̴�
    private BoxCollider boxCollider; //���� �浹ó�� �ݶ��̴�

    private int returnIndex;

    private float distToPlayer;

    private WaitForSeconds dieAnimDelay;

    private bool isStiffness;
    public float DistToPlayer { get => distToPlayer; }
    public int ReturnIndex { set => returnIndex = value; }
    protected override void Start()
    {
        base.Start();

        InGameManager.Instance.DGameOver += () => overlappingAvoider.enabled = false;
    }
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
    public override void InitMonsterData()
    {
        base.InitMonsterData();

        overlappingAvoider = transform.Find("OverlappingAvoider").GetComponent<SphereCollider>();
        boxCollider = GetComponent<BoxCollider>();
        ReadCSVData();
    }
    protected virtual void ReadCSVData()
    {
        expGreenWeight = InGameManager.Instance.CSVManager.GetCSVData<int>(4, mobScale + 1 , 1);
        expGreenWeightDecrease = InGameManager.Instance.CSVManager.GetCSVData<int>(4, mobScale + 1, 2);

        expBlueWeight = InGameManager.Instance.CSVManager.GetCSVData<int>(4, mobScale + 1, 3);
        expBlueWeightIncrease = InGameManager.Instance.CSVManager.GetCSVData<int>(4, mobScale + 1, 4);
        expBlueWeightDecrease = InGameManager.Instance.CSVManager.GetCSVData<int>(4, mobScale + 1, 5);
        
        expRedWeight = InGameManager.Instance.CSVManager.GetCSVData<int>(4, mobScale + 1, 6);
        expRedWeightIncrease = InGameManager.Instance.CSVManager.GetCSVData<int>(4, mobScale + 1, 7);
        
        expPurpleWeight = InGameManager.Instance.CSVManager.GetCSVData<int>(4, mobScale + 1, 8);
        expPurpleWeightIncrease = InGameManager.Instance.CSVManager.GetCSVData<int>(4, mobScale + 1, 9);

        attackRange = InGameManager.Instance.CSVManager.GetCSVData<float>(5, id, 11);
        
        maxHp = InGameManager.Instance.CSVManager.GetCSVData<float>(5, id, 12);
        currentHp = maxHp;
        hpIncrease = InGameManager.Instance.CSVManager.GetCSVData<float>(5, id, 13);

        damage = InGameManager.Instance.CSVManager.GetCSVData<float>(5, id, 14);
        currentDamage = damage;
        damageIncrease = InGameManager.Instance.CSVManager.GetCSVData<float>(5, id, 15);

        speed = InGameManager.Instance.CSVManager.GetCSVData<float>(5, id, 16);
        speedIncrease = InGameManager.Instance.CSVManager.GetCSVData<float>(5, id, 17);
    }
    public void ResetMonster() //���� ���� �� �ʱ�ȭ
    {
        IsDie = false; //��ȣ�ۿ� ��ȿ�ϵ��� �ʱ�ȭ
        eCharacterActionable = ECharacterActionable.Actionable;
        overlappingAvoider.enabled = true;
        boxCollider.enabled = true;

        currentHp = maxHp + (hpIncrease * (InGameManager.Instance.Timer / 60));
        currentDamage = damage + (damageIncrease * (InGameManager.Instance.Timer / 60));
        currentSpeed = speed + (speedIncrease * (InGameManager.Instance.Timer / 60)); ;

        StartCoroutine(Co_StateMachine()); //���� ���� �ӽ� ����
        StartCoroutine(Co_UpdatePositionData()); //���� ��ġ ���� �ڷ�ƾ ����
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
        animator.SetBool(ConstDefine.BOOL_ISMOVE, false);
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
            InGameManager.Instance.Player.Hit(currentDamage);
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
        if (value >= currentSlowDownValue)
        {
            if (slowDownCoroutine != null)
            {
                StopCoroutine(slowDownCoroutine);
                currentSpeed += currentSlowDownValue;
            }
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
        currentSlowDownValue = 0;
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
        if(InGameManager.Instance.killCountForItem >= 100)
        {
            InGameManager.Instance.killCountForItem = 0;
            int rand = Random.Range(4, 8);
            return (EItemID)rand;
        }

        int green = expGreenWeight - (expGreenWeightDecrease * ((int)InGameManager.Instance.Timer / 60));
        int blue = expBlueWeight + (expBlueWeightIncrease * ((int)InGameManager.Instance.Timer / 60)) - (expBlueWeightDecrease * ((int)InGameManager.Instance.Timer / 60));
        int red = expRedWeight + (expRedWeightIncrease * ((int)InGameManager.Instance.Timer / 60)); 
        int purple = expPurpleWeight + (expPurpleWeightIncrease * ((int)InGameManager.Instance.Timer / 60));

        int[] array = { green, blue, red, purple }; 

        int total = green + blue + red + purple;
        int weight = Random.Range(1, total + 1);

        int sum = 0;

        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
            if(weight < sum)
            {
                return (EItemID)i;
            }
        }

        return EItemID.ExpGreen;
    }
}
