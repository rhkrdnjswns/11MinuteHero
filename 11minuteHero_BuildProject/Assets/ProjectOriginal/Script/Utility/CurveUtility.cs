using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveUtility
{
    //베지어 곡선 공식 그냥 코드로 그대로 옮긴 거임. t는 스칼라로 0~1사이의 값(보간이랑 똑같음)
    public static Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0 + 2 * u * t * p1 + tt * p2;
        //모든 투사체 생성 위치 y값이 0.5라서 해주는 처리
        p.y = 0.5f;

        return p;
    }
}
