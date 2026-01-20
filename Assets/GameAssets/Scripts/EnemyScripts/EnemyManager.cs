using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager> 
{
    private List<GameObject> _activeEnemies = new List<GameObject>();

    [SerializeField] private Transform _playerTransform; 

    public void RegisterEnemy(GameObject enemy)
    {
        _activeEnemies.Add(enemy);
        Debug.Log($"Enemy registered. Total active enemies: {_activeEnemies.Count}");
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        if (_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Remove(enemy);
        }
    }   

    public List<GameObject> GetActiveEnemies() => _activeEnemies;

    public GameObject GetNearestEnemy(Vector3 origin, float maxDistance) {
        var enemies = GetActiveEnemies();
        if (enemies.Count == 0) return null;

        float minDistance = float.MaxValue;

        GameObject nearest = null;

        foreach (var enemy in enemies)
        {
            if (enemy == null || enemy.gameObject.activeSelf == false) continue;

            float distance = Vector3.Distance(origin, enemy.transform.position);
            if (distance <= maxDistance)
            {
                if (nearest == null || distance < minDistance)
                {
                    minDistance = distance;
                    nearest = enemy;
                }
            }
        }

        //Debug.Log($"Nearest enemy found: {nearest?.name ?? "None"}");

        return nearest;
    }
}
