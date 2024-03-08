using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour //����ü Ŭ����. ȭ��, �ܰ� �� ����ü���� �ش� Ŭ������ ��ӹ��� ����. //����ü�� Weapon�� ������ ��ü�� ����
{
    protected RangedAttackUtility rangedAttackUtility; //����ü�� �߻��� ���Ÿ���ų ����
    protected Vector3 shotDirection; //����ü�� ���ư� ����
    
    public bool IsMaxLevel { get; set; } //����ü�� ������ �ִ� ���� Ư�� ȿ���� ����ϱ� ���� üũ
    public virtual void SetCount(int count)
    {
        //������ ����ü�� ��� ����Ƚ�� ����, �ܰ��� ��� ƨ��� Ƚ�� ����
    }
    public virtual void SetActivateTime(float time)
    {
        //���� �ð��� ���� �� ������� ����ü�� ��� �ð� ����
    }
    public virtual void SetDistance(float distance)
    {
        //���ư��� �Ÿ��� ������ ����ü�� ��� �Ÿ� ����
    }
    public virtual void SetAttackRadiusUtility(AttackRadiusUtility attackRadiusUtility)
    {
        //�ݰ� ������ �ϴ� ����ü�� ��� ���� ����
    }
    public virtual void SetRangeAttackUtility(RangedAttackUtility rangedSkill) //����ü �ʱ�ȭ
    {
        rangedAttackUtility = rangedSkill;
    }
    public void SetShotDirection(Vector3 direction) //���ư� ���� ����
    {
        shotDirection = direction;
    }
    public virtual void IncreaseSize(float value)
    {
        transform.localScale += Vector3.one * (value / 100f);
    }
    public virtual void ShotProjectile(Transform target) //����ü �߻� (������ ����ü�� ��� target�� �Ű������� �ޱ� ���� �����ε�)
    {
        StartCoroutine(Co_Shot());
    }
    public virtual void ShotProjectile(Vector3 pos) //��ź�� ����ü�� Ư�� ��ġ�� ������ ��� ȣ��
    {
        StartCoroutine(Co_Shot());
    }
    public virtual void ShotProjectile() //����ü �߻�
    {
        StartCoroutine(Co_Shot());
    }
    protected abstract IEnumerator Co_Shot(); //����ü�� ���ư��� �ڷ�ƾ

    protected virtual void OnTriggerEnter(Collider other) //����ü �浹 ó��
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER)) //���Ϳ� �ε��� ���
        {
            other.GetComponent<Monster>().Hit(rangedAttackUtility.ProjectileDamage); //Monster Ŭ������ �����Ͽ� ������ ����
            rangedAttackUtility.ReturnProjectile(this); //���� Ǯ�� �ǵ���
            // -> �ʵ� ���� ���� ���� ���Ͱ� �����ϱ� ������ �浹�� ���͸� �����ϴ� �� ���� ����Ʈ�� ��ȸ�ϴ� �۾����� ���ɸ鿡�� ȿ������
        }
    }
}
