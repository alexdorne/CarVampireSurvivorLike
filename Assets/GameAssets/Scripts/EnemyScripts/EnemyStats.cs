using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour, IPoolable
{
    [SerializeField] private EnemyDataSO enemyData;

    [SerializeField] private SkinnedMeshRenderer mesh; 
    [SerializeField] private Material takeDamageMat;
    [SerializeField] private Material baseMat;

    public int attackDamage = 1; 

    public int CurrentHealth { get; private set; }


    public void Initialize() {
        CurrentHealth = enemyData.baseMaxHealth;
    }

    public void TakeDamage(int damageAmount) {
        CurrentHealth -= damageAmount;
        if (gameObject.activeSelf) {
            StartCoroutine(TakeDamageSequence()); 
        }

    }

    private IEnumerator TakeDamageSequence() {
        mesh.material = takeDamageMat;
        if (CurrentHealth <= 0) {
            Die();
            mesh.material = baseMat;
            yield break;
        }
        yield return new WaitForSeconds(0.1f);
        mesh.material = baseMat;
    }

    private void Die() {
        GameObject xpPickup = PoolManager.Instance.Get("XPPickups"); 
        xpPickup.transform.position = transform.position + Vector3.up * 0.5f;
        SessionStats.Instance.AddToKills(1); 
        PoolManager.Instance.Return("Zombies", this); 
    }

    public void OnSpawnFromPool() {
        Initialize();
    }

    public void OnReturnToPool() {
        // Any cleanup logic when returning to pool
    }


}
