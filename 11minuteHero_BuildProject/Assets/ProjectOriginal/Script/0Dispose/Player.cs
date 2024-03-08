using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //화면 터치 시 UI체크를 위해 using 선언

public class Player : MonoBehaviour//플레이어 클래스
{
#if UNITY_EDITOR
    private RaycastHit hit; //화면 터치 정보
    private Camera mainCam; //메인카메라
    private Vector3 touchPos; //터치 위치
#endif

    public Animator animator;
    [SerializeField] private Material material; //플레이어 메테리얼
    public bool IsMove { get; set; } //이동중인지 체크

    private bool IsInvincible;//무적인지 체크

    private bool bHitInvincible; //맞아서 되는 무적

    private Vector3 direction = Vector3.zero; //터치 위치

    [SerializeField] private PlayerStats playerStats; //플레이어 능력치

    [SerializeField] private Weapon1 weapon; //무기 참조. 캐릭터별로 무기가 고정이기 때문에 인스펙터에서 할당함

    private MoveUtility moveUtility = new MoveUtility(); //이동 관련 클래스

    private LayerMask layerMask; //화면 터치 시 체크할 레이어마스크

    private SphereCollider itemGainCollider; //아이템 획득 콜라이더

    private float itemGainOriginRadius; //아이템 획득 콜라이더의 반경 초기값

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
        mainCam = FindObjectOfType<CameraFollow>().GetComponent<Camera>(); //카메라 참조
        animator = GetComponent<Animator>();
        playerStats = new PlayerStats(100, 1.5f, 10, 100); //플레이어 능력치 인스턴스화
        hpBar = GameObject.Find(ConstDefine.NAME_PLAYER_UI_HP).GetComponent<BarImageUtility>();
        expBar = GameObject.Find(ConstDefine.NAME_PLAYER_UI_EXP).GetComponent<BarImageUtility>();
        Debug.Log(playerStats.ToString());
        material.color = Color.clear;
    }
    private void Start()
    {
        InGameManager.Instance.DGameOver += StopAllCoroutines; //게임오버 델리게이트에 코루틴 정지 함수 추가

        switch (weapon.WeaponType) //무기타입에 따라 분기 나누기 (애니메이션 재생 여부 결정)
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
    private IEnumerator Co_Move() //터치한 위치로 이동. 이동 중에는 이동 애니메이션 연출.
    {
        while(InGameManager.Instance.GameState != EGameState.GameOver)
        {
            yield return InGameManager.Instance.FrameDelay;
            if (isRolling) yield return new WaitUntil(() => !isRolling);
            animator.SetBool(ConstDefine.BOOL_ISMOVE, IsMove = moveUtility.Move(direction, playerStats.CurrentSpeed));
        }
    }
#if UNITY_EDITOR //에디터 테스트 코드
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
    private void OnDrawGizmos() //에디터 테스트를 위한 기즈모 그리기
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(direction, 0.5f);
    }
    private IEnumerator Co_MeeleeAttack() //근접공격 코루틴
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
    private IEnumerator Co_RangedAttack() //원거리 공격 코루틴
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
    private IEnumerator Co_SetInvincible() //피격시 n초 무적 코루틴.
    {
        float timer = 0;
        Color color = material.color;
        color.a = 0;
        material.color = color;
        for (int i = 0; i < 2; i++) //총 2초동안 플레이어 깜빡거리는 효과
        {
            while (timer < 0.5f)
            {
                timer += Time.deltaTime; //메테리얼 색상 조정
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
        yield return new WaitForSeconds(1f); //1초 지연 후 무적 해제
        bHitInvincible = false;
    }
    public void Hit(float damage) //플레이어 피격 함수
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
    public void Recovery(float value) //수치 적용 타입에 따라 Hp 회복
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
        Debug.Log("구르기 끝");
    }
}
