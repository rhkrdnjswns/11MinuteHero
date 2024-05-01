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
        //반경 공격을 하는 오브젝트의 반경 설정
    }
    public virtual void SetSlowDownData(float value, float duration)
    {
        //슬로우 디버프를 적용하는 오브젝트의 슬로우 관련 설정
    }
    public virtual void SetSpeed(float value)
    {
        //속도를 가지는 오브젝트의 속도 설정
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
        //하위 오브젝트 기능 구현
    }
}
