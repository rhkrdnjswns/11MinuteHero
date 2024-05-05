using System.Collections.Generic;
using UnityEngine;

public interface IProjectileUsable
{
    public void ReturnProjectile(Projectile p);
}
public class ProjectileUtility : IProjectileUsable
{
    private ObjectPool<Projectile> projectilePool;

    private Transform parent;

    private List<Projectile> allProjectileList = new List<Projectile>();
    public List<Projectile> AllProjectileList { get => allProjectileList; }
    public ProjectileUtility(GameObject obj, int createCount, Transform parent)
    {
        this.parent = parent;
        projectilePool = new ObjectPool<Projectile>(obj, createCount, parent);

        CreateProjectile();
    }
    public void CreateProjectile()
    {
        projectilePool.CreateObject();
        foreach (var item in projectilePool.Pool)
        {
            if(!allProjectileList.Contains(item))
            {
                allProjectileList.Add(item);
            }
        }
    }
    public bool IsValid()
    {
        if(!projectilePool.IsValid())
        {
            foreach (var item in projectilePool.Pool)
            {
                if (!allProjectileList.Contains(item))
                {
                    allProjectileList.Add(item);
                }
            }
            return false;
        }
        return true;
    }
    public Projectile GetProjectile()
    {
        Projectile p = projectilePool.GetObject();
        p.transform.SetParent(null);
        p.gameObject.SetActive(true);

        return p;
    }
    public Projectile GetProjectile(Quaternion rot)
    {
        Projectile p = projectilePool.GetObject();
        p.transform.localRotation = rot;
        p.transform.SetParent(null);
        p.gameObject.SetActive(true);

        return p;
    }
    public void ReturnProjectile(Projectile p)
    {
        p.gameObject.SetActive(false);
        p.transform.SetParent(parent);
        p.transform.localPosition = Vector3.zero;
        p.transform.localRotation = Quaternion.identity;

        projectilePool.ReturnObject(p);
    }
    public void SetOwner()
    {
        foreach (var item in allProjectileList)
        {
            item.SetOwner(this);
        }
    }
    public void SetSpeed(float speed)
    {
        foreach (var item in allProjectileList)
        {
            item.SetSpeed(speed);
        }
    }
    public void SetTargetTag(string tag)
    {
        foreach (var item in allProjectileList)
        {
            item.SetTargetTag(tag);
        }
    }
    public void SetDamage(float damage)
    {
        foreach (var item in allProjectileList)
        {
            item.SetDamage(damage);
        }
    }
    public void SetActivateTime(float time)
    {
        foreach (var item in allProjectileList)
        {
            item.SetActivateTime(time);
        }
    }
    public void SetPenetrationCount(int count)
    {
        foreach (var item in allProjectileList)
        {
            item.SetPenetrationCount(count);
        }
    }
    public void SetRotateSpeed(float speed)
    {
        foreach (var item in allProjectileList)
        {
            item.SetRotationSpeed(speed);
        }
    }
    public void SetDistance(float distance)
    {
        foreach (var item in allProjectileList)
        {
            item.SetDistance(distance);
        }
    }
    public void SetAngle(float angle)
    {
        foreach (var item in allProjectileList)
        {
            item.SetAngle(angle);
        }
    }
    public void SetSlowDownData(float value, float duration)
    {
        foreach (var item in allProjectileList)
        {
            item.SetSlowDownData(value, duration);
        }
    }
    public void SetStunDuration(float duration)
    {
        foreach (var item in allProjectileList)
        {
            item.SetStunDuration(duration);
        }
    }
    public void SetAttackRadius(float radius)
    {
        foreach (var item in allProjectileList)
        {
            item.SetAttackRadius(radius);
        }
    }
    public void SetDotData(float interval, float duration)
    {
        foreach (var item in allProjectileList)
        {
            item.SetDotData(interval, duration);
        }
    }
    public void SetBounceCount(int count)
    {
        foreach (var item in allProjectileList)
        {
            item.SetBounceCount(count);
        }
    }
    public void SetExplosionDamage(float damage)
    {
        foreach (var item in allProjectileList)
        {
            item.SetExplosionDamage(damage);
        }
    }
    public void SetAction(System.Action action)
    {
        foreach (var item in allProjectileList)
        {
            item.SetAction(action);
        }
    }
    public void SetKnockBackData(float speed, float duration)
    {
        foreach (var item in allProjectileList)
        {
            item.SetKnockBackData(speed, duration);
        }
    }
    public void SetSensingRadius(float radius)
    {
        foreach (var item in allProjectileList)
        {
            item.SetSensingRadius(radius);
        }
    }
    public void IncreaseSize(float value)
    {
        foreach (var item in allProjectileList)
        {
            item.IncreaseSize(value);
        }
    }
}

