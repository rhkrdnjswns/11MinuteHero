using System.Collections;
using UnityEngine;

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
    public virtual void KnockBack(float knockBackRange, float stiffnessTime) //�˹� �Լ�
    {
        if (IsKnockBack) return;
        IsKnockBack = true;
        eCharacterActionable = ECharacterActionable.Unactionable; //�ൿ �Ұ� ó��

        StartCoroutine(Co_KnockBack(knockBackRange, stiffnessTime));
    }
    protected virtual IEnumerator Co_KnockBack(float range, float stiffnessTime) //range��ŭ �ڷ� �з����� stiffnessTime��ŭ ����
    {
        Vector3 direction = transform.forward.normalized * -1; //�޹��� ����
        Vector3 pos = transform.position + direction * range; //�޹������� range��ŭ�� ��ġ ����

        while (Vector3.Distance(transform.position, pos) > 0.1f) //�������� pos�� ������ �������� �ʹ� �����ɸ��� ������ �������� ����
        {
            transform.position = Vector3.Lerp(transform.position, pos, 2 * Time.deltaTime); //������������ �ε巴�� ĳ���� �̵�
            if (IsDie) yield break; //�˹� ���� ����ϸ� �ڷ�ƾ Ż��
            yield return null;
        }
        yield return new WaitForSeconds(stiffnessTime); //���� ó��
        IsKnockBack = false;
        eCharacterActionable = ECharacterActionable.Actionable; //�ൿ �簳
    }
}
