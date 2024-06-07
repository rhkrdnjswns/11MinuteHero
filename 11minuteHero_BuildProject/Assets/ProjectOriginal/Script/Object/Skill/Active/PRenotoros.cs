using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRenotoros : PMagicShield
{
    private MRenotoros mRenotoros;

    public float radius = 2f; // 원의 반지름
    public float angularSpeed = 90f; // 회전 속도 (각속도)

    private float angle = 0f; // 회전 각도

    public void SetReference(MRenotoros reference)
    {
        mRenotoros = reference;
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
            transform.position += (transform.position - pos).normalized * rangedAttackUtility.ProjectileSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        mRenotoros.ReturnCount++;
        rangedAttackUtility.ReturnProjectile(this);
    }
}
