using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFireBomb : PBomb //충격폭탄과 교집합이 많아서 상속관계로 둠. 클래스 명은 헷갈릴 수 있으니 그냥 앞에 P로 썼음
{
    private float dotDuration; //불타는 구역 지속시간
    private float dotInterval; //불타는 구역 데미지 간격
    private float currentTime; //현재 지속 시간

    public override void IncreaseSize(float value)
    {
        projectileObj.localScale += Vector3.one * value / 100f;
        ParticleSystem.MainModule m = explosionParticle.main;
        m.startSpeed = attackRadiusUtility.Radius;
    }
    public void SetDotInfo(float duration, float interval)
    {
        dotDuration = duration;
        dotInterval = interval;
    }
    protected override IEnumerator Co_Shot()
    {
        float timer = 0;
        while (timer < flightDuration)
        {
            //y축 속도는 중력 가속도의 영향을 받기 때문에 아래처럼 처리. 결국 수평 평면 높이로 돌아옴. 체공시간 / 2 지점이 포물선 운동의 최고 높이 지점
            transform.Translate(0, (velocityY - (ConstDefine.GRAVITY * timer)) * Time.deltaTime, velocityX * Time.deltaTime);
            timer += Time.deltaTime;

            yield return null;
        }
        bombObject.SetActive(false);
        transform.position += Vector3.down * 0.5f;
        StartCoroutine(Co_Burn());
        while (currentTime < dotDuration)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator Co_Burn()
    {
        explosionParticle.Play();
        while (currentTime < dotDuration)
        {
            yield return new WaitForSeconds(dotInterval);
            if (currentTime >= dotDuration) break;
            attackRadiusUtility.AttackLayerInRadius(attackRadiusUtility.GetLayerInRadius(transform), rangedAttackUtility.ProjectileDamage);
        }
        yield return new WaitForSeconds(1f);
        currentTime = 0;
        rangedAttackUtility.ReturnProjectile(this);
    }
}
