using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Projectile
{
    private bool isExplosionReady; //����Ƚ�� �����ߴ��� üũ
    private float explosionDamage; //���� ������
    private float explosionRadius; //���� �ݰ�

    [SerializeField] private GameObject explosionPrefab; //���� ������Ʈ ������

    private GameObject explosionObj; //���� ������Ʈ ���������� ������ ������Ʈ�� ����

    private ParticleSystem explosionParticle; //���� ��ƼŬ

    private LayerMask layerMask;

    private bool isAlreadyInit; //ù �ʱ�ȭ���� üũ

    private void InitExplosionObj() //���̾ ���� ������Ʈ �ʱ�ȭ
    {
        if (isAlreadyInit) return;
        layerMask = LayerMask.GetMask(ConstDefine.TAG_MONSTER);

        explosionRadius = 3; //���� �ݰ�

        explosionObj = Instantiate(explosionPrefab);
        explosionObj.transform.SetParent(null);
        explosionParticle = explosionObj.GetComponent<ParticleSystem>();

        isAlreadyInit = true;
    }
    public void SetProjectile(float range, float projectileSpeed, float damage)
    {
        //base.SetProjectile(range, projectileSpeed, damage);
        //explosionDamage = damage;
        //InitExplosionObj(); //ù ���� �� ���� ������Ʈ �ʱ�ȭ
    }
    //public override void ShotProjectile(Vector3 pos, bool isExplosion) //���� Ƚ�� ���� �ø��� isExplosionReady true�� ����
    //{
    //    isExplosionReady = isExplosion;
    //    base.ShotProjectile(pos, isExplosion);
    //}

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstDefine.TAG_MONSTER)) //���Ϳ� �ε��� ���
        {
            //other.GetComponent<Monster>().Hit(damage); //Monster Ŭ������ �����Ͽ� ������ ����
            if (isExplosionReady) //���� Ƚ�� ���� �ÿ�
            {
                explosionObj.transform.position = transform.position; //���� ����Ʈ ��ġ ����
                explosionParticle.Play();
                Collider[] inRadiusMonsterArray = Physics.OverlapSphere(transform.position, explosionRadius, layerMask); //�ݰ游ŭ �浹 ����
                if (inRadiusMonsterArray.Length != 0)
                {
                    foreach (var monster in inRadiusMonsterArray) //�ݰ� �� ���� ���� ������
                    {
                        monster.GetComponent<Monster>().Hit(explosionDamage);
                    }
                }
            }
            //rangedWeapon.ReturnProjectile(this); //���� Ǯ�� �ǵ���
        }
    }
    private void OnDrawGizmos() //�ݰ� �����
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    protected override IEnumerator Co_Shot()
    {
        throw new System.NotImplementedException();
    }
}
