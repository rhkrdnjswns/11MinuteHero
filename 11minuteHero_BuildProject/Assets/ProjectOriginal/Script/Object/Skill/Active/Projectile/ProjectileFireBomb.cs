using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireBomb : ParabolaProjectile //충격폭탄과 교집합이 많아서 상속관계로 둠. 클래스 명은 헷갈릴 수 있으니 그냥 앞에 P로 썼음
{
    private float dotDuration; //불타는 구역 지속시간
    private float currentTime; //현재 지속 시간
    private float dotAreaRadius;

    private WaitForSeconds dotDelay;

    [SerializeField] protected GameObject bombObject; //폭탄 오브젝트
    [SerializeField] protected ParticleSystem explosionParticle; //폭발 파티클

    [SerializeField] protected Transform projectileObj;

    private Collider[] collisionArray = new Collider[30];

    protected WaitForSeconds coroutineEndDelay = new WaitForSeconds(1f);

    public override void IncreaseSize(float value)
    {
        projectileObj.localScale += Vector3.one * value;
    }
    public override void SetDotData(float dotInterval, float duration)
    {
        dotDelay = new WaitForSeconds(dotInterval);
        dotDuration = duration;
    }
    public override void SetAttackRadius(float radius)
    {
        dotAreaRadius = radius;
        ParticleSystem.MainModule m = explosionParticle.main;
        m.startSpeed = dotAreaRadius;
    }
    protected override IEnumerator Co_Shot()
    {
        float timer = 0;
        bombObject.SetActive(true);

        while (timer < flightDuration)
        {
            //y축 속도는 중력 가속도의 영향을 받기 때문에 아래처럼 처리. 결국 수평 평면 높이로 돌아옴. 체공시간 / 2 지점이 포물선 운동의 최고 높이 지점
            transform.Translate(0, (velocityY - (ConstDefine.GRAVITY_SCALE * timer)) * Time.deltaTime, velocityX * Time.deltaTime);
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
            yield return dotDelay;

            if (currentTime >= dotDuration) break;

            int num = Physics.OverlapSphereNonAlloc(transform.position, dotAreaRadius, collisionArray, ConstDefine.LAYER_MONSTER);
            AttackInRangeUtility.AttackLayerInRange(collisionArray, damage, num);
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += num * damage;
#endif
        }
        yield return coroutineEndDelay;

        currentTime = 0;
        owner.ReturnProjectile(this);
    }
}
