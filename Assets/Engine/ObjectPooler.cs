using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    public static ObjectPooler current_pooler = null;

    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledObjects;

    void Awake()
    {
        if (current_pooler == null)
            current_pooler = this;
        else
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = Instantiate(item.objectToPool);
                obj.transform.SetParent(transform, true);
                obj.name = item.nametag;
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string name, Transform parent = null)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].name == name)
            {
                if (parent != null) {
                    pooledObjects[i].transform.SetParent(parent);
                    pooledObjects[i].transform.localPosition = Vector3.zero;
                }
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.name == name)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    obj.transform.SetParent(transform, true);
                    obj.name = item.nametag;
                    pooledObjects.Add(obj);
                    if (parent != null)
                    {
                        obj.transform.SetParent(parent, false);
                        obj.transform.localPosition = Vector3.zero;
                    }
                    return obj;
                }
            }
        }
        return null;
    }
}

[System.Serializable]
public class ObjectPoolItem
{
    public string nametag;
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand;
}
