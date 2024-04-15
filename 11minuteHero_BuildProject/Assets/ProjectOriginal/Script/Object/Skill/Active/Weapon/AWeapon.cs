using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AWeapon : SActive
{
    public float CoolTime { get => CurrentCoolTime; }
    public virtual bool GetBShotDone() //�������� �߻� ���� üũ�� ���� �Լ�
    {
        return false;
    }
    public abstract void Attack();
    public virtual void SetSpeedUp()
    {
        //���ڱ��� �̵��ӵ� ���� �Լ�
    }
}
