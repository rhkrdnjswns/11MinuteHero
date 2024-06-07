using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBomb : Projectile
{
    protected float angle; //포물선 운동 각도
    protected float distance; //포물선 운동의 수평 평면 거리
    protected float velocity; //포물선 운동 초기속도
    protected float velocityX; //포물선 운동 x축 초기속도
    protected float velocityY; //포물선 운동 y축 초기속도
    protected float flightDuration; //포물선 운동의 체공 시간

    [SerializeField] protected GameObject bombObject; //폭탄 오브젝트
    [SerializeField] protected ParticleSystem explosionParticle; //폭발 파티클

    protected AttackRadiusUtility attackRadiusUtility; //폭탄 폭발 반경 참조

    [SerializeField] protected Transform projectileObj;

    protected WaitForSeconds coroutineEndDelay;
    public override void SetAttackRadiusUtility(AttackRadiusUtility attackRadiusUtility)
    {
        this.attackRadiusUtility = attackRadiusUtility;
        coroutineEndDelay = new WaitForSeconds(1f);
    }
    public void SetAngle(float angle)
    {
        this.angle = angle;
    }
    public override void ShotProjectile(Vector3 pos)
    {
        //포물선 초기 속도들 구해옴
        distance = Vector3.Distance(transform.position, pos + Vector3.up * 0.5f);
        velocity = ProjectileMotionUtility.GetProjectileVelocity(distance, angle);
        velocityX = ProjectileMotionUtility.GetVelocityX(velocity, angle);
        velocityY = ProjectileMotionUtility.GetVelocityY(velocity, angle);

        flightDuration = distance / velocityX; //수평 평면 거리가 결국 체공 시간임(끝지점에 도착했다는 건 체공이 끝났다는 거)
                                               //x축 속도는 중력 가속도의 영향을 받지 않기 때문에 계속 일정함. 때문에 x축 속도로 distance만큼 가는 시간이 체공 시간임

        transform.forward = pos + Vector3.up * 0.5f - transform.position;

        bombObject.SetActive(true);

        base.ShotProjectile();
    }
    public override void IncreaseSize(float value)
    {
        projectileObj.localScale += Vector3.one * 0.5f * value / 100f;
        explosionParticle.transform.localScale += Vector3.one * value / 100f;
    }
    protected override IEnumerator Co_Shot()
    {
        float timer = 0;
        while(timer < flightDuration)
        {
            //y축 속도는 중력 가속도의 영향을 받기 때문에 아래처럼 처리. 결국 수평 평면 높이로 돌아옴. 체공시간 / 2 지점이 포물선 운동의 최고 높이 지점
            transform.Translate(0, (velocityY - (ConstDefine.GRAVITY * timer)) * Time.deltaTime, velocityX * Time.deltaTime);
            timer += Time.deltaTime;

            yield return null;
        }
        bombObject.SetActive(false);
        transform.position += Vector3.down * 0.5f;

        explosionParticle.Play();
        attackRadiusUtility.AttackLayerInRadius(attackRadiusUtility.GetLayerInRadius(transform), rangedAttackUtility.ProjectileDamage, EAttackType.Slow, 1f, 50f);
#if UNITY_EDITOR
        int count = attackRadiusUtility.GetLayerInRadius(transform).Length;
        InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += count * rangedAttackUtility.ProjectileDamage;
#endif

        yield return coroutineEndDelay;
        rangedAttackUtility.ReturnProjectile(this);
    }
}
