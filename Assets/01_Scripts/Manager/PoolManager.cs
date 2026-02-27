using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class PoolManager : Singleton<PoolManager>
{
    private readonly Dictionary<GameObject, Queue<GameObject>> pools = new();
    private readonly Dictionary<GameObject, GameObject> objToPrefab = new();
    private readonly Dictionary<GameObject, Transform> poolParents = new();

    public GameObject Get(GameObject prefab)
    {
        if (!pools.ContainsKey(prefab))
        {
            GameObject parent = new GameObject(prefab.name + "##POOL");
            parent.transform.SetParent(transform);
            poolParents[prefab] = parent.transform;

            pools.Add(prefab, new Queue<GameObject>());
        }

        if (pools[prefab].Count != 0)
        {
            GameObject obj = pools[prefab].Dequeue();
            obj.SetActive(true);
            objToPrefab[obj] = prefab;

            return obj;
        }
        else
        {
            GameObject obj = Instantiate(prefab, poolParents[prefab], true);
            objToPrefab[obj] = prefab;

            return obj;
        }
    }

    public void Return(GameObject obj)
    {
        GameObject prefab = objToPrefab[obj];
        obj.SetActive(false);
        obj.transform.SetParent(poolParents[prefab]);
        pools[prefab].Enqueue(obj);
    }

    public void Return(GameObject obj, float delay)
    {
        StartCoroutine(DelayedReturn(obj, delay));
    }

    private IEnumerator DelayedReturn(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Return(obj);
    }
}