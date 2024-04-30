using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireBomb : ParabolaProjectile //�����ź�� �������� ���Ƽ� ��Ӱ���� ��. Ŭ���� ���� �򰥸� �� ������ �׳� �տ� P�� ����
{
    private float dotDuration; //��Ÿ�� ���� ���ӽð�
    private float currentTime; //���� ���� �ð�
    private float dotAreaRadius;

    private WaitForSeconds dotDelay;

    [SerializeField] protected GameObject bombObject; //��ź ������Ʈ
    [SerializeField] protected ParticleSystem explosionParticle; //���� ��ƼŬ

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
            //y�� �ӵ��� �߷� ���ӵ��� ������ �ޱ� ������ �Ʒ�ó�� ó��. �ᱹ ���� ��� ���̷� ���ƿ�. ü���ð� / 2 ������ ������ ��� �ְ� ���� ����
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
