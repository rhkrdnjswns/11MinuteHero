using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decal : MonoBehaviour
{
    protected virtual void Awake()
    {
        //gameObject.SetActive(false);
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public virtual IEnumerator Co_ActiveDecal(Vector3 size) //벡터 스케일로 조정
    {
        transform.localScale = size;
        transform.position += Vector3.up * 0.01f;
        gameObject.SetActive(true);

        yield return null;
    }
    public virtual IEnumerator Co_ActiveDecal(float size) //통합 스케일로 조정
    {
        transform.localScale = Vector3.one * size;
        transform.position += Vector3.up * 0.01f;
        gameObject.SetActive(true);

        yield return null;
    }
    public virtual IEnumerator Co_ActiveDecal(Vector3 size, float chargingTime) //차징 데칼용
    {
        transform.localScale = size;
        transform.position += Vector3.up * 0.01f;
        gameObject.SetActive(true);

        yield return null;
    }
    public virtual IEnumerator Co_ActiveDecal(float size, float chargingTime) //차징 데칼용
    {
        transform.localScale = Vector3.one * size;
        transform.position += Vector3.up * 0.01f;
        gameObject.SetActive(true);

        yield return null;
    }
    public virtual void InActiveDecal(Transform parent)
    {
        gameObject.SetActive(false);
        transform.SetParent(parent);
        transform.localScale = Vector3.one;
    }
}
