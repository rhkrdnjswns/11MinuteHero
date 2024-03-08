using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //ȭ�� ��ġ �� UIüũ�� ���� using ����

public class Player : MonoBehaviour//�÷��̾� Ŭ����
{
#if UNITY_EDITOR
    private RaycastHit hit; //ȭ�� ��ġ ����
    private Camera mainCam; //����ī�޶�
    private Vector3 touchPos; //��ġ ��ġ
#endif

    public Animator animator;
    [SerializeField] private Material material; //�÷��̾� ���׸���
    public bool IsMove { get; set; } //�̵������� üũ

    private bool IsInvincible;//�������� üũ

    private bool bHitInvincible; //�¾Ƽ� �Ǵ� ����

    private Vector3 direction = Vector3.zero; //��ġ ��ġ

    [SerializeField] private PlayerStats playerStats; //�÷��̾� �ɷ�ġ

    [SerializeField] private Weapon1 weapon; //���� ����. ĳ���ͺ��� ���Ⱑ �����̱� ������ �ν����Ϳ��� �Ҵ���

    private MoveUtility moveUtility = new MoveUtility(); //�̵� ���� Ŭ����

    private LayerMask layerMask; //ȭ�� ��ġ �� üũ�� ���̾��ũ

    private SphereCollider itemGainCollider; //������ ȹ�� �ݶ��̴�

    private float itemGainOriginRadius; //������ ȹ�� �ݶ��̴��� �ݰ� �ʱⰪ

    private BarImageUtility hpBar;

    private BarImageUtility expBar;

    private bool isRolling;

    public Weapon1 Weapon { get => weapon; }

    public PlayerStats PlayerStats { get => playerStats; }

    public SphereCollider ItemGainCollider { get => itemGainCollider; set => itemGainCollider = value; }
    public float ItemGainOriginRadius { get => itemGainOriginRadius; }
    public MoveUtility MoveUtility { get => moveUtility; }
    public Vector3 Direction { set => direction = value; }

    private void Awake()
    {
        layerMask = LayerMask.GetMask(ConstDefine.TAG_FLOOR);
        itemGainCollider = transform.Find(ConstDefine.NAME_ITEMGAINER).GetComponent<SphereCollider>();
        itemGainOriginRadius = itemGainCollider.radius;
        mainCam = FindObjectOfType<CameraFollow>().GetComponent<Camera>(); //ī�޶� ����
        animator = GetComponent<Animator>();
        playerStats = new PlayerStats(100, 1.5f, 10, 100); //�÷��̾� �ɷ�ġ �ν��Ͻ�ȭ
        hpBar = GameObject.Find(ConstDefine.NAME_PLAYER_UI_HP).GetComponent<BarImageUtility>();
        expBar = GameObject.Find(ConstDefine.NAME_PLAYER_UI_EXP).GetComponent<BarImageUtility>();
        Debug.Log(playerStats.ToString());
        material.color = Color.clear;
    }
    private void Start()
    {
        InGameManager.Instance.DGameOver += StopAllCoroutines; //���ӿ��� ��������Ʈ�� �ڷ�ƾ ���� �Լ� �߰�

        switch (weapon.WeaponType) //����Ÿ�Կ� ���� �б� ������ (�ִϸ��̼� ��� ���� ����)
        {
            case Weapon1.eWeaponType.Range:
                StartCoroutine(Co_RangedAttack());
                break;
            case Weapon1.eWeaponType.Melee:
                StartCoroutine(Co_MeeleeAttack());
                break;
            default:
                Debug.LogError("Unsigned Weapon Type");
                break;
        }
        weapon.InitWeapon();
        transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * -1;

        hpBar.SetFillAmount(playerStats.Hp);
        expBar.SetFillAmount(0, playerStats.Level.ToString());
        StartCoroutine(Co_Move());
    }
    private IEnumerator Co_Move() //��ġ�� ��ġ�� �̵�. �̵� �߿��� �̵� �ִϸ��̼� ����.
    {
        while(InGameManager.Instance.GameState != EGameState.GameOver)
        {
            yield return InGameManager.Instance.FrameDelay;
            if (isRolling) yield return new WaitUntil(() => !isRolling);
            animator.SetBool(ConstDefine.BOOL_ISMOVE, IsMove = moveUtility.Move(direction, playerStats.CurrentSpeed));
        }
    }
#if UNITY_EDITOR //������ �׽�Ʈ �ڵ�
    public void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
        //    {
        //        if (EventSystem.current.IsPointerOverGameObject()) return;
        //        touchPos = new Vector3(hit.point.x, 0, hit.point.z);
        //    }
        //}
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Rolling();
        }
    }
    public void FixedUpdate()
    {
        //animator.SetBool(ConstDefine.BOOL_ISMOVE, IsMove = moveUtility.Move(transform, touchPos, 0.1f, playerStats.CurrentSpeed));
    }
