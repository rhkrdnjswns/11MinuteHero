using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RangedAttackUtility //모든 투사체를 사용하는 스킬들의 기능과 데이터 모음. 관통횟수, 생성 시간 등 공통이 아닌 고유 정보는 해당 스킬 클래스에서 선언
{
    [SerializeField] private GameObject projectilePrefab; //투사체 프리팹
    protected Queue<Projectile> projectileQueue = new Queue<Projectile>(); //투사체 풀

    private List<Projectile> allProjectileList = new List<Projectile>();

    [SerializeField] protected int projectileCreateCount; //투사체 생성 시 생성 수
    protected int shotCount; //발사할 투사체 개수

    [SerializeField] protected float projectileSpeed; //투사체 속도
    protected float projectileDamage; //투사체 공격력

    protected Transform spawnPoint; //투사체 생성 위치
    protected Transform activatedProjectileParent; //발사한 투사체의 트랜스폼 부모

    private float increaseValue; //투사체 범위 증가 패시브가 이미 적용된 상태에서 새 투사체를 생성하는 경우 사이즈 적용을 위해 범위 증가 수치를 기억할 변수

    private Transform parent;
    public Transform Parent
    {
        get
        {
            return parent;
        }
        set
        {
            parent = value;
            activatedProjectileParent = GameObject.Find(ConstDefine.NAME_FIELD).transform;
        }
    }

    //public 

    public int ShotCount { get => shotCount; set => shotCount = value; }

    public float ProjectileSpeed { get => projectileSpeed; }
    public float ProjectileDamage { get => projectileDamage; }
    public List<Projectile> AllProjectileList { get => allProjectileList; }

    public bool IsValid()
    {
        return projectileQueue.Count > 0;
    }
    public Projectile SummonProjectile() //큐에서 투사체를 꺼내고 유효한 위치값 초기화 후 생성하고 반환
    {
        Projectile p = projectileQueue.Dequeue();

        p.transform.SetParent(activatedProjectileParent);
        p.gameObject.SetActive(true);
        return p;
    }
    public Projectile SummonProjectile(float addPosY)
    {
        Projectile p = projectileQueue.Dequeue();
        p.transform.localPosition += Vector3.up * addPosY;

        p.transform.SetParent(activatedProjectileParent);
        p.gameObject.SetActive(true);
        return p;
    }
    public Projectile SummonProjectile(Quaternion quaternion) //큐에서 투사체를 꺼내고 유효한 위치값 초기화 후 생성하고 반환. 회전값도 설정하는 경우의 오버로딩
    {
        Projectile p = projectileQueue.Dequeue();
        p.transform.localRotation = quaternion;

        p.transform.SetParent(activatedProjectileParent);
        p.gameObject.SetActive(true);
        return p;
    }
    public void ReturnProjectile(Projectile projectile) //재사용을 위한 투사체 회수
    {
        projectile.gameObject.SetActive(false);
        if(parent == null)
        {
            GameObject.Destroy(projectile.gameObject);
            return;
        }
        projectile.transform.SetParent(parent);
        projectile.transform.localPosition = Vector3.zero;
        projectile.transform.localRotation = Quaternion.identity;

        projectileQueue.Enqueue(projectile);
    }
    public void CreateNewProjectile()
    {
        for (int i = 0; i < projectileCreateCount; i++)
        {
            GameObject obj = Object.Instantiate(projectilePrefab); //게임오브젝트 생성 후 초기화
            obj.SetActive(false);
            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            Projectile p = obj.GetComponent<Projectile>();
            p.SetRangeAttackUtility(this);
            projectileQueue.Enqueue(p);
            allProjectileList.Add(p);
            p.IncreaseSize(increaseValue);
        }
    }
    public void CreateNewProjectile(AttackRadiusUtility attackRadiusUtility)
    {
        for (int i = 0; i < projectileCreateCount; i++)
        {
            GameObject obj = Object.Instantiate(projectilePrefab); //게임오브젝트 생성 후 초기화
            obj.SetActive(false);
            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            Projectile p = obj.GetComponent<Projectile>();
            p.SetRangeAttackUtility(this);
            p.SetAttackRadiusUtility(attackRadiusUtility);
            projectileQueue.Enqueue(p);
            allProjectileList.Add(p);
            p.IncreaseSize(increaseValue);
        }
    }
    public void SetAttackRadiusUtility(AttackRadiusUtility attackRadiusUtility)
    {
        foreach (var projectile in allProjectileList)
        {
            projectile.SetAttackRadiusUtility(attackRadiusUtility);
        }
    }
    public void SetActivateTime(float time)
    {
        foreach (var projectile in allProjectileList)
        {
            projectile.SetActivateTime(time);
        }
    }
    public void SetDistance(float distance)
    {
        foreach (var projectile in allProjectileList)
        {
            projectile.SetDistance(distance);
        }
    }
    public void SetCount(int count)
    {
        foreach (var projectile in allProjectileList)
        {
            projectile.SetCount(count);
        }
    }
    public void SetDamage(float damage)
    {
        projectileDamage = damage;
    }
    public void SetSpeed(float speed)
    {
        projectileSpeed = speed;
    }
    public void IncreaseSize(float value)
    {
        increaseValue += value;
        foreach (var projectile in allProjectileList)
        {
            projectile.IncreaseSize(value);
        }
    }
}
