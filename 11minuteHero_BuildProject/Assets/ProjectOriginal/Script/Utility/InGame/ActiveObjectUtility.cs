using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActiveObjectUsable
{
    public void ReturnActiveObject(ActiveObject obj);
}
public class ActiveObjectUtility : IActiveObjectUsable
{
    private ObjectPool<ActiveObject> objectPool;

    private Transform parent; 

    private List<ActiveObject> allActiveObjectList = new List<ActiveObject>();
    public List<ActiveObject> AllActiveObjectList { get => allActiveObjectList; }
    public ActiveObjectUtility(GameObject obj, int createCount, Transform parent)
    {
        this.parent = parent;
        objectPool = new ObjectPool<ActiveObject>(obj, createCount, parent);

        CreateProjectile();
    }
    public void CreateProjectile()
    {
        objectPool.CreateObject();
        foreach (var item in objectPool.Pool)
        {
            if (!allActiveObjectList.Contains(item))
            {
                allActiveObjectList.Add(item);
            }
        }
    }
    public bool IsValid()
    {
        if (!objectPool.IsValid())
        {
            foreach (var item in objectPool.Pool)
            {
                if (!allActiveObjectList.Contains(item))
                {
                    allActiveObjectList.Add(item);
                }
            }
            return false;
        }
        return true;
    }
    public ActiveObject GetObject()
    {
        ActiveObject obj = objectPool.GetObject();
        obj.transform.SetParent(null);
        obj.gameObject.SetActive(true);

        return obj;
    }
    public ActiveObject GetObjectMaintainParent()
    {
        ActiveObject obj = objectPool.GetObject();
        obj.gameObject.SetActive(true);

        return obj;
    }
    public void ReturnActiveObject(ActiveObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(parent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        objectPool.ReturnObject(obj);
    }

    public void SetOwner()
    {
        foreach (var item in allActiveObjectList)
        {
            item.SetOwner(this);
        }
    }
    public void SetDamage(float damage)
    {
        foreach (var item in allActiveObjectList)
        {
            item.SetDamage(damage);
        }
    }
    public void SetSpeed(float value)
    {
        foreach (var item in allActiveObjectList)
        {
            item.SetSpeed(value);
        }
    }
    public void SetAttackRadius(float radius)
    {
        foreach (var item in allActiveObjectList)
        {
            item.SetAttackRadius(radius);
        }
    }
    public void SetSlowDownData(float value, float duration)
    {
        foreach (var item in allActiveObjectList)
        {
            item.SetSlowDownData(value, duration);
        }
    }
    public void IncreaseSize(float value)
    {
        foreach (var item in allActiveObjectList)
        {
            item.IncreaseSize(value);
        }
    }
}
