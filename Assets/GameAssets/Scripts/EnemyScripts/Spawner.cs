using Unity.AI.Navigation;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] private NavMeshSurface _navMeshSurface;

    [SerializeField] private float _spawnInterval = 5f;
    private float _spawnTimer;

    public void SpawnEnemy(Vector3 position)
    {
        GameObject newEnemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        EnemyManager.Instance.RegisterEnemy(newEnemy);
    }

    private Vector3 GetRandomNavMeshPosition() {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        UnityEngine.AI.NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, 10f, UnityEngine.AI.NavMesh.AllAreas)) {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    private void Update() {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0f) {
            Vector3 spawnPosition = GetRandomNavMeshPosition();
            SpawnEnemy(spawnPosition);
            _spawnTimer = _spawnInterval;
        }

    }
}
