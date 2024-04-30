using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParabolaProjectile : Projectile
{
    protected float angle; //포물선 운동 각도
    protected float distance; //포물선 운동의 수평 평면 거리
    protected float velocity; //포물선 운동 초기속도
    protected float velocityX; //포물선 운동 x축 초기속도
    protected float velocityY; //포물선 운동 y축 초기속도
    protected float flightDuration; //포물선 운동의 체공 시간

    public override void SetAngle(float angle)
    {
        this.angle = angle;
    }
    public override void SetMotion(Vector3 pos)
    {
        //포물선 초기 속도들 구해옴
        distance = Vector3.Distance(transform.position, pos + Vector3.up * 0.5f);

        velocity = ProjectileMotionUtility.GetProjectileVelocity(distance, angle);

        velocityX = ProjectileMotionUtility.GetVelocityX(velocity, angle);
        velocityY = ProjectileMotionUtility.GetVelocityY(velocity, angle);

        flightDuration = distance / velocityX; //수평 평면 거리가 결국 체공 시간임(끝지점에 도착했다는 건 체공이 끝났다는 거)
                                               //x축 속도는 중력 가속도의 영향을 받지 않기 때문에 계속 일정함. 때문에 x축 속도로 distance만큼 가는 시간이 체공 시간임

        transform.forward = pos + Vector3.up * 0.5f - transform.position;
    }
}
