using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileMotionUtility
{
    /// <summary>
    /// 포물선 운동의 초기 속도 반환
    /// V(초기속도) = d(수평 평면 거리) / (sin2θ(발사 각도에 따른 사인(높이) * 2) / g(중력 가속도)
    /// 수평 평면 거리와 발사 각도를 통해 초기 속도를 구하는 공식
    /// </summary>
    /// <param name="distance">수평 평면 거리</param>
    /// <param name="angle">각도</param>
    /// <returns></returns>
    public static float GetProjectileVelocity(float distance, float angle)
    {
        return distance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / ConstDefine.GRAVITY);
    }
    /// <summary>
    /// 포물선 운동의 Vx 반환
    /// Vx(x방향으로의 초기 속도) = V(초기 속도) * cosθ(발사각도에 따른 코사인(밑변))
    /// 외부 저항 없이 중력 가속도만 존재하는 환경에서 x방향으로의 속도는 항상 초기 속도이다.
    /// </summary>
    /// <param name="velocity">포물선 운동의 초기 속도</param>
    /// <param name="angle">각도</param>
    /// <returns></returns>
    public static float GetVelocityX(float velocity, float angle)
    { 
        return Mathf.Sqrt(velocity) * Mathf.Cos(angle * Mathf.Deg2Rad);
    }
    /// <summary>
    /// 포물선 운동의 Vy 반환
    /// Vy(y방향으로의 초기 속도) = V(초기 속도) * sinθ(발사각도에 따른 사인(높이))
    /// 외부 저항 없이 중력 가속도만 존재하는 환경에서 y방향으로의 속도는 Vy(y방향으로의 초기 속도) - (g(중력 가속도) * t(체공 시간))이다. 
    /// Vy = 0 이면 포물선의 최고 높이에 도달한 것
    /// </summary>
    /// <param name="velocity">포물선 운동의 초기 속도</param>
    /// <param name="angle">각도</param>
    /// <returns></returns>
    public static float GetVelocityY(float velocity, float angle)
    {
        return Mathf.Sqrt(velocity) * Mathf.Sin(angle * Mathf.Deg2Rad);
    }
}
