using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonPersistent<PoolManager> 
{
    [SerializeField] private List<PoolConfig> _poolConfigs = new List<PoolConfig>();

    private Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();
    private Transform _poolParent;

    private void Start() {
        _poolParent = new GameObject("PooledObjects").transform;
        _poolParent.SetParent(transform);

        foreach (var config in _poolConfigs) {
            CreatePool(config);
        }
    }

    private void CreatePool(PoolConfig config) {
        if (config.prefab == null) {
            Debug.LogWarning($"PoolManager: Prefab for pool '{config.poolName}' is null. Skipping pool creation.");
            return;
        }

        Transform thisPoolParent = new GameObject($"Pool_{config.poolName}").transform;
        thisPoolParent.SetParent(_poolParent);

        var pool = new ObjectPool(config.prefab, config.initialSize, config.canExpand, thisPoolParent);
        pools[config.poolName] = pool;
    }

    public void CreatePool(string poolName, GameObject prefab, int initialSize = 10, bool canExpand = true) {
        if (pools.ContainsKey(poolName))
        {
            Debug.LogWarning($"Pool {poolName} already exists!");
            return;
        }
        
        Transform thisPoolParent = new GameObject($"Pool_{poolName}").transform;
        thisPoolParent.SetParent(_poolParent);
        
        var pool = new ObjectPool(prefab, initialSize, canExpand, thisPoolParent);
        pools[poolName] = pool;
    }
    public GameObject Get(string poolName)
    {
        if (!pools.ContainsKey(poolName))
        {
            Debug.LogError($"Pool {poolName} does not exist!");
            return null;
        }
        
        return pools[poolName].Get();
    }


    public T Get<T>(string poolName) where T : Component {
        if (!pools.ContainsKey(poolName))
        {
            Debug.LogError($"Pool {poolName} does not exist!");
            return null;
        }
        
        return pools[poolName].Get<T>();
    }
    
    public void Return<T>(string poolName, T obj) where T : Component {
        Return(poolName, obj.gameObject);
    }

    public void Return(string poolName, GameObject obj){
        if (!pools.ContainsKey(poolName))
        {
            Debug.LogError($"Pool {poolName} does not exist!");
            return;
        }
        
        pools[poolName].Return(obj);
    }
    
    public void ReturnAll(string poolName)
    {
        if (!pools.ContainsKey(poolName))
        {
            Debug.LogError($"Pool {poolName} does not exist!");
            return;
        }
        
        var poolObj = pools[poolName];
        var returnAllMethod = poolObj.GetType().GetMethod("ReturnAll");
        returnAllMethod?.Invoke(poolObj, null);
    }
    
    public int CountActive(string poolName)
    {
        if (!pools.ContainsKey(poolName)) return 0;
        return pools[poolName].CountActive();
    }
    
    public int CountInactive(string poolName)
    {
        if (!pools.ContainsKey(poolName)) return 0;
        return pools[poolName].CountInactive();
    }


}


[System.Serializable]
public class PoolConfig 
{
    public string poolName;
    public GameObject prefab; 
    public int initialSize = 100;
    public bool canExpand = true;
}