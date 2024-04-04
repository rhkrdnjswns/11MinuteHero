using System.Collections;
using UnityEngine;

public enum EApplicableType //��ġ ���� ���
{
    Value,
    Percentage
}
public enum ECharacterActionable //ĳ���Ͱ� �ൿ ������ �������� ����
{
    Actionable,
    Unactionable
}
public abstract class Character : MonoBehaviour //�𸮾��� Character�� ���� ����. �÷��̾�, ������ �θ� Ŭ����
{
    [SerializeField] protected float maxHp; //�ִ� ü��
    [SerializeField] protected float speed; //�̵��ӵ�

    [SerializeField] protected float currentHp; //���� ü��
    protected float currentSpeed; //���� �̵��ӵ�

    protected Animator animator; //�ִϸ����� ������Ʈ ����

    protected Rigidbody rigidbody; //������ٵ� ������Ʈ ����

    protected ECharacterActionable eCharacterActionable = ECharacterActionable.Actionable;

    public bool IsDie { get; set; }
    public bool IsKnockBack { get; set; }

    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float Speed { get => speed; set => speed = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
    public ECharacterActionable ECharacterActionable { get => eCharacterActionable; set => eCharacterActionable = value; }

    protected virtual void Awake() //�ʱ�ȭ
    {
        currentHp = maxHp;
        currentSpeed = speed;

        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }
    protected virtual void DecreaseHp(float value) //ü�� ����
    {
        currentHp -= value;
        if (currentHp < 0) //ü���� 0�� �� ��� ó��
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
    protected virtual bool Move() //�̵� �Լ�
    {
        if (IsDie || eCharacterActionable == ECharacterActionable.Unactionable) return false; //�ൿ ������ ���°� �ƴϸ� false
        return true;
    }
    public virtual void KnockBack(float speed, float duration) //ĳ���� �޹��������� �˹� �Լ�
    {
        if (IsKnockBack) return;
        IsKnockBack = true;
        eCharacterActionable = ECharacterActionable.Unactionable; //�ൿ �Ұ� ó��

        StartCoroutine(Co_KnockBack(speed, duration));
    }
    public virtual void KnockBack(float speed, float duration, Vector3 direction) //������ ���������� �˹� �Լ�
    {
        if (IsKnockBack) return;
        IsKnockBack = true;
        eCharacterActionable = ECharacterActionable.Unactionable; //�ൿ �Ұ� ó��

        StartCoroutine(Co_KnockBack(speed, duration, direction));
    }
    protected virtual IEnumerator Co_KnockBack(float speed, float duration) //���ǵ� �ӵ��� �෹�̼� ���� �ڷ� �з���
    {
        rigidbody.velocity = transform.forward * -1 * speed;

        yield return new WaitForSeconds(duration); //���� ó��
        rigidbody.velocity = Vector3.zero;

        IsKnockBack = false;
        eCharacterActionable = ECharacterActionable.Actionable; //�ൿ �簳
    }
    protected virtual IEnumerator Co_KnockBack(float speed, float duration, Vector3 direction) //���ǵ� �ӵ��� �෹�̼� ���� �𷺼� �������� �з���
    {
        rigidbody.velocity = direction * speed;

        yield return new WaitForSeconds(duration); //���� ó��
        rigidbody.velocity = Vector3.zero;

        IsKnockBack = false;
        eCharacterActionable = ECharacterActionable.Actionable; //�ൿ �簳
    }
}
