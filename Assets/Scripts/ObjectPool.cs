    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [HideInInspector] public GameObject prefab;
    public int size;
    List<GameObject> poolInactive = new List<GameObject>();

    public void Initialize(GameObject obj)
    {
        prefab = obj;
        for (int i = 0; i < size; i++)
        {
            GameObject temp = Instantiate(prefab, transform);
            temp.SetActive(false);
            poolInactive.Add(temp);
        }
    }

    public GameObject GetFromPool()
    {
        if (poolInactive.Count == 0) return Instantiate(prefab, transform);

        GameObject obj = poolInactive[0];
        poolInactive.RemoveAt(0);
        
        obj.SetActive(true);

        return obj;
    }
    
    public void ReturnToPool(GameObject obj)
    {
        if (poolInactive.Count >= size)
        {
            Destroy(obj);
            return;
        }
        
        obj.SetActive(false);
        poolInactive.Add(obj);
    }
}
