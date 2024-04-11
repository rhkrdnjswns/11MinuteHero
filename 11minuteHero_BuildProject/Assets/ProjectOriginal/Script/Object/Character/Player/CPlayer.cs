using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CPlayer : Character
{
    //[SerializeField] private Transform rendererBody;
    [SerializeField] private Renderer[] rendererArray; //��Ƽ���� �����̴� ȿ���� �ֱ� ���� ����

    private int maxExp = 100; //�䱸 ����ġ
    private int currentExp; //���� ����ġ
    private int level = 1; //����

    [SerializeField] private float currentMaxHp; //���� �ִ� ü��

    private int expGained; //����ġ ȹ�淮
    private float damageReduction; //���� ���ҷ�

    [SerializeField] private GameObject weaponPrefab;
    protected AWeapon weapon;

    [SerializeField] private Transform weaponSocket;
    public bool IsMove { get; set; } //�̵������� üũ
    private bool isInvincible;//�������� üũ
    protected bool isDodge; //������ ���� ��� true

    private Vector3 direction = Vector3.zero; //�÷��̾� �̵� ����

    private SphereCollider itemCollider; //������ ȹ�� �ݶ��̴�
    private float itemGainRadius; //������ ȹ�� �ݶ��̴� �ݰ�

    private BarImageUtility hpBar;  //�÷��̾� ü�¹�
    private BarImageUtility expBar; //�÷��̾� ����ġ��

    private IEnumerator attackCoroutine;
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
    public AWeapon Weapon { get => weapon; }
    protected abstract IEnumerator Co_Attack();
    protected abstract IEnumerator Co_Dodge();

    protected override void Awake() //�÷��̾� ĳ���� ���� �ʱ�ȭ
    {
        base.Awake();

        currentMaxHp = maxHp;

        itemCollider = transform.Find(ConstDefine.NAME_ITEMGAINER).GetComponent<SphereCollider>();
        itemGainRadius = itemCollider.radius;

        hpBar = GameObject.Find(ConstDefine.NAME_PLAYER_UI_HP).GetComponent<BarImageUtility>();
        expBar = GameObject.Find(ConstDefine.NAME_PLAYER_UI_EXP).GetComponent<BarImageUtility>();

        GameObject obj = Instantiate(weaponPrefab);
        weapon = obj.GetComponent<AWeapon>();
        rendererArray = GetComponentsInChildren<Renderer>();
    }
    private void Start()
    {
        weapon.InitSkill();

        transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * -1; //�������� ���� ����

        hpBar.SetFillAmount(currentHp);
        expBar.SetFillAmount(0, level.ToString());

        StartCoroutine(Co_Move());
        attackCoroutine = Co_Attack();
        StartCoroutine(attackCoroutine);
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
        for (int i = 0; i < 2; i++) //�� 2�ʵ��� �÷��̾� �����Ÿ��� ȿ��
        {
            Color color;
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
        yield return new WaitForSeconds(1f); //1�� ���� �� ���� ����
        isInvincible = false;
    }
    private IEnumerator Co_Move()
    {
        while (InGameManager.Instance.GameState != EGameState.GameOver)
        {
            yield return null;
            if (isDodge) yield return new WaitUntil(() => !isDodge); //ȸ�� ���� ��� ȸ�� ������� ���
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
            InGameManager.Instance.GameState = EGameState.GameOver; //����� ��� ���� ���� ���ӿ����� ����
        }
        hpBar.SetFillAmount(currentHp / currentMaxHp);
    }
    public void IncreaseExp(int value) //����ġ ���� �Լ�
    {
        currentExp += value + (value * expGained / 100); //����ġ ȹ�淮��ŭ ������ ����ġ ����
        if (currentExp >= maxExp)
        {
            level++;
            currentExp -= maxExp;
            maxExp += 100 * (level - 1);
        }
        if (expBar.gameObject.activeSelf)
        {
            expBar.SetFillAmount((float)currentExp / (float)maxExp, level.ToString()); //����ġ�� ���� (fillAmount�� 0~1 ���̶� float�� ����ȯ�ؾ� ��)
        }
    }
    public void RecoverHp(float value, EApplicableType type) //ü�� ȸ�� �Լ�
    {
        currentHp += type == EApplicableType.Value ? value : currentMaxHp * value / 100;
        if (currentHp > currentMaxHp) currentHp = currentMaxHp;
        hpBar.SetFillAmount(currentHp / currentMaxHp);
    }
    public void InActiveExpBar()
    {
        expBar.transform.parent.gameObject.SetActive(false);
    }
    public void IncreaseItemColliderRadius(float value)
    {
        itemCollider.radius += itemGainRadius * value / 100;
    }
    public void IncreaseMaxHp(float value)
    {
        currentMaxHp += maxHp * value / 100;
        RecoverHp(maxHp * value / 100, EApplicableType.Value);
        hpBar.SetFillAmount(currentHp / currentMaxHp);
    }
    public void IncreaseSpeed(float value, EApplicableType type)
    {
        currentSpeed += type == EApplicableType.Value ? value : speed * value / 100;
    }
    public void DecreaseSpeed(float value)
    {
        currentSpeed -= value;
    }
    public void ChangeWeapon(AWeapon weapon)
    {
        StopCoroutine(attackCoroutine);
        InGameManager.Instance.SkillManager.ActiveSkillList.Remove(this.weapon);
        Destroy(this.weapon.gameObject);
        this.weapon = weapon;

        weaponSocket.GetChild(0).gameObject.SetActive(false);
        weaponSocket.GetChild(1).gameObject.SetActive(true);
        attackCoroutine = Co_Attack();
        StartCoroutine(attackCoroutine);
    }
    public override void KnockBack(float speed, float duration) //ĳ���� �޹��������� �˹� �Լ�
    {
        if (isDodge) return;
        base.KnockBack(speed, duration);
        //rigidbody.velocity = Vector3.zero;
    }
    public override void KnockBack(float speed, float duration, Vector3 direction) //������ ���������� �˹� �Լ�
    {
        if (isDodge) return;
        base.KnockBack(speed, duration, direction);
        //rigidbody.velocity = Vector3.zero;
    }
}
