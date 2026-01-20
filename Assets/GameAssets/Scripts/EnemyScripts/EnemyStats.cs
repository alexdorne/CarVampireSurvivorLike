using UnityEngine;

public class EnemyStats : MonoBehaviour, IPoolable
{
    [SerializeField] private EnemyDataSO enemyData;

    public int CurrentHealth { get; private set; }


    public void Initialize() {
        CurrentHealth = enemyData.baseMaxHealth;
    }

    public void TakeDamage(int damageAmount) {
        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        PoolManager.Instance.Return("Zombies", this); 
    }

    public void OnSpawnFromPool() {
        Initialize();
    }

    public void OnReturnToPool() {
        // Any cleanup logic when returning to pool
    }


}
