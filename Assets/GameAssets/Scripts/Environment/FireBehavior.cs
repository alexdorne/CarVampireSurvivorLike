using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehavior : MonoBehaviour
{
    List<GameObject> entitiesInFire = new List<GameObject>();

    [SerializeField] private float burnInterval = 1.0f;

    private float burnTimer = 1f;

    [SerializeField] private float fireDuration = 3.0f;
    private void OnEnable() {
        StartCoroutine(FireLifetime());
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            if (!entitiesInFire.Contains(other.gameObject)) {
                entitiesInFire.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            if (entitiesInFire.Contains(other.gameObject)) {
                entitiesInFire.Remove(other.gameObject);
            }
        }
    }

    private void Update() {
        if (entitiesInFire.Count != 0) {
            burnTimer += Time.deltaTime;

            if (burnTimer >= burnInterval) {
                Burn();
                burnTimer = 0f;
            }
        }
        else {
            burnTimer = 1f;
        }
    }

    private void Burn() {
        foreach (GameObject entity in entitiesInFire) {
            EnemyStats enemyStats = entity.GetComponent<EnemyStats>();
            if (enemyStats != null) {
                enemyStats.TakeDamage(1); 
            }
        }
    }

    private IEnumerator FireLifetime() {
        yield return new WaitForSeconds(fireDuration);
        entitiesInFire.Clear();
        burnTimer = 1f;
        PoolManager.Instance.Return("Fire", this.gameObject);
    }
}
