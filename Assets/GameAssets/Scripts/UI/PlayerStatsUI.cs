using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    private void Awake() {
        PlayerHealth.Instance.OnHealthChanged += UpdateHealthUI; 
    }

    public void UpdateHealthUI(int health, int maxHealth) {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

        healthText.text = $"{health}/{maxHealth}"; 
    }
}
