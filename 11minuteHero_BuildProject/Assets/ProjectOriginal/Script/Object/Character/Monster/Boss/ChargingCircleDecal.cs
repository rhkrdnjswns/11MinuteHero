using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingCircleDecal : Decal
{
    private Transform charging;
    private Vector3 defaultScale = new Vector3(0, 0, 1);
    private void ResetCharging()
    {
        charging.localScale = defaultScale;
    }
    protected override void Awake()
    {
        base.Awake();
        charging = transform.GetChild(0).transform;
        ResetCharging();
    }
    public override IEnumerator Co_ActiveDecal(float size, float chargingTime)
    {
        transform.position += Vector3.up * 0.01f;
        transform.localScale = Vector3.one * size;
        gameObject.SetActive(true);

        float timer = 0;
        while (timer < chargingTime)
        {
            charging.localScale = Vector3.Lerp(defaultScale, Vector3.one, timer / chargingTime);
            timer += Time.deltaTime;

            yield return null;
        }
    }
    public override IEnumerator Co_ActiveDecal(Vector3 size, float chargingTime)
    {
        transform.position += Vector3.up * 0.01f;
        transform.localScale = size;
        gameObject.SetActive(true);

        float timer = 0;
        while (timer < chargingTime)
        {
            charging.localScale = Vector3.Lerp(defaultScale, Vector3.one, timer / chargingTime);
            timer += Time.deltaTime;

            yield return null;
        }
    }
    public override void InActiveDecal(Transform parent)
    {
        base.InActiveDecal(parent);
        ResetCharging();
    }
}