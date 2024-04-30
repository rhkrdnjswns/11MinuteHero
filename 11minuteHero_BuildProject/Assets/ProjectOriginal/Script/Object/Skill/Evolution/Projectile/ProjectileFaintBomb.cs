using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFaintBomb : ProjectileShockBomb
{
    protected override IEnumerator Co_Shot()
    {
        float timer = 0;
        while (timer < flightDuration)
        {
            //y축 속도는 중력 가속도의 영향을 받기 때문에 아래처럼 처리. 결국 수평 평면 높이로 돌아옴. 체공시간 / 2 지점이 포물선 운동의 최고 높이 지점
            transform.Translate(0, (velocityY - (ConstDefine.GRAVITY_SCALE * timer)) * Time.deltaTime, velocityX * Time.deltaTime);
            timer += Time.deltaTime;

            yield return null;
        }
        bombObject.SetActive(false);
        transform.position += Vector3.down * 0.5f;

        explosionParticle.Play();
        int num = Physics.OverlapSphereNonAlloc(transform.position, radius, collisionArray, ConstDefine.LAYER_MONSTER);
        AttackInRangeUtility.AttackAndStunLayerInRange(collisionArray, damage, num, duration);
#if UNITY_EDITOR
        InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += num * damage;
#endif

        yield return coroutineEndDelay;

        owner.ReturnProjectile(this);
    }
}
