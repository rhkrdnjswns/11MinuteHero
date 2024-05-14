using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHellFire : ProjectilePenetration
{
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private GameObject fireBallObj;

    private Collider[] explosionCollisionArray = new Collider[100];
    private float explosionDamage;
    private bool isExplosion;

    private float explosionRadius;

    private WaitForSeconds explosionDelay = new WaitForSeconds(1f);
    public override void SetAttackRadius(float radius)
    {
        explosionRadius = radius;
    }
    public override void SetExplosionDamage(float damage)
    {
        explosionDamage = damage;
    }
    protected override IEnumerator Co_Shot()
    {
        fireBallObj.SetActive(true);
        float timer = 0;
        isExplosion = false;
        while (timer < activateTime)
        {
            if (isExplosion) break;
            transform.position += shotDirection * speed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        if(!isExplosion)
        {
            StartCoroutine(Co_Explosion());
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            other.GetComponent<Character>().Hit(damage); //Monster 클래스를 추출하여 데미지 연산
#if UNITY_EDITOR
            InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += damage;
#endif
            currentCount--;

            if(other.TryGetComponent(out Boss b)) StartCoroutine(Co_Explosion());

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
        int num = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, explosionCollisionArray, ConstDefine.LAYER_MONSTER);
        AttackInRangeUtility.AttackLayerInRange(explosionCollisionArray, explosionDamage, num);
#if UNITY_EDITOR
        InGameManager.Instance.SkillManager.ActiveSkillList[index].TotalDamage += num * explosionDamage;
#endif
        yield return explosionDelay;

        currentCount = count;

        owner.ReturnProjectile(this);
    }
    public override void IncreaseSize(float value)
    {
        base.IncreaseSize(value);
       // explosionParticle.transform.localScale = Vector3.one * attackRadiusUtility.Radius;
    }
}
