using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveUtility
{
    //������ � ���� �׳� �ڵ�� �״�� �ű� ����. t�� ��Į��� 0~1������ ��(�����̶� �Ȱ���)
    public static Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0 + 2 * u * t * p1 + tt * p2;
        //��� ����ü ���� ��ġ y���� 0.5�� ���ִ� ó��
        p.y = 0.5f;

        return p;
    }
}
