using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    public ObjectPool(GameObject obj, int createCount, Transform parent)
    {
        objPrefab = obj;
        this.createCount = createCount;
        this.parent = parent;
    }

    protected int createCount;

    protected GameObject objPrefab;

    protected Transform parent;

    protected Queue<T> objectPool = new Queue<T>();
    public Queue<T> Pool { get => objectPool; }

    public void CreateObject()
    {
        for (int i = 0; i < createCount; i++)
        {
            GameObject o = Object.Instantiate(objPrefab);
            o.SetActive(false);
            o.transform.SetParent(parent);
            o.transform.localPosition = Vector3.zero;
            o.transform.localRotation = Quaternion.identity;

            objectPool.Enqueue(o.GetComponent<T>());
        }
    }
    public bool IsValid()
    {
        if (objectPool.Count <= 0)
        {
            CreateObject();
            return false;
        }
        else return true;
    }
    public T GetObject()
    {
        return objectPool.Dequeue();
    }
    public void ReturnObject(T obj)
    {
        objectPool.Enqueue(obj);
    }
}
