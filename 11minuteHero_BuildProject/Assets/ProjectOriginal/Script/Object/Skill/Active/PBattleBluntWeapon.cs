using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBattleBluntWeapon : Projectile
{
    protected float distance; //����ü�� ���ư��� �Ÿ�

    private Vector3 summonPos; //����ü�� ���ư� �Ÿ� üũ�� ���� ���� ����Ʈ
    [SerializeField] private Transform objTransform; //�������� ����ü ������Ʈ�� Ʈ������ (ȸ�� ȿ���� ���� ����)

    private ABattleBluntWeapon aBattleBluntWeapon;
    private bool isReturn; //����ü�� �ٽ� ���ƿ��� ���� �˹��� X
    public override void SetDistance(float distance)
    {
        this.distance = distance;
    }
    public void SetBattalBluntWeapon(ABattleBluntWeapon reference)
    {
        aBattleBluntWeapon = reference;
    }
    public override void ShotProjectile() //����ü ���� ��ġ�� ���ư� ���� �����ϰ� ���ư��� �ڷ�ƾ ����
    {
        summonPos = InGameManager.Instance.Player.transform.position;
        summonPos.y = 0.5f;
        isReturn = false;

        base.ShotProjectile();
        StartCoroutine(Co_Rotate());
    }
    protected override IEnumerator Co_Shot()
    {
        while (Vector3.Distance(summonPos, transform.position) < distance) //��ȯ �� ��Ÿ���ŭ ���ư�
        {
            transform.position += shotDirection.normalized * rangedAttackUtility.ProjectileSpeed * Time.deltaTime;
            yield return null;
        }
        //yield return new WaitForSeconds(0.5f);
        isReturn = true;
        while (Vector3.Distance(InGameManager.Instance.Player.transform.position + Vector3.up * 0.5f, transform.position) > 0.5f) //���� �÷��̾� ��ġ�� ���ư�
        {
            Vector3 direction = InGameManager.Instance.Player.transform.position + Vector3.up * 0.5f - transform.position;//�÷��̾ ��� �̵��� �� �ֱ� ������ ��ġ�� �����Ӹ��� ����
            transform.position += direction.normalized * rangedAttackUtility.ProjectileSpeed * Time.deltaTime;
            yield return null;
        }
        aBattleBluntWeapon.ReturnCount++;
        rangedAttackUtility.ReturnProjectile(this);
    }
    private IEnumerator Co_Rotate() //������ġ�� Ȱ��ȭ �� ���ȿ��� ��� ȸ���ϵ��� �ϴ� �ڷ�ƾ
    {
        while (true)
        {
            objTransform.Rotate(Vector3.forward * 1440 * Time.deltaTime);
            yield return null;
        }
    }
    protected override void OnTriggerEnter(Collider other) //����ü �浹 ó��
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER)) //���Ϳ� �ε��� ���
        {
            Character c = other.GetComponent<Character>();
            c.Hit(rangedAttackUtility.ProjectileDamage); //Monster Ŭ������ �����Ͽ� ������ ����
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += rangedAttackUtility.ProjectileDamage;
#endif
            if (!c.IsDie && !isReturn) c.KnockBack(0.3f, 0.3f);
        }
    }
}
