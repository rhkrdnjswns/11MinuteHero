using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon2
{
    [SerializeField] private GameObject projectilePrefab; //����ü ������
    protected Queue<Projectile> projectileQueue = new Queue<Projectile>(); //����ü Ǯ

    [SerializeField] protected int projectileCreateCount; //����ü ���� �� ���� ��
    [SerializeField] protected float projectileSpeed; //����ü �ӵ�
    [SerializeField] protected float range; //����ü ���ư��� �Ÿ�
    protected Transform spawnPoint; //����ü�� ���� ��ġ
    protected Transform activatedProjectileParent; //������ ����ü�� Ʈ������ �θ�
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
    protected void CreateNewProjectile(float count) //����ü count��ŭ ����
    {
        for (int i = 0; i < count; i++)
        {
            CreateProjectile();
        }
    }
    private void CreateProjectile() //����ü ����
    {
        GameObject obj = Instantiate(projectilePrefab); //���ӿ�����Ʈ ���� �� �ʱ�ȭ
        obj.SetActive(false);
        obj.transform.SetParent(transform);

        projectileQueue.Enqueue(obj.GetComponent<Projectile>());
    }
    protected virtual void SetProjectile() //����ü �ʱ�ȭ
    {
        foreach (Projectile projectile in projectileQueue)
        {
            //projectile.SetProjectile(range, projectileSpeed, damage, new RangedWeapon1());
            //projectile.RangedWeapon = this;
        }
    }
    protected virtual void SummonProjectile() //����ü ���� �Լ�
    {
        if(projectileQueue.Count <= 0)//Ǯ�� ����ü�� ������ ���� ����
        {
            CreateNewProjectile(projectileCreateCount);
        }
        Projectile p = projectileQueue.Dequeue();

        p.transform.SetParent(spawnPoint); //Ǯ���� ���� ����ü ��ġ, ȸ���� ������ġ�� �°� �ʱ�ȭ
        p.transform.position = spawnPoint.position;
        p.transform.localRotation = Quaternion.Euler(90, 0, 0);

        p.transform.SetParent(activatedProjectileParent); //���� �θ� �ٲ���. �ȹٲ��ָ� ������ġ�� ���ӵǼ� �÷��̾� �̵��� ����ü�� �����
        p.gameObject.SetActive(true);

        p.ShotProjectile(); //����ü �߻� �Լ� ȣ��
    }
    public override void Attack() //���� �Լ� �������̵�
    {
        SummonProjectile();
    }
    public void ReturnProjectile(Projectile projectile) //����ü�� Ǯ�� �ǵ����� �Լ�
    {
        projectile.gameObject.SetActive(false); //Ǯ�� �ǵ����� �����ʱ�ȭ
        projectile.transform.SetParent(transform);

        projectileQueue.Enqueue(projectile);
    }
}
