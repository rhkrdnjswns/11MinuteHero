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
    public virtual IEnumerator Co_ActiveDecal(Vector3 size) //���� �����Ϸ� ����
    {
        transform.localScale = size;
        transform.position += Vector3.up * 0.01f;
        gameObject.SetActive(true);

        yield return null;
    }
    public virtual IEnumerator Co_ActiveDecal(float size) //���� �����Ϸ� ����
    {
        transform.localScale = Vector3.one * size;
        transform.position += Vector3.up * 0.01f;
        gameObject.SetActive(true);

        yield return null;
    }
    public virtual IEnumerator Co_ActiveDecal(Vector3 size, float chargingTime) //��¡ ��Į��
    {
        transform.localScale = size;
        transform.position += Vector3.up * 0.01f;
        gameObject.SetActive(true);

        yield return null;
    }
    public virtual IEnumerator Co_ActiveDecal(float size, float chargingTime) //��¡ ��Į��
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
