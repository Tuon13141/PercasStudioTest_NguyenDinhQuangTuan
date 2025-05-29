using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> poolQueue = new Queue<T>();
    private T prefab;
    private Transform parent;

    public ObjectPool(T prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
    }

    public T Get()
    {
        if (poolQueue.Count > 0)
        {
            T obj = poolQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            T obj = GameObject.Instantiate(prefab, parent);
            return obj;
        }
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        poolQueue.Enqueue(obj);
    }

    public void ReturnAllToPool(IEnumerable<T> activeObjects)
    {
        foreach (var obj in activeObjects)
        {
            Return(obj);
        }
    }
}
