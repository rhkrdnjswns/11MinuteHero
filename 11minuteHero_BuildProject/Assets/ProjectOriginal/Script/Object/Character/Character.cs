using System.Collections;
using UnityEngine;

public enum EApplicableType //수치 적용 방식
{
    Value,
    Percentage
}
public enum ECharacterActionable //캐릭터가 행동 가능한 상태인지 구분
{
    Actionable,
    Unactionable
}
public abstract class Character : MonoBehaviour //언리얼의 Character와 같은 역할. 플레이어, 몬스터의 부모 클래스
{
    [SerializeField] protected float maxHp; //최대 체력
    [SerializeField] protected float speed; //이동속도

    [SerializeField] protected float currentHp; //현재 체력
    protected float currentSpeed; //현재 이동속도

    protected Animator animator; //애니메이터 컴포넌트 참조

    protected Rigidbody rigidbody; //리지드바디 컴포넌트 참조

    protected ECharacterActionable eCharacterActionable = ECharacterActionable.Actionable;

    public bool IsDie { get; set; }
    public bool IsKnockBack { get; set; }

    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float Speed { get => speed; set => speed = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
    public ECharacterActionable ECharacterActionable { get => eCharacterActionable; set => eCharacterActionable = value; }

    protected virtual void Awake() //초기화
    {
        currentHp = maxHp;
        currentSpeed = speed;

        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }
    protected virtual void DecreaseHp(float value) //체력 감소
    {
        currentHp -= value;
        if (currentHp < 0) //체력이 0이 된 경우 처리
        {
            currentHp = 0;
            IsDie = true;
            eCharacterActionable = ECharacterActionable.Unactionable;
        }
    }
    public virtual void Hit(float damage)
    {
        DecreaseHp(damage);
    }
    protected virtual bool Move() //이동 함수
    {
        if (IsDie || eCharacterActionable == ECharacterActionable.Unactionable) return false; //행동 가능한 상태가 아니면 false
        return true;
    }
    public virtual void KnockBack(float speed, float duration) //캐릭터 뒷방향으로의 넉백 함수
    {
        if (IsKnockBack) return;
        IsKnockBack = true;
        eCharacterActionable = ECharacterActionable.Unactionable; //행동 불가 처리

        StartCoroutine(Co_KnockBack(speed, duration));
    }
    public virtual void KnockBack(float speed, float duration, Vector3 direction) //임의의 방향으로의 넉백 함수
    {
        if (IsKnockBack) return;
        IsKnockBack = true;
        eCharacterActionable = ECharacterActionable.Unactionable; //행동 불가 처리

        StartCoroutine(Co_KnockBack(speed, duration, direction));
    }
    protected virtual IEnumerator Co_KnockBack(float speed, float duration) //스피드 속도로 듀레이션 동안 뒤로 밀려남
    {
        rigidbody.velocity = transform.forward * -1 * speed;

        yield return new WaitForSeconds(duration); //경직 처리
        rigidbody.velocity = Vector3.zero;

        IsKnockBack = false;
        eCharacterActionable = ECharacterActionable.Actionable; //행동 재개
    }
    protected virtual IEnumerator Co_KnockBack(float speed, float duration, Vector3 direction) //스피드 속도로 듀레이션 동안 디렉션 방향으로 밀려남
    {
        rigidbody.velocity = direction * speed;

        yield return new WaitForSeconds(duration); //경직 처리
        rigidbody.velocity = Vector3.zero;

        IsKnockBack = false;
        eCharacterActionable = ECharacterActionable.Actionable; //행동 재개
    }
}
