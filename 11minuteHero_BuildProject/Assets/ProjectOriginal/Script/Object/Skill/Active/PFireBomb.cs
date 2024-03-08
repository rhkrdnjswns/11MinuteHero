using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFireBomb : PBomb //�����ź�� �������� ���Ƽ� ��Ӱ���� ��. Ŭ���� ���� �򰥸� �� ������ �׳� �տ� P�� ����
{
    private float dotDuration; //��Ÿ�� ���� ���ӽð�
    private float dotInterval; //��Ÿ�� ���� ������ ����
    private float currentTime; //���� ���� �ð�

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
            //y�� �ӵ��� �߷� ���ӵ��� ������ �ޱ� ������ �Ʒ�ó�� ó��. �ᱹ ���� ��� ���̷� ���ƿ�. ü���ð� / 2 ������ ������ ��� �ְ� ���� ����
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
