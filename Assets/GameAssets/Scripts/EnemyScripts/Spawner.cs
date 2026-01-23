using Unity.AI.Navigation;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private string zombiePoolName = "Zombies";

    [SerializeField] private NavMeshSurface _navMeshSurface;

    [SerializeField] private float _startingSpawnInterval = 5f;
    [SerializeField] private float _spawnRateMultiplier;
    [SerializeField] private float _spawnCountMultiplier; 
    [SerializeField] private int _startSpawnCount; 

    private float _spawnInterval; 
    private int _numberOfEnemiesToSpawn; 
    private float _spawnTimer;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _minSpawnDistance = 10f;
    [SerializeField] private float _maxSpawnDistance = 30f;

    private void Awake() {
        _spawnInterval = _startingSpawnInterval;
        _numberOfEnemiesToSpawn = _startSpawnCount;
    }

    private void Start() {
        XPSystem.Instance.OnLevelUp += ChangeSpawnInterval; 
    }

    public void SpawnEnemy(Vector3 position) {
        EnemyStats newEnemy = PoolManager.Instance.Get<EnemyStats>(zombiePoolName);

        if (newEnemy != null) {
            newEnemy.transform.position = position;
            newEnemy.transform.rotation = Quaternion.identity;

            newEnemy.Initialize();

            EnemyManager.Instance.RegisterEnemy(newEnemy.gameObject);
        }
    }

    public void ChangeSpawnInterval(int level) {
        _spawnInterval = _startingSpawnInterval / (1 + level * _spawnRateMultiplier);
        _numberOfEnemiesToSpawn = _startSpawnCount + Mathf.RoundToInt(1.0f * _spawnCountMultiplier);
    }

    private Vector3 GetRandomNavMeshPosition() {
        Vector3 spawnPosition = Vector3.zero;
        bool validPositionFound = false;
        int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++) {
            Vector3 randomDirection = Random.insideUnitSphere * _maxSpawnDistance;
            randomDirection += transform.position;
            randomDirection.y = transform.position.y;

            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, _maxSpawnDistance, UnityEngine.AI.NavMesh.AllAreas)) {
                float distanceToPlayer = Vector3.Distance(hit.position, _playerTransform.position);

                if (distanceToPlayer >= _minSpawnDistance && distanceToPlayer <= _maxSpawnDistance) {
                    spawnPosition = hit.position;
                    validPositionFound = true;
                    break;
                }
            }
        }

        return validPositionFound ? spawnPosition : Vector3.zero;
    }

    private void Update() {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0f) {
            for (int i = 0; i < _numberOfEnemiesToSpawn; i++) {
                Vector3 spawnPosition = GetRandomNavMeshPosition();
                if (spawnPosition != Vector3.zero) {
                    SpawnEnemy(spawnPosition);
                }
                _spawnTimer = _spawnInterval;
            }
        }
    }
}
