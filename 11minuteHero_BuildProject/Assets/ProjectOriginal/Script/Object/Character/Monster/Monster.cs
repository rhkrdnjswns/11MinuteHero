using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// * 일반 몬스터 AI는 상태머신으로 디자인
public class Monster : Character
{
    public enum EMonsterState
    {
        Chase = 0,
        Attack
    }

    [SerializeField] protected EMonsterState monsterState;
    [SerializeField] protected float damage; //데미지

    [SerializeField] private int rewardExp;
    [SerializeField] private float attackRange;

    [SerializeField] private GameObject damageUIPrefab;

    private DamageUIContainer damageUIContainer = new DamageUIContainer();
    protected SphereCollider overlappingAvoider; //몬스터 겹침 방지 콜라이더
    private BoxCollider boxCollider; //몬스터 충돌처리 콜라이더
    private DebuffList debuffList;

    private int returnIndex;

    private float distToPlayer;
    public float DistToPlayer { get => distToPlayer; }
    public DebuffList DebuffList { get => debuffList; set => debuffList = value; }

    public int ReturnIndex { set => returnIndex = value; }
    private IEnumerator Co_StateMachine() //일반 몬스터 상태머신
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
                    Debug.LogError("유효하지 않은 몬스터 상태입니다.");
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
    //몬스터를 새로 생성 시 한 번만 호출
    public void InitDamageUIContainer()
    {
        damageUIContainer.InitDamageUIContainer(transform, 20, damageUIPrefab);
    }
    public void ResetMonster() //몬스터 생성 시 초기화
    {
        currentHp = maxHp;
        currentSpeed = speed;

        StartCoroutine(Co_StateMachine()); //몬스터 상태 머신 실행
        StartCoroutine(Co_UpdatePositionData()); //몬스터 위치 관련 코루틴 실행

        IsDie = false; //상호작용 유효하도록 초기화
        eCharacterActionable = ECharacterActionable.Actionable;
        overlappingAvoider.enabled = true;
        boxCollider.enabled = true;
    }
    public override void Hit(float damage) //몬스터 피격 함수
    {
        base.Hit(damage);
        damageUIContainer.ActiveDamageUI(damage);
        if (currentHp <= 0) StartCoroutine(Co_DieEvent()); //죽었으면 사망 코루틴 실행
    }
    public override void KnockBack(float speed, float duration)
    {
        overlappingAvoider.enabled = false; //몬스터 겹침방지 콜라이더를 꺼줌 (rigidbody.velocity가 0이라 뒤에 다른 몬스터가 있는 경우 밀려나지 않음)

        base.KnockBack(speed, duration);
    }
    protected override IEnumerator Co_KnockBack(float speed, float duration)
    {
        rigidbody.velocity = transform.forward * -1 * speed;

        yield return new WaitForSeconds(duration); //경직 처리
        rigidbody.velocity = Vector3.zero;
        overlappingAvoider.enabled = true;

        IsKnockBack = false;
        eCharacterActionable = ECharacterActionable.Actionable; //행동 재개
    }
    protected override IEnumerator Co_KnockBack(float speed, float duration, Vector3 direction)
    {
        rigidbody.velocity = direction * speed;

        yield return new WaitForSeconds(duration); //경직 처리
        rigidbody.velocity = Vector3.zero;
        overlappingAvoider.enabled = true;

        IsKnockBack = false;
        eCharacterActionable = ECharacterActionable.Actionable; //행동 재개
    }
    private IEnumerator Co_UpdatePositionData()
    {
        while(true)
        {
            if (Vector3.Distance(transform.position, InGameManager.Instance.Player.transform.position) > ConstDefine.REPOSITION_DISTANCE) //몬스터 재배치
            {
                transform.Translate(Vector3.forward * ConstDefine.REPOSITION_VALUE);
            }
            rigidbody.velocity = Vector3.zero;
            yield return null;
        }
    }
    private IEnumerator Co_DieEvent() //사망 효과 코루틴
    {
        IsDie = true; //상호작용 하지 않게 처리
        overlappingAvoider.enabled = false;
        boxCollider.enabled = false;

        animator.SetTrigger(ConstDefine.TRIGGER_DIE); //사망 애니메이션
        InGameManager.Instance.Player.IncreaseExp(rewardExp); //사망 애니메이션과 같이 플레이어 경험치 증가
        InGameManager.Instance.KillCount++; //킬카운트 1 증가

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);//애니메이션 재생 시간만큼 딜레이

        while (transform.position.y > -1.5) //몬스터가 땅 밑으로 사라지는 효과
        {
            transform.position += Vector3.down * 1 * Time.deltaTime;
            yield return null;
        }
        InGameManager.Instance.MonsterPool.ReturnMonster(this, returnIndex); //몬스터 풀로 되돌림     
    }
    protected override bool Move()
    {
        if (!base.Move() || IsDie) return false;
        if (Vector3.Distance(InGameManager.Instance.Player.transform.position, transform.position) > attackRange) //공격 사거리까지 이동
        {
            transform.forward = (InGameManager.Instance.Player.transform.position - transform.position).normalized; //플레이어를 바라보는 방향 벡터 구함
            transform.position += transform.forward.normalized * currentSpeed * Time.deltaTime; //대각선 이동을 위한 벡터 정규화 및 이동속도만큼 이동.
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
    /*  -------충돌체크를 플레이어가 아니라 몬스터가 하는 이유---------
     *  1. 몬스터마다 데미지가 다름.
     *  2. 플레이어가 충돌체크를 하면 몬스터의 데미지를 알아야 함.
     *  3. 그럼 몬스터와 충돌할 때마다 충돌체에 GetComponent를 통해서 데미지를 가져와야함
     *  4. 결과적으로 충돌할 때마다 GetComponent를 호출해야 하니깐 훨씬 더 비효율적이라고 생각했음.
     *  5. 근데 몬스터 수가 늘어나면 충돌 체크를 몬스터 수만큼 해야하니깐 디버깅을 통해서 뭐가 더 성능이 나은지 판단하고 바꿔야 할 듯
     */
    private void OnTriggerStay(Collider other) //플레이어와의 충돌 체크
    {
        if (other.CompareTag(ConstDefine.TAG_PLAYER))
        {
            InGameManager.Instance.Player.Hit(damage);
        }
    }
}
