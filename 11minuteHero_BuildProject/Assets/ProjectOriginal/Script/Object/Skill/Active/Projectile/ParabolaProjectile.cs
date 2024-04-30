using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParabolaProjectile : Projectile
{
    protected float angle; //������ � ����
    protected float distance; //������ ��� ���� ��� �Ÿ�
    protected float velocity; //������ � �ʱ�ӵ�
    protected float velocityX; //������ � x�� �ʱ�ӵ�
    protected float velocityY; //������ � y�� �ʱ�ӵ�
    protected float flightDuration; //������ ��� ü�� �ð�

    public override void SetAngle(float angle)
    {
        this.angle = angle;
    }
    public override void SetMotion(Vector3 pos)
    {
        //������ �ʱ� �ӵ��� ���ؿ�
        distance = Vector3.Distance(transform.position, pos + Vector3.up * 0.5f);

        velocity = ProjectileMotionUtility.GetProjectileVelocity(distance, angle);

        velocityX = ProjectileMotionUtility.GetVelocityX(velocity, angle);
        velocityY = ProjectileMotionUtility.GetVelocityY(velocity, angle);

        flightDuration = distance / velocityX; //���� ��� �Ÿ��� �ᱹ ü�� �ð���(�������� �����ߴٴ� �� ü���� �����ٴ� ��)
                                               //x�� �ӵ��� �߷� ���ӵ��� ������ ���� �ʱ� ������ ��� ������. ������ x�� �ӵ��� distance��ŭ ���� �ð��� ü�� �ð���

        transform.forward = pos + Vector3.up * 0.5f - transform.position;
    }
}
