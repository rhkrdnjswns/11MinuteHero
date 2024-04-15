using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AWeapon : SActive
{
    public float CoolTime { get => CurrentCoolTime; }
    public virtual bool GetBShotDone() //스태프의 발사 종료 체크를 위한 함수
    {
        return false;
    }
    public abstract void Attack();
    public virtual void SetSpeedUp()
    {
        //사자궁의 이동속도 증가 함수
    }
}
