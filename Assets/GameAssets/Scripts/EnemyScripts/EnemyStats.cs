using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private EnemyDataSO enemyData;

    public int CurrentHealth { get; private set; }

    private void Start() {
        CurrentHealth = enemyData.baseMaxHealth;
    }

    public void TakeDamage(int damageAmount) {
        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        gameObject.SetActive(false);
    }


}
