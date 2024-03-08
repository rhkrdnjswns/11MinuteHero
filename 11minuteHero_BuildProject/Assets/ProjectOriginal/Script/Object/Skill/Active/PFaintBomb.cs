using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFaintBomb : PBomb
{
    protected override IEnumerator Co_Shot()
    {
        float timer = 0;
        while (timer < flightDuration)
        {
            //y�� �ӵ��� �߷� ���ӵ��� ������ �ޱ� ������ �Ʒ�ó�� ó��. �ᱹ ���� ��� ���̷� ���ƿ�. ü���ð� / 2 ������ ������ ��� �ְ� ���� ����
            transform.Translate(0, (velocityY - (ConstDefine.GRAVITY * timer)) * Time.deltaTime, velocityX * Time.deltaTime);
            timer += Time.deltaTime;

            yield return null;
        }
        bombObject.SetActive(false);
        transform.position += Vector3.down * 0.5f;
        explosionParticle.Play();
        attackRadiusUtility.AttackLayerInRadius(attackRadiusUtility.GetLayerInRadius(transform), rangedAttackUtility.ProjectileDamage, EAttackType.Sturn, 1f);
        yield return new WaitForSeconds(1f);
        rangedAttackUtility.ReturnProjectile(this);
    }
}
