using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    //public Transform startPoint;    // ������(Transform)
    //public Transform endPoint;      // ����(Transform)
    //public Transform controlPoint;  // ������(Transform)
    //public float speed = 5f;        // ����ü�� �ӵ�
    //public float maxSpeed = 5f;

    //public Transform monster;
    //public Vector3 pos;
    //public Vector3 velocity;

    //private float timeCounter = 0f; // �ð� ����

    //private void Start()
    //{
    //    // pos = monster.transform.position;
    //    // velocity = (controlPoint.position - startPoint.position).normalized * speed;
    //}
    //void Update()
    //{
    //    // �ð��� ������Ŵ
    //    //timeCounter += speed * Time.deltaTime;
    //    //if (timeCounter >= 1) timeCounter = 1;
    //    //// ������ ��� ���� �̵�
    //    //Vector3 newPos = CalculateBezierPoint(startPoint.position, Vector3.Cross(startPoint.position - controlPoint.position, Vector3.up).normalized * 10, controlPoint.position, timeCounter);
    //    //transform.position = newPos;
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        StartCoroutine(Co_Parabola());
    //        //Debug.Log(transform.forward);
    //        ////Debug.Log(new Vector3(-transform.forward.y, transform.forward.x, 0).normalized);
    //        //transform.position += Vector3.Cross(transform.forward.normalized, Vector3.up) * -1;
    //    }
    //    // ������ �����ϸ� �ı�
    //    //if (timeCounter >= 1f)
    //    //{
    //    //    Destroy(gameObject);
    //    //}
    //    //transform.position = Vector3.Slerp(transform.position, endPoint.position, speed * Time.deltaTime);

    //// ������ ��� �� ���� ����ϴ� �Լ�
    //Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    //{
    //    float u = 1 - t;
    //    float tt = t * t;
    //    float uu = u * u;
    //    Vector3 p = uu * p0 + 2 * u * t * p1 + tt * p2;
    //    p.y = 0f;
    //    return p;
    //}
}
