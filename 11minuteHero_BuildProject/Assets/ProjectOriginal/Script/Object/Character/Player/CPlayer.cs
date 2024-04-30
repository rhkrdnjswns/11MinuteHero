using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CPlayer : Character
{
    private Coroutine invincibilityCoroutine;

    [SerializeField] private Renderer[] rendererArray; //��Ƽ���� �����̴� ȿ���� �ֱ� ���� ����

    private int maxExp = 100; //�䱸 ����ġ
    [SerializeField] private int currentExp; //���� ����ġ
    private int level = 1; //����
    private float dodgeCoolTime;

    [SerializeField] private float currentMaxHp; //���� �ִ� ü��

    [SerializeField] private int expGained; //����ġ ȹ�淮
    [SerializeField] private float damageReduction; //���� ���ҷ�

    [SerializeField] private Transform weaponSocket;
    [SerializeField] private GameObject weaponPrefab;

    private ActiveSkill weapon;

    public bool IsMove { get; set; } //�̵������� üũ
    private bool isInvincible;//�������� üũ
    protected bool isDodge; //������ ���� ��� true

    private Vector3 direction = Vector3.zero; //�÷��̾� �̵� ����

    private SphereCollider itemCollider; //������ ȹ�� �ݶ��̴�
    private float itemGainRadius; //������ ȹ�� �ݶ��̴� �ݰ�

    private WaitForSeconds invincibleDelay = new WaitForSeconds(1f);
    public Vector3 Direction
    {
        set
        {
            direction = value;
            if (direction == Vector3.zero) rigidbody.velocity = Vector3.zero;
        }
    }
    public int ExpGained { get => expGained; set => expGained = value; }
    public float DamageReduction { get => damageReduction; set => damageReduction = value; }
    public float DodgeCoolTime { get => dodgeCoolTime; }
    public bool IsDodge { get => isDodge; }
    public Animator Animator { get => animator; }
    protected abstract IEnumerator Co_Dodge();
    public ActiveSkill Weapon { get => weapon; }
    protected override void Awake() //�÷��̾� ĳ���� ���� �ʱ�ȭ
    {
        base.Awake();

        GameObject obj = Instantiate(weaponPrefab);
        weapon = obj.GetComponent<ActiveSkill>();

        currentMaxHp = maxHp;

        itemCollider = transform.Find(ConstDefine.NAME_ITEMGAINER).GetComponent<SphereCollider>();
        itemGainRadius = itemCollider.radius;

        rendererArray = GetComponentsInChildren<Renderer>();
    }
    protected override void Start()
    {
        base.Start();

        transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * -1; //�������� ���� ����

        InGameManager.Instance.InGameBasicUIManager.PlayerHpBar.SetFillAmount(currentHp);
        InGameManager.Instance.InGameBasicUIManager.PlayerExpBar.SetFillAmount(0, level.ToString());

        StartCoroutine(Co_Move());
    }
#if UNITY_EDITOR
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dodge();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            KnockBack(2f, 0.3f);
        }
        // Debug.DrawRay(transform.position, (transform.forward + transform.right) * 3, Color.red);
    }
