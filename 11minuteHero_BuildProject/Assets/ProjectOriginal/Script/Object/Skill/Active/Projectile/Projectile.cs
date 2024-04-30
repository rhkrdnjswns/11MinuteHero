using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour //����ü Ŭ����. ȭ��, �ܰ� �� ����ü���� �ش� Ŭ������ ��ӹ��� ����. //����ü�� Weapon�� ������ ��ü�� ����
{
    protected Vector3 shotDirection; //����ü�� ���ư� ����

    protected float damage;
    protected float speed;
    protected IProjectileUsable owner;
    protected string targetTag;
#if UNITY_EDITOR
    public int index;
#endif
    public bool IsMaxLevel { get; set; } //����ü�� ������ �ִ� ���� Ư�� ȿ���� ����ϱ� ���� üũ

    #region ---------��� ����ü �������--------------
    public virtual void SetOwner(IProjectileUsable owner)
    {
        this.owner = owner;
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    public void SetTargetTag(string tag)
    {
        targetTag = tag;
    }
    #endregion

    public virtual void SetPenetrationCount(int count)
    {
        //������ ����ü�� ��� ����Ƚ�� ����
    }
    public virtual void SetActivateTime(float time)
    {
        //Ȱ��ȭ �ð��� ������ ����ü�� ��� Ȱ��ȭ �ð� ����
    }
    public virtual void SetBounceCount(int count)
    {
        //ƨ��� ����ü�� ��� ƨ��� Ƚ�� ����
    }
    public virtual void SetTargetTransform(Transform transform)
    {
        //������ ����ü�� ��� Ÿ�� ���� ����
    }
    public virtual void SetDistance(float distance)
    {
        //���ư��� �Ÿ��� ������ ����ü�� ��� �Ÿ� ����
    }
    public virtual void SetAttackRadius(float radius)
    {
        //�ݰ� ������ �ϴ� ����ü�� ��� �ݰ� ����
    }
    public virtual void SetRotationSpeed(float speed)
    {
        //�����ϴ� ����ü�� ��� ���� �ӵ� ����
    }
    public virtual void SetSensingRadius(float radius)
    {
        //�ֺ� �ݰ��� �����ϴ� ����ü�� ��� ���� �ݰ� ����
    }
    public virtual void SetDotData(float dotInterval, float duration)
    {
        //��Ʈ �������� ������ ����ü�� ��� ������ �ֱ�� ���ӽð� ����
    }
    public virtual void SetCount(int count)
    {
        //������ ����ü�� ��� ����Ƚ�� ����, �ܰ��� ��� ƨ��� Ƚ�� ����
    }
    public virtual void SetTargetPos(Vector3 pos)
    {
        //Ư�� ��ġ�� ���ư��� ����ü�� ��� ��ġ ����
    }
    public virtual void SetExplosionDamage(float damage)
    {
        //�����ϴ� ����ü�� ��� ���� ������ ����
    }
    public virtual void SetAngle(float angle)
    {
        //������ ��� �ϴ� ����ü�� ���� ����
    }
    public virtual void SetMotion(Vector3 pos)
    {
        //������ ��� �ϴ� ����ü�� ��� ��ǥ ���������� ������ � ���� ��ġ ����
    }
    public virtual void SetSlowDownData(float value, float duration)
    {
        //���� �� �̵��ӵ� ���ϸ� �����ϴ� ����ü�� ��� ����ġ�� ���ӽð� ����
    }
    public virtual void SetStunDuration(float duration)
    {
        //���� �� ������Ű�� ����ü�� ��� ���� ���ӽð� ����
    }
    public virtual void SetAction(System.Action action)
    {
        //Ư�� �̺�Ʈ�� �߽��ؾ� �ϴ� ����ü�� ��� �̺�Ʈ ����
    }
    public virtual void ShotProjectile() //����ü �߻�
    {
        StartCoroutine(Co_Shot());
    }
    public void SetShotDirection(Vector3 direction) //���ư� ���� ����
    {
        shotDirection = direction;
    }
    public virtual void IncreaseSize(float value)
    {
        transform.localScale += Vector3.one * value;
    }
    protected abstract IEnumerator Co_Shot(); //����ü�� ���ư��� �ڷ�ƾ

    protected virtual void OnTriggerEnter(Collider other) //����ü �浹 ó��
    {
        if (other.CompareTag(targetTag)) 
        {
            other.GetComponent<Character>().Hit(damage); //Monster Ŭ������ �����Ͽ� ������ ����
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
#endif
            owner.ReturnProjectile(this); //���� Ǯ�� �ǵ���
            // -> �ʵ� ���� ���� ���� ���Ͱ� �����ϱ� ������ �浹�� ���͸� �����ϴ� �� ���� ����Ʈ�� ��ȸ�ϴ� �۾����� ���ɸ鿡�� ȿ������
        }
    }
}
