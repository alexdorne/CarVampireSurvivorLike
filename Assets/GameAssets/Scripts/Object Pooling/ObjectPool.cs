using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> where T : Component
{
    private T prefab; 
    private Transform parentTransform;
    private Queue<T> availableObjects = new Queue<T>();
    private List<T> allObjects = new List<T>();
    private int initialSize; 
    private bool canExpand; 

    public ObjectPool(T prefab, int initialSize = 10, bool canExpand = true, Transform parent = null)
    {
        this.prefab = prefab;
        this.initialSize = initialSize;
        this.canExpand = canExpand;
        this.parentTransform = parent;


        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private T CreateNewObject()
    {
        T newObj = Object.Instantiate(prefab, parentTransform);
        newObj.gameObject.SetActive(false);
        availableObjects.Enqueue(newObj);
        allObjects.Add(newObj);
        return newObj;
    }

    public T Get() {
        T obj; 

        if (availableObjects.Count > 0)
        {
            obj = availableObjects.Dequeue();
        }
        else if (canExpand)
        {
            obj = CreateNewObject();
            availableObjects.Dequeue();
        }
        else
        {
            Debug.LogWarning($"ObjectPool {prefab.name} is empty and cannot expand.");
            return null; 
        }

        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj) {
        if (obj == null || !allObjects.Contains(obj)) {
            Debug.LogWarning("Trying to return an object that doesn't belong to this pool.");
            return;
        }

        obj.gameObject.SetActive(false);

        if (parentTransform != null) {
            obj.transform.SetParent(parentTransform);
        }

        availableObjects.Enqueue(obj);
    }

    public void ReturnAll() {
        foreach(var obj in allObjects) {
            if (obj.gameObject.activeSelf) {
                obj.gameObject.SetActive(false);
                availableObjects.Enqueue(obj);
            }
        }
    }

    public int CountActive() {
        return allObjects.Count - availableObjects.Count;
    }

    public int CountInactive() {
        return availableObjects.Count;
    }   

    public int CountTotal() { 
        return allObjects.Count;
    }   

}
