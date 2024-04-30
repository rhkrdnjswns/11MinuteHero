using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShockBomb : ParabolaProjectile
{
    private float slowDownValue;
    protected float duration;

    [SerializeField] protected GameObject bombObject; //��ź ������Ʈ
    [SerializeField] protected ParticleSystem explosionParticle; //���� ��ƼŬ

    [SerializeField] protected Transform projectileObj;

    protected float radius;
    protected Collider[] collisionArray = new Collider[30];
    protected WaitForSeconds coroutineEndDelay = new WaitForSeconds(1f);

    public override void SetAttackRadius(float radius)
    {
        this.radius = radius;
    }
    public override void SetSlowDownData(float value, float duration)
    {
        slowDownValue = value;
        this.duration = duration;
    }
    public override void IncreaseSize(float value)
    {
        projectileObj.localScale += Vector3.one * value;
        explosionParticle.transform.localScale += Vector3.one * value;
    }
    protected override IEnumerator Co_Shot()
    {
        float timer = 0;
        bombObject.SetActive(true);
        while(timer < flightDuration)
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
        AttackInRangeUtility.AttackAndSlowDownLayerInRange(collisionArray, damage, num, slowDownValue, duration);
#if UNITY_EDITOR
        InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += num * damage;
#endif
        yield return coroutineEndDelay;

        owner.ReturnProjectile(this);
    }
}
