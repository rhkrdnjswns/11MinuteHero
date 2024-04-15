using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHellFire : PPenetrationProjectile
{
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private GameObject fireBallObj;
    private AttackRadiusUtility attackRadiusUtility;
    private float explosionDamage;
    private bool isExplosion;

    private WaitForSeconds explosionDelay;
    public override void SetAttackRadiusUtility(AttackRadiusUtility attackRadiusUtility)
    {
        this.attackRadiusUtility = attackRadiusUtility;
        explosionDelay = new WaitForSeconds(2f);
    }
    public void SetExplosionDamage(float value)
    {
        explosionDamage = value;
    }
    protected override IEnumerator Co_Shot()
    {
        fireBallObj.SetActive(true);
        float timer = 0;
        isExplosion = false;
        while (timer < activateTime)
        {
            transform.position += shotDirection * rangedAttackUtility.ProjectileSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Co_Explosion());
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER))
        {
            other.GetComponent<Character>().Hit(rangedAttackUtility.ProjectileDamage); //Monster 클래스를 추출하여 데미지 연산
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += rangedAttackUtility.ProjectileDamage;
#endif
            currentCount--;

            if (currentCount > 0) return;
            StartCoroutine(Co_Explosion());
        }
    }
    private IEnumerator Co_Explosion()
    {
        if (isExplosion) yield break;
        fireBallObj.SetActive(false);
        isExplosion = true;
        explosionParticle.Play();
        attackRadiusUtility.AttackLayerInRadius(attackRadiusUtility.GetLayerInRadius(transform), explosionDamage);
#if UNITY_EDITOR
        int count = attackRadiusUtility.GetLayerInRadius(transform).Length;
        InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += count * explosionDamage;
#endif
        yield return explosionDelay;

        currentCount = count;
        rangedAttackUtility.ReturnProjectile(this);
    }
    public override void IncreaseSize(float value)
    {
        base.IncreaseSize(value);
        if (attackRadiusUtility == null) return;
        explosionParticle.transform.localScale = Vector3.one * attackRadiusUtility.Radius;
    }
}
