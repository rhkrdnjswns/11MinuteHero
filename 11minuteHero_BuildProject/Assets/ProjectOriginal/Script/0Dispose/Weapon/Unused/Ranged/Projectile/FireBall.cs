using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Projectile
{
    private bool isExplosionReady; //공격횟수 충족했는지 체크
    private float explosionDamage; //폭발 데미지
    private float explosionRadius; //폭발 반경

    [SerializeField] private GameObject explosionPrefab; //폭발 오브젝트 프리팹

    private GameObject explosionObj; //폭발 오브젝트 프리팹으로 생성할 오브젝트의 참조

    private ParticleSystem explosionParticle; //폭발 파티클

    private LayerMask layerMask;

    private bool isAlreadyInit; //첫 초기화인지 체크

    private void InitExplosionObj() //파이어볼 폭발 오브젝트 초기화
    {
        if (isAlreadyInit) return;
        layerMask = LayerMask.GetMask(ConstDefine.TAG_MONSTER);

        explosionRadius = 3; //폭발 반경

        explosionObj = Instantiate(explosionPrefab);
        explosionObj.transform.SetParent(null);
        explosionParticle = explosionObj.GetComponent<ParticleSystem>();

        isAlreadyInit = true;
    }
    public void SetProjectile(float range, float projectileSpeed, float damage)
    {
        //base.SetProjectile(range, projectileSpeed, damage);
        //explosionDamage = damage;
        //InitExplosionObj(); //첫 생성 시 폭발 오브젝트 초기화
    }
    //public override void ShotProjectile(Vector3 pos, bool isExplosion) //공격 횟수 충족 시마다 isExplosionReady true로 해줌
    //{
    //    isExplosionReady = isExplosion;
    //    base.ShotProjectile(pos, isExplosion);
    //}

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER)) //몬스터와 부딪힌 경우
        {
            //other.GetComponent<Monster>().Hit(damage); //Monster 클래스를 추출하여 데미지 연산
            if (isExplosionReady) //공격 횟수 충족 시에
            {
                explosionObj.transform.position = transform.position; //폭발 이펙트 위치 갱신
                explosionParticle.Play();
                Collider[] inRadiusMonsterArray = Physics.OverlapSphere(transform.position, explosionRadius, layerMask); //반경만큼 충돌 연산
                if (inRadiusMonsterArray.Length != 0)
                {
                    foreach (var monster in inRadiusMonsterArray) //반경 내 몬스터 폭발 데미지
                    {
                        monster.GetComponent<Monster>().Hit(explosionDamage);
                    }
                }
            }
            //rangedWeapon.ReturnProjectile(this); //이후 풀로 되돌림
        }
    }
    private void OnDrawGizmos() //반경 기즈모
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    protected override IEnumerator Co_Shot()
    {
        throw new System.NotImplementedException();
    }
}
