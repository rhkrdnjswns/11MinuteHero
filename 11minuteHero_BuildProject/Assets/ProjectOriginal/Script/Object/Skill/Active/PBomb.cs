using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBomb : Projectile
{
    protected float angle; //������ � ����
    protected float distance; //������ ��� ���� ��� �Ÿ�
    protected float velocity; //������ � �ʱ�ӵ�
    protected float velocityX; //������ � x�� �ʱ�ӵ�
    protected float velocityY; //������ � y�� �ʱ�ӵ�
    protected float flightDuration; //������ ��� ü�� �ð�

    [SerializeField] protected GameObject bombObject; //��ź ������Ʈ
    [SerializeField] protected ParticleSystem explosionParticle; //���� ��ƼŬ

    protected AttackRadiusUtility attackRadiusUtility; //��ź ���� �ݰ� ����

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
        //������ �ʱ� �ӵ��� ���ؿ�
        distance = Vector3.Distance(transform.position, pos + Vector3.up * 0.5f);
        velocity = ProjectileMotionUtility.GetProjectileVelocity(distance, angle);
        velocityX = ProjectileMotionUtility.GetVelocityX(velocity, angle);
        velocityY = ProjectileMotionUtility.GetVelocityY(velocity, angle);

        flightDuration = distance / velocityX; //���� ��� �Ÿ��� �ᱹ ü�� �ð���(�������� �����ߴٴ� �� ü���� �����ٴ� ��)
                                               //x�� �ӵ��� �߷� ���ӵ��� ������ ���� �ʱ� ������ ��� ������. ������ x�� �ӵ��� distance��ŭ ���� �ð��� ü�� �ð���

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
            //y�� �ӵ��� �߷� ���ӵ��� ������ �ޱ� ������ �Ʒ�ó�� ó��. �ᱹ ���� ��� ���̷� ���ƿ�. ü���ð� / 2 ������ ������ ��� �ְ� ���� ����
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
