using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RangedAttackUtility //��� ����ü�� ����ϴ� ��ų���� ��ɰ� ������ ����. ����Ƚ��, ���� �ð� �� ������ �ƴ� ���� ������ �ش� ��ų Ŭ�������� ����
{
    [SerializeField] private GameObject projectilePrefab; //����ü ������
    protected Queue<Projectile> projectileQueue = new Queue<Projectile>(); //����ü Ǯ

    private List<Projectile> allProjectileList = new List<Projectile>();

    [SerializeField] protected int projectileCreateCount; //����ü ���� �� ���� ��
    protected int shotCount; //�߻��� ����ü ����

    [SerializeField] protected float projectileSpeed; //����ü �ӵ�
    protected float projectileDamage; //����ü ���ݷ�

    protected Transform spawnPoint; //����ü ���� ��ġ
    protected Transform activatedProjectileParent; //�߻��� ����ü�� Ʈ������ �θ�

    private float increaseValue; //����ü ���� ���� �нú갡 �̹� ����� ���¿��� �� ����ü�� �����ϴ� ��� ������ ������ ���� ���� ���� ��ġ�� ����� ����

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
    public Projectile SummonProjectile() //ť���� ����ü�� ������ ��ȿ�� ��ġ�� �ʱ�ȭ �� �����ϰ� ��ȯ
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
    public Projectile SummonProjectile(Quaternion quaternion) //ť���� ����ü�� ������ ��ȿ�� ��ġ�� �ʱ�ȭ �� �����ϰ� ��ȯ. ȸ������ �����ϴ� ����� �����ε�
    {
        Projectile p = projectileQueue.Dequeue();
        p.transform.localRotation = quaternion;

        p.transform.SetParent(activatedProjectileParent);
        p.gameObject.SetActive(true);
        return p;
    }
    public void ReturnProjectile(Projectile projectile) //������ ���� ����ü ȸ��
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
            GameObject obj = Object.Instantiate(projectilePrefab); //���ӿ�����Ʈ ���� �� �ʱ�ȭ
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
            GameObject obj = Object.Instantiate(projectilePrefab); //���ӿ�����Ʈ ���� �� �ʱ�ȭ
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
