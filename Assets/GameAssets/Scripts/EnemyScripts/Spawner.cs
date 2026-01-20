using Unity.AI.Navigation;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private string zombiePoolName = "Zombies";

    [SerializeField] private NavMeshSurface _navMeshSurface;

    [SerializeField] private float _spawnInterval = 5f;
    private float _spawnTimer;

    private void Awake() {
        
    }

    public void SpawnEnemy(Vector3 position)
    {
        EnemyStats newEnemy = PoolManager.Instance.Get<EnemyStats>(zombiePoolName);

        if (newEnemy != null)
        {
            newEnemy.transform.position = position; 
            newEnemy.transform.rotation = Quaternion.identity;

            newEnemy.Initialize();

            EnemyManager.Instance.RegisterEnemy(newEnemy.gameObject);

        }
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
