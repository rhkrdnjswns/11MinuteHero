using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Test : MonoBehaviour
{
    public Transform target; // 목표 오브젝트 (오브젝트 B)
    public float duration = 10f; // 포물선 운동의 지속 시간
    private float elapsedTime = 0f; // 경과 시간
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

        // 현재 경과 시간을 0과 1 사이의 값으로 정규화
        //float t = Mathf.Clamp01(elapsedTime / duration);

        // 구형 보간(Slerp)을 사용하여 현재 위치를 목표 위치로 부드럽게 이동
        transform.position = Vector3.Slerp(startPosition, target.position, t);

        if(t >= 1f)
        {
            elapsedTime = 0;
        }
    }
}
