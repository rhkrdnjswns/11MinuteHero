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
            //y�� �ӵ��� �߷� ���ӵ��� ������ �ޱ� ������ �Ʒ�ó�� ó��. �ᱹ ���� ��� ���̷� ���ƿ�. ü���ð� / 2 ������ ������ ��� �ְ� ���� ����
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
