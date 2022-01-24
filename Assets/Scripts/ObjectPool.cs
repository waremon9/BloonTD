using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int size;
    List<GameObject> poolActive = new List<GameObject>();
    List<GameObject> poolInactive = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < size; i++)
        {
            GameObject temp = Instantiate(prefab, this.transform);
            temp.SetActive(false);
            poolInactive.Add(temp);
        }
    }

    public GameObject GetFromPool()
    {
        if (poolActive.Count == 0) return null;

        GameObject obj = poolInactive[0];
        obj.SetActive(true);

        return obj;
    }
    
    public void ReturnToPool(GameObject obj)
    {
        poolActive.Add(obj);
    }
}
