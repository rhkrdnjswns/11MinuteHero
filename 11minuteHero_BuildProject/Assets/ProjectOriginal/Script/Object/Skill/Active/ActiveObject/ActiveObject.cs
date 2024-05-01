using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObject : MonoBehaviour
{
    protected IActiveObjectUsable owner;
    protected float damage;
    public void SetOwner(IActiveObjectUsable owner)
    {
        this.owner = owner;
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public virtual void SetAttackRadius(float radius)
    {
        //�ݰ� ������ �ϴ� ������Ʈ�� �ݰ� ����
    }
    public virtual void SetSlowDownData(float value, float duration)
    {
        //���ο� ������� �����ϴ� ������Ʈ�� ���ο� ���� ����
    }
    public virtual void SetSpeed(float value)
    {
        //�ӵ��� ������ ������Ʈ�� �ӵ� ����
    }
    public virtual void IncreaseSize(float value)
    {
        transform.localScale += Vector3.one * value;
    }
    public void Activate()
    {
        StartCoroutine(Co_Activation());
    }
    protected virtual IEnumerator Co_Activation()
    {
        yield return null;
        //���� ������Ʈ ��� ����
    }
}
