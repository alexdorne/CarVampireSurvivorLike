using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonPersistent<PoolManager> 
{
    [SerializeField] private List<PoolConfig> _poolConfigs = new List<PoolConfig>();

    private Dictionary<string, object> pools = new Dictionary<string, object>();
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

        Component component = null;

        if (!string.IsNullOrEmpty(config.componentTypeName)) {
            component = config.prefab.GetComponent(config.componentTypeName);
            if (component == null) {
                Debug.LogError($"Component of type '{config.componentTypeName}' not found on prefab for pool '{config.poolName}'.");
                return;
            }
        }
        else {
            var components = config.prefab.GetComponents<MonoBehaviour>();
            if (components.Length == 0) {
                Debug.LogError($"No MonoBehaviour components found on prefab for pool '{config.poolName}'. Please specify a component type name.");
                return;
            }
        }

        var poolType = typeof(ObjectPool<>).MakeGenericType(component.GetType());
        var pool = System.Activator.CreateInstance(poolType, component, config.initialSize, config.canExpand, thisPoolParent);

        pools[config.poolName] = pool;
    }

    public void CreatePool<T>(string poolName, T prefab, int initialSize = 10, bool canExpand = true) where T : Component {
        if (pools.ContainsKey(poolName))
        {
            Debug.LogWarning($"Pool {poolName} already exists!");
            return;
        }
        
        Transform thisPoolParent = new GameObject($"Pool_{poolName}").transform;
        thisPoolParent.SetParent(_poolParent);
        
        var pool = new ObjectPool<T>(prefab, initialSize, canExpand, thisPoolParent);
        pools[poolName] = pool;
    }
    
    public T Get<T>(string poolName) where T : Component {
        if (!pools.ContainsKey(poolName))
        {
            Debug.LogError($"Pool {poolName} does not exist!");
            return null;
        }
        
        var pool = pools[poolName] as ObjectPool<T>;
        if (pool == null)
        {
            Debug.LogError($"Pool {poolName} is not of type {typeof(T).Name}");
            return null;
        }
        
        return pool.Get();
    }
    
    public void Return<T>(string poolName, T obj) where T : Component {
        if (!pools.ContainsKey(poolName))
        {
            Debug.LogError($"Pool {poolName} does not exist!");
            return;
        }
        
        var pool = pools[poolName] as ObjectPool<T>;
        pool?.Return(obj);
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
        
        var poolObj = pools[poolName];
        var method = poolObj.GetType().GetMethod("CountActive");
        return (int)(method?.Invoke(poolObj, null) ?? 0);
    }
    
    public int CountInactive(string poolName)
    {
        if (!pools.ContainsKey(poolName)) return 0;
        
        var poolObj = pools[poolName];
        var method = poolObj.GetType().GetMethod("CountInactive");
        return (int)(method?.Invoke(poolObj, null) ?? 0);
    }


}


[System.Serializable]
public class PoolConfig 
{
    public string poolName;
    public GameObject prefab; 
    public string componentTypeName; 
    public int initialSize = 100;
    public bool canExpand = true;
}