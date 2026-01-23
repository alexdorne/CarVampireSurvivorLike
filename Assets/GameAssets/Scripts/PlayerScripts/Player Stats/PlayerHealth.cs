using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public int CurrentHealth {  get; private set; }

    public int MaxHealth { get; private set; }

    [SerializeField] private int startingMaxHealth;

    [SerializeField] private float damageCooldown = 0.5f;

    [SerializeField] private GameObject deathScreen; 

    private bool canTakeDamage = true;

    public event Action<int, int> OnHealthChanged; 

    private void Awake() {
        PlayerState.Instance.SetPlayerState(PlayerState.PlayerStates.Alive);
        MaxHealth = startingMaxHealth;
        CurrentHealth = MaxHealth;
    }

    private void Start() {
        OnHealthChanged.Invoke(CurrentHealth, MaxHealth); 
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy") && canTakeDamage) {
            EnemyStats enemyStats = collision.gameObject.GetComponent<EnemyStats>();
            StartCoroutine(TakeDamage(enemyStats.attackDamage)); 
        }
    }

    private IEnumerator TakeDamage(int damage) {
        canTakeDamage = false;
        CurrentHealth -= damage;

        if (CurrentHealth <= 0) {
            Death(); 
        }

        OnHealthChanged.Invoke(CurrentHealth, MaxHealth); 

        yield return new WaitForSeconds(damageCooldown);

        canTakeDamage = true; 
    }

    private void Death() {
        PlayerState.Instance.SetPlayerState(PlayerState.PlayerStates.Dead);
        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