#endif
    private void OnDrawGizmos() //������ �׽�Ʈ�� ���� ����� �׸���
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(direction, 0.5f);
    }
    private IEnumerator Co_MeeleeAttack() //�������� �ڷ�ƾ
    {
        float timer = 0;
        while (true)
        {
            timer = 0;
            while (timer < weapon.CoolTime)
            {
                timer += Time.deltaTime;
                yield return InGameManager.Instance.FrameDelay;
            }
            if (isRolling) yield return new WaitUntil(() => !isRolling);
            animator.SetTrigger(ConstDefine.TRIGGER_MELEE_ATTACK);
            weapon.Attack();
            yield return InGameManager.Instance.FrameDelay;
        }
    }
    private IEnumerator Co_RangedAttack() //���Ÿ� ���� �ڷ�ƾ
    {
        float timer = 0;
        while (true)
        {
            timer = 0;
            while (timer < weapon.CoolTime)
            {
                timer += Time.deltaTime;
                yield return InGameManager.Instance.FrameDelay;
            }
            if (isRolling) yield return new WaitUntil(() => !isRolling);
            weapon.Attack();
            yield return InGameManager.Instance.FrameDelay;
        }
    }
    private IEnumerator Co_SetInvincible() //�ǰݽ� n�� ���� �ڷ�ƾ.
    {
        float timer = 0;
        Color color = material.color;
        color.a = 0;
        material.color = color;
        for (int i = 0; i < 2; i++) //�� 2�ʵ��� �÷��̾� �����Ÿ��� ȿ��
        {
            while (timer < 0.5f)
            {
                timer += Time.deltaTime; //���׸��� ���� ����
                color.a = timer / 0.7f;
                material.color = color;
                yield return null;
            }
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                color.a = timer / 0.7f;
                material.color = color;
                yield return null;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f); //1�� ���� �� ���� ����
        bHitInvincible = false;
    }
    public void Hit(float damage) //�÷��̾� �ǰ� �Լ�
    {
        if (IsInvincible || bHitInvincible) return;
        playerStats.DecreaseHp(damage);
        hpBar.SetFillAmount(playerStats.Hp / playerStats.CurrentMaxHp);
        bHitInvincible = true;
        StartCoroutine(Co_SetInvincible());
    }
    public void SetExp(int value)
    {
        float exp = playerStats.IncreaseExp(value);
        expBar.SetFillAmount(exp, playerStats.Level.ToString());
    }
    public void Recovery(float value) //��ġ ���� Ÿ�Կ� ���� Hp ȸ��
    {
        playerStats.Recovery(value);
        hpBar.SetFillAmount(playerStats.Hp / playerStats.CurrentMaxHp);
    }
    public void Recovery(float value, EApplicableType type)
    {
        playerStats.Recovery(value, type);
        hpBar.SetFillAmount(playerStats.Hp / playerStats.CurrentMaxHp);
    }
    public void Rolling()
    {
        IsInvincible = true;
        isRolling = true;
        StartCoroutine(Co_Rolling());
    }
    private IEnumerator Co_Rolling()
    {
        animator.SetTrigger(ConstDefine.TRIGGER_Dodge);
        Vector3 dir = transform.forward;
        dir.y = 0;
        yield return new WaitForSeconds(0.25f);
        while (isRolling)
        {
            yield return InGameManager.Instance.FrameDelay;
            transform.position += dir * playerStats.CurrentSpeed * 2.8f * Time.deltaTime;
        }
    }
    public void EndRolling()
    {
        IsInvincible = false;
        isRolling = false;
        IsMove = false;
        Debug.Log("������ ��");
    }
}
