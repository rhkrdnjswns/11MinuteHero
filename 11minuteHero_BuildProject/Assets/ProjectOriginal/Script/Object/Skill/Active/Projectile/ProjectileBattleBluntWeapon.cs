using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBattleBluntWeapon : Projectile
{
    private float distance; //����ü�� ���ư��� �Ÿ�
    private float rotateSpeed; //���� �ӵ�
    private Vector3 summonPos; //����ü�� ���ư� �Ÿ� üũ�� ���� ���� ����Ʈ
    private float knockBackSpeed;
    private float knockBackDuration;

    [SerializeField] private Transform objTransform; //����ü �޽� ������Ʈ�� Ʈ������ (ȸ�� ȿ���� ���� ����)

    private System.Action IncreaseReturnCount;

    private bool isReturn; //����ü�� �ٽ� ���ƿ��� ���� �˹��� X

    public override void SetKnockBackData(float speed, float duration)
    {
        knockBackSpeed = speed;
        knockBackDuration = duration;
    }
    public override void SetRotationSpeed(float speed)
    {
        rotateSpeed = speed;
    }
    public override void SetAction(System.Action action)
    {
        IncreaseReturnCount += action;
    }
    public override void SetDistance(float distance)
    {
        this.distance = distance;
    }
    public override void ShotProjectile() //����ü ���� ��ġ�� ���ư� ���� �����ϰ� ���ư��� �ڷ�ƾ ����
    {
        summonPos = InGameManager.Instance.Player.transform.position + Vector3.up * 0.5f;
        isReturn = false;

        base.ShotProjectile();
        StartCoroutine(Co_Rotate());
    }
    protected override IEnumerator Co_Shot()
    {
        while (Vector3.Distance(summonPos, transform.position) < distance) //��ȯ �� ��Ÿ���ŭ ���ư�
        {
            transform.position += shotDirection.normalized * speed * Time.deltaTime;
            yield return null;
        }
        isReturn = true;
        while (Vector3.Distance(InGameManager.Instance.Player.transform.position + Vector3.up * 0.5f, transform.position) > 0.5f) //���� �÷��̾� ��ġ�� ���ư�
        {
            Vector3 direction = InGameManager.Instance.Player.transform.position + Vector3.up * 0.5f - transform.position;//�÷��̾ ��� �̵��� �� �ֱ� ������ ��ġ�� �����Ӹ��� ����
            transform.position += direction.normalized * speed * Time.deltaTime;
            yield return null;
        }
        IncreaseReturnCount(); //ȸ�� ī��Ʈ ������Ŵ

        owner.ReturnProjectile(this); //����ü ȸ��
    }
    private IEnumerator Co_Rotate() //������ġ�� Ȱ��ȭ �� ���ȿ��� ��� ȸ���ϵ��� �ϴ� �ڷ�ƾ
    {
        while (true)
        {
            objTransform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }
    protected override void OnTriggerEnter(Collider other) //����ü �浹 ó��
    {
        if (other.CompareTag(targetTag)) //���Ϳ� �ε��� ���
        {
            Character c = other.GetComponent<Character>();
            c.Hit(damage); //Monster Ŭ������ �����Ͽ� ������ ����
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
#endif
            if (!c.IsDie && !isReturn) c.KnockBack(knockBackSpeed, knockBackDuration, (c.transform.position - transform.position).normalized);
        }
    }
}
