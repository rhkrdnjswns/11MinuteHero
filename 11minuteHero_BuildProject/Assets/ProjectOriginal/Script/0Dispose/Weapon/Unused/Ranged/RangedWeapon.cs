using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon2
{
    [SerializeField] private GameObject projectilePrefab; //투사체 프리팹
    protected Queue<Projectile> projectileQueue = new Queue<Projectile>(); //투사체 풀

    [SerializeField] protected int projectileCreateCount; //투사체 생성 시 생성 수
    [SerializeField] protected float projectileSpeed; //투사체 속도
    [SerializeField] protected float range; //투사체 날아가는 거리
    protected Transform spawnPoint; //투사체를 생성 위치
    protected Transform activatedProjectileParent; //생성된 투사체의 트랜스폼 부모
    //protected override void Awake()
    //{
    //    base.Awake();
    //    spawnPoint = transform.root.Find(ConstDefine.NAME_PROJECTILE_SPAWN_POINT).transform;
    //    activatedProjectileParent = GameObject.Find(ConstDefine.NAME_FIELD).transform;
    //    CreateNewProjectile(projectileCreateCount);
    //}
    public override void UpdateDamage()
    {
        base.UpdateDamage();
        SetProjectile();
    }
    protected void CreateNewProjectile(float count) //투사체 count만큼 생성
    {
        for (int i = 0; i < count; i++)
        {
            CreateProjectile();
        }
    }
    private void CreateProjectile() //투사체 생성
    {
        GameObject obj = Instantiate(projectilePrefab); //게임오브젝트 생성 후 초기화
        obj.SetActive(false);
        obj.transform.SetParent(transform);

        projectileQueue.Enqueue(obj.GetComponent<Projectile>());
    }
    protected virtual void SetProjectile() //투사체 초기화
    {
        foreach (Projectile projectile in projectileQueue)
        {
            //projectile.SetProjectile(range, projectileSpeed, damage, new RangedWeapon1());
            //projectile.RangedWeapon = this;
        }
    }
    protected virtual void SummonProjectile() //투사체 생성 함수
    {
        if(projectileQueue.Count <= 0)//풀에 투사체가 없으면 새로 생성
        {
            CreateNewProjectile(projectileCreateCount);
        }
        Projectile p = projectileQueue.Dequeue();

        p.transform.SetParent(spawnPoint); //풀에서 꺼낸 투사체 위치, 회전각 생성위치에 맞게 초기화
        p.transform.position = spawnPoint.position;
        p.transform.localRotation = Quaternion.Euler(90, 0, 0);

        p.transform.SetParent(activatedProjectileParent); //이후 부모를 바꿔줌. 안바꿔주면 생성위치에 종속되서 플레이어 이동을 투사체가 따라옴
        p.gameObject.SetActive(true);

        p.ShotProjectile(); //투사체 발사 함수 호출
    }
    public override void Attack() //공격 함수 오버라이딩
    {
        SummonProjectile();
    }
    public void ReturnProjectile(Projectile projectile) //투사체를 풀로 되돌리는 함수
    {
        projectile.gameObject.SetActive(false); //풀로 되돌리기 관련초기화
        projectile.transform.SetParent(transform);

        projectileQueue.Enqueue(projectile);
    }
}
