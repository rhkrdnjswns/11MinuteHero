using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Test : MonoBehaviour
{
    public Transform target; // ��ǥ ������Ʈ (������Ʈ B)
    public float duration = 10f; // ������ ��� ���� �ð�
    private float elapsedTime = 0f; // ��� �ð�
    Vector3 startPosition;
    [Range(0, 1)]
    public float t;
    public void Start()
    {
        startPosition = transform.position;
    }
    public void Update()
    {
        elapsedTime += Time.deltaTime;

        // ���� ��� �ð��� 0�� 1 ������ ������ ����ȭ
        //float t = Mathf.Clamp01(elapsedTime / duration);

        // ���� ����(Slerp)�� ����Ͽ� ���� ��ġ�� ��ǥ ��ġ�� �ε巴�� �̵�
        transform.position = Vector3.Slerp(startPosition, target.position, t);

        if(t >= 1f)
        {
            elapsedTime = 0;
        }
    }
}
