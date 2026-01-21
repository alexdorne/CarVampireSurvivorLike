using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{
    private GameObject prefab; 
    private Transform parentTransform;
    private Queue<GameObject> availableObjects = new Queue<GameObject>();
    private List<GameObject> allObjects = new List<GameObject>();
    private int initialSize; 
    private bool canExpand; 

    public ObjectPool(GameObject prefab, int initialSize = 10, bool canExpand = true, Transform parent = null)
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

    private GameObject CreateNewObject()
    {
        GameObject newObj = Object.Instantiate(prefab, parentTransform);
        newObj.gameObject.SetActive(false);
        availableObjects.Enqueue(newObj);
        allObjects.Add(newObj);
        return newObj;
    }

    public GameObject Get() {
        GameObject obj; 

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

    public T Get<T>() where T : Component {
        GameObject obj = Get();
        if (obj != null) {
            return obj.GetComponent<T>();
        }
        return null;
    }

    public void Return(GameObject obj) {
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