#endif
    public override void Hit(float damage) //�ǰ�
    {
        if (isInvincible || isDodge) return;

        base.Hit(damage);
        isInvincible = true;

        StartCoroutine(Co_HitInvincible());
    }
    public virtual bool Dodge() //ȸ��
    {
        if (eCharacterActionable == ECharacterActionable.Unactionable) return false;

        isDodge = true;
        rigidbody.velocity = Vector3.zero;

        StartCoroutine(Co_Dodge());
        return true;
    }
    public void AnimEvent_EndDodge() //ȸ�� ����
    {
        isDodge = false;
        IsMove = false; //ȸ�� ���� ���� ���̽�ƽ �Է��� ��� �̵� �ִϸ��̼��� ����Ǵ� �� ����

        Debug.Log("ȸ�� ����");
    }
    private IEnumerator Co_HitInvincible() //�ǰݽ� n�� ���� �ڷ�ƾ
    {
        float timer = 0;
        Color color;
        for (int i = 0; i < 2; i++) //�� 2�ʵ��� �÷��̾� �����Ÿ��� ȿ��
        {
            while (timer < 0.5f)
            {
                timer += Time.deltaTime; //���׸��� ���� ����
                for(int j = 0; j < rendererArray.Length; j++)
                {
                    color = rendererArray[j].materials[1].color;
                    color.a = timer / 0.7f;
                    rendererArray[j].materials[1].color = color;
                }

                yield return null;
            }
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                for (int j = 0; j < rendererArray.Length; j++)
                {
                    color = rendererArray[j].materials[1].color;
                    color.a = timer / 0.7f;
                    rendererArray[j].materials[1].color = color;
                }
                yield return null;
            }
            yield return null;
        }
        yield return invincibleDelay; //1�� ���� �� ���� ����
        isInvincible = false;
    }
    private IEnumerator Co_Move()
    {
        WaitUntil dodgeEnd = new WaitUntil(() => !isDodge);
        while (true)
        {
            yield return null;
            if (isDodge) yield return dodgeEnd; //ȸ�� ���� ��� ȸ�� ������� ���
            animator.SetBool(ConstDefine.BOOL_ISMOVE, IsMove = Move());
        }
    }

    protected override bool Move()
    {
        if (!base.Move() || direction == Vector3.zero) return false;

        Vector3 camForward = Camera.main.transform.forward; //ī�޶��� ��,��,��,�� ���⺤�͸� ������
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0; //y���� 0���� �������� (�÷��̾� ȸ���� ����)
        camRight.y = 0;

        Vector3 moveDirection = (direction.x * camRight.normalized) + (direction.z * camForward); //�Ű������� ���� ���⺤���� x,y = ���̽�ƽ�� �¿�, �յ� ����ȭ ����
        //ī�޶� �������� �������� �ϱ� ������ ����ī�޶��� ���⺤�͸� ������

        //�÷��̾ �Է¹��� ������ �ٶ󺸵��� ��
        transform.forward = moveDirection;

        //���� ���͸� ����ȭ�Ͽ� �ӵ��� �����ϵ��� ����
        rigidbody.velocity = moveDirection.normalized * currentSpeed;
        //transform.position += moveDirection.normalized * currentSpeed * Time.deltaTime;
        return true;
    }

    protected override void DecreaseHp(float value) //ü�� ���� �Լ�
    {
        currentHp -= value - (value * damageReduction / 100); //���� ���ҷ���ŭ ���ҵ� ���ط� ����
        if (currentHp < 0)
        {
            currentHp = 0;
            InGameManager.Instance.DGameOver(); //����� ��� ���� ���� ���ӿ����� ����
        }
        InGameManager.Instance.InGameBasicUIManager.PlayerHpBar.SetFillAmount(currentHp / currentMaxHp);
    }
    public void IncreaseExp(int value) //����ġ ���� �Լ�
    {
        currentExp += value + (value * expGained / 100); //����ġ ȹ�淮��ŭ ������ ����ġ ����
        if (currentExp >= maxExp)
        {
            level++;
            currentExp -= maxExp;
            maxExp += 100 * (level - 1);
            InGameManager.Instance.DLevelUp();
        }
        if (InGameManager.Instance.InGameBasicUIManager.PlayerExpBar.gameObject.activeSelf)
        {
            InGameManager.Instance.InGameBasicUIManager.PlayerExpBar.SetFillAmount((float)currentExp / (float)maxExp, level.ToString()); //����ġ�� ���� (fillAmount�� 0~1 ���̶� float�� ����ȯ�ؾ� ��)
        }
    }
    public void RecoverHp(float value, EApplicableType type) //ü�� ȸ�� �Լ�
    {
        currentHp += type == EApplicableType.Value ? value : currentMaxHp * value / 100;
        if (currentHp > currentMaxHp) currentHp = currentMaxHp;
        InGameManager.Instance.InGameBasicUIManager.PlayerHpBar.SetFillAmount(currentHp / currentMaxHp);
    }
    public void IncreaseItemColliderRadius(float value)
    {
        itemCollider.radius += itemGainRadius * value / 100;
    }
    public void IncreaseMaxHp(float value)
    {
        currentMaxHp += maxHp * value / 100;
        RecoverHp(maxHp * value / 100, EApplicableType.Value);
        InGameManager.Instance.InGameBasicUIManager.PlayerHpBar.SetFillAmount(currentHp / currentMaxHp);
    }
    public void IncreaseDamage(float value)
    {
        foreach (var item in InGameManager.Instance.SkillManager.InPossessionSkillList)
        {
            if(item.TryGetComponent(out ActiveSkill active))
            {
                active.IncreaseDamage(value);
            }
        }
    }
    public void DecreaseDamage(float value)
    {
        foreach (var item in InGameManager.Instance.SkillManager.InPossessionSkillList)
        {
            if (item.TryGetComponent(out ActiveSkill active))
            {
                active.DecreaseDamage(value);
            }
        }
    }
    public void IncreaseSpeed(float value, EApplicableType type)
    {
        currentSpeed += type == EApplicableType.Value ? value : speed * value / 100;
    }
    public void DecreaseSpeed(float value, EApplicableType type)
    {
        currentSpeed -= type == EApplicableType.Value ? value : speed * value / 100;
    }
    public void ChangeWeapon(ActiveSkill weapon)
    {
        this.weapon.gameObject.SetActive(false);
        this.weapon = weapon;

        weaponSocket.GetChild(0).gameObject.SetActive(false);
        weaponSocket.GetChild(1).gameObject.SetActive(true);
    }
    public override void KnockBack(float speed, float duration) //ĳ���� �޹��������� �˹� �Լ�
    {
        if (isDodge) return;
        base.KnockBack(speed, duration);
    }
    public override void KnockBack(float speed, float duration, Vector3 direction) //������ ���������� �˹� �Լ�
    {
        if (isDodge) return;
        base.KnockBack(speed, duration, direction);
    }
    public void SetInvincibility(float speedValue, float damagePercentage, float duration, EApplicableType speedApplicableType, EApplicableType damageApplicableType)
    {
        if(invincibilityCoroutine != null)
        {
            StopCoroutine(invincibilityCoroutine);

            DecreaseSpeed(speedValue, speedApplicableType);
            DecreaseDamage(damagePercentage);
        }
        invincibilityCoroutine = StartCoroutine(Co_SetInvincibility(speedValue, damagePercentage, duration, speedApplicableType, damageApplicableType));
    }
    private IEnumerator Co_SetInvincibility(float speedValue, float damagePercentage, float duration, EApplicableType speedApplicableType, EApplicableType damageApplicableType)
    {
        isInvincible = true;
        IncreaseSpeed(speedValue, speedApplicableType);
        IncreaseDamage(damagePercentage);

        float timer = 0;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        DecreaseSpeed(speedValue, speedApplicableType);
        DecreaseDamage(damagePercentage);
        isInvincible = false;
    }
}
