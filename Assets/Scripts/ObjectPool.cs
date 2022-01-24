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
        if (poolInactive.Count == 0) return null;

        GameObject obj = poolInactive[0];
        poolInactive.RemoveAt(0);
        
        obj.SetActive(true);
        poolActive.Add(obj);

        return obj;
    }
    
    public void ReturnToPool(GameObject obj)
    {
        if (poolActive.Count >= size)
        {
            Destroy(obj);
            return;
        }

        poolActive.Remove(obj);
        
        obj.SetActive(false);
        poolInactive.Add(obj);
    }
}
