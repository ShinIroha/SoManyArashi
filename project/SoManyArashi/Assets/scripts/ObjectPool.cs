using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ObjectPool
{
    Stack<GameObject> objects = new Stack<GameObject>();
    GameObject prefab;
    int step;

    static Dictionary<string, ObjectPool> existPools = new Dictionary<string, ObjectPool>();

    public static ObjectPool GetObjectPool(string prefabPath, int initialSize = 10, int step = 10)
    {
        if (initialSize < 1)
            initialSize = 1;
        if (step < 1)
            step = 1;
        if (existPools.ContainsKey(prefabPath))
        {
            return existPools[prefabPath];
        }
        else
        {
            return new ObjectPool(prefabPath, initialSize, step);
        }
    }

    public ObjectPool(string prefabPath, int initialSize, int step)
    {
        prefab = Resources.Load<GameObject>(prefabPath);
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab);
            obj.GetComponent<ObjectPoolRestorer>().sourcePool = this;
            objects.Push(obj);
        }
        this.step = step;
    }

    public GameObject New()
    {
        GameObject obj;
        if (objects.Count == 0)
        {
            for (int i = 0; i < step; i++)
            {
                obj = GameObject.Instantiate(prefab);
                obj.GetComponent<ObjectPoolRestorer>().sourcePool = this;
                objects.Push(obj);
            }
        }
        obj = objects.Pop();
        return obj;
    }

    public GameObject New(Vector3 position)
    {
        GameObject obj;
        if (objects.Count == 0)
        {
            for (int i = 0; i < step; i++)
            {
                obj = GameObject.Instantiate(prefab);
                obj.GetComponent<ObjectPoolRestorer>().sourcePool = this;
                objects.Push(obj);
            }
        }
        obj = objects.Pop();
        obj.transform.position = position;
        return obj;
    }

    public GameObject New(Vector3 position,Quaternion rotation)
    {
        GameObject obj;
        if (objects.Count == 0)
        {
            for (int i = 0; i < step; i++)
            {
                obj = GameObject.Instantiate(prefab);
                obj.GetComponent<ObjectPoolRestorer>().sourcePool = this;
                objects.Push(obj);
            }
        }
        obj = objects.Pop();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public void Store(GameObject obj)
    {
        objects.Push(obj);
    }
}
