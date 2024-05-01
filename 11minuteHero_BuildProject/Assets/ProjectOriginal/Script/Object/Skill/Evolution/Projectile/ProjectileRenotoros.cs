using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRenotoros : ProjectileMagicShield
{
    private System.Action IncreaseReturnCount;
    public override void SetAction(System.Action action)
    {
        IncreaseReturnCount += action;
    }
    public override void ShotProjectile()
    {
        transform.localRotation = Quaternion.identity;
        StartCoroutine(Co_Shot());
    }
    protected override IEnumerator Co_Shot()
    {
        float timer = 0;
        transform.position += shotDirection;
        Vector3 pos = InGameManager.Instance.Player.transform.position + Vector3.up * 0.5f;
        while (timer < activateTime)
        {
            transform.RotateAround(pos, Vector3.up, 120 / Mathf.Lerp(1f, 1.5f, timer / activateTime) * Time.deltaTime);
            transform.position += (transform.position - pos).normalized * speed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        IncreaseReturnCount();
        owner.ReturnProjectile(this);
    }
}
