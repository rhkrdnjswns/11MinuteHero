using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CPlayer : Character
{
    private Coroutine invincibilityCoroutine;

    [SerializeField] private Renderer[] rendererArray; //머티리얼 깜빡이는 효과를 주기 위한 참조

    private int maxExp = 100; //요구 경험치
    [SerializeField] private int currentExp; //현재 경험치
    private int level = 1; //레벨
    private float dodgeCoolTime;

    [SerializeField] private float currentMaxHp; //현재 최대 체력

    [SerializeField] private int expGained; //경험치 획득량
    [SerializeField] private float damageReduction; //피해 감소량

    [SerializeField] private Transform weaponSocket;
    [SerializeField] private GameObject weaponPrefab;

    private ActiveSkill weapon;

    public bool IsMove { get; set; } //이동중인지 체크
    private bool isInvincible;//무적인지 체크
    protected bool isDodge; //구르기 중인 경우 true

    private Vector3 direction = Vector3.zero; //플레이어 이동 방향

    private SphereCollider itemCollider; //아이템 획득 콜라이더
    private float itemGainRadius; //아이템 획득 콜라이더 반경

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
    protected override void Awake() //플레이어 캐릭터 관련 초기화
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

        transform.forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * -1; //정면으로 시점 변경

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
    public override void Hit(float damage) //피격
    {
        if (isInvincible || isDodge) return;

        base.Hit(damage);
        isInvincible = true;

        StartCoroutine(Co_HitInvincible());
    }
    public virtual bool Dodge() //회피
    {
        if (eCharacterActionable == ECharacterActionable.Unactionable) return false;

        isDodge = true;
        rigidbody.velocity = Vector3.zero;

        StartCoroutine(Co_Dodge());
        return true;
    }
    public void AnimEvent_EndDodge() //회피 종료
    {
        isDodge = false;
        IsMove = false; //회피 종료 직후 조이스틱 입력이 없어도 이동 애니메이션이 재생되는 것 방지

        Debug.Log("회피 종료");
    }
    private IEnumerator Co_HitInvincible() //피격시 n초 무적 코루틴
    {
        float timer = 0;
        Color color;
        for (int i = 0; i < 2; i++) //총 2초동안 플레이어 깜빡거리는 효과
        {
            while (timer < 0.5f)
            {
                timer += Time.deltaTime; //메테리얼 색상 조정
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
        yield return invincibleDelay; //1초 지연 후 무적 해제
        isInvincible = false;
    }
    private IEnumerator Co_Move()
    {
        WaitUntil dodgeEnd = new WaitUntil(() => !isDodge);
        while (true)
        {
            yield return null;
            if (isDodge) yield return dodgeEnd; //회피 중인 경우 회피 종료까지 대기
            animator.SetBool(ConstDefine.BOOL_ISMOVE, IsMove = Move());
        }
    }

    protected override bool Move()
    {
        if (!base.Move() || direction == Vector3.zero) return false;

        Vector3 camForward = Camera.main.transform.forward; //카메라의 앞,뒤,좌,우 방향벡터를 가져옴
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0; //y값을 0으로 고정해줌 (플레이어 회전축 고정)
        camRight.y = 0;

        Vector3 moveDirection = (direction.x * camRight.normalized) + (direction.z * camForward); //매개변수로 받은 방향벡터의 x,y = 조이스틱의 좌우, 앞뒤 정규화 벡터
        //카메라를 기준으로 움직여야 하기 때문에 메인카메라의 방향벡터를 곱해줌

        //플레이어가 입력받은 방향을 바라보도록 함
        transform.forward = moveDirection;

        //방향 벡터를 정규화하여 속도가 일정하도록 해줌
        rigidbody.velocity = moveDirection.normalized * currentSpeed;
        //transform.position += moveDirection.normalized * currentSpeed * Time.deltaTime;
        return true;
    }

    protected override void DecreaseHp(float value) //체력 감소 함수
    {
        currentHp -= value - (value * damageReduction / 100); //피해 감소량만큼 감소된 피해량 연산
        if (currentHp < 0)
        {
            currentHp = 0;
            InGameManager.Instance.DGameOver(); //사망한 경우 게임 상태 게임오버로 변경
        }
        InGameManager.Instance.InGameBasicUIManager.PlayerHpBar.SetFillAmount(currentHp / currentMaxHp);
    }
    public void IncreaseExp(int value) //경험치 증가 함수
    {
        currentExp += value + (value * expGained / 100); //경험치 획득량만큼 증가된 경험치 연산
        if (currentExp >= maxExp)
        {
            level++;
            currentExp -= maxExp;
            maxExp += 100 * (level - 1);
            InGameManager.Instance.DLevelUp();
        }
        if (InGameManager.Instance.InGameBasicUIManager.PlayerExpBar.gameObject.activeSelf)
        {
            InGameManager.Instance.InGameBasicUIManager.PlayerExpBar.SetFillAmount((float)currentExp / (float)maxExp, level.ToString()); //경험치바 갱신 (fillAmount가 0~1 값이라 float로 형변환해야 함)
        }
    }
    public void RecoverHp(float value, EApplicableType type) //체력 회복 함수
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
    public override void KnockBack(float speed, float duration) //캐릭터 뒷방향으로의 넉백 함수
    {
        if (isDodge) return;
        base.KnockBack(speed, duration);
    }
    public override void KnockBack(float speed, float duration, Vector3 direction) //임의의 방향으로의 넉백 함수
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
