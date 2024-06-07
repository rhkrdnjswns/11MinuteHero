using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGolemProjectile : Projectile
{
    protected float distance; //투사체가 날아가는 거리
    System.Action<Vector3> action;

    public override void SetAction<T>(System.Action<T> action)
    {
        if(action is System.Action<Vector3> vectorAction)
        {
            this.action = vectorAction;
        }
    }
    public override void SetDistance(float distance)
    {
        this.distance = distance;
    }
    protected override IEnumerator Co_Shot()
    {
        Vector3 summonPos = transform.position;
        summonPos.y = 0.5f;
        while(Vector3.Distance(summonPos, transform.position) < distance)
        {
            transform.position += shotDirection.normalized * speed * Time.deltaTime;
            yield return null;
        }
        action(transform.position);

        owner.ReturnProjectile(this);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstDefine.TAG_PLAYER))
        {
            InGameManager.Instance.Player.Hit(damage);
        }
    }
}
