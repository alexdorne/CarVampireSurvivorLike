using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text killCountText; 

    private void Awake() {
        PlayerHealth.Instance.OnHealthChanged += UpdateHealthUI; 
        SessionStats.Instance.OnKillsChange += UpdateKillCountText; 
    }

    public void UpdateHealthUI(int health, int maxHealth) {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

        healthText.text = $"{health}/{maxHealth}"; 
    }

    public void UpdateKillCountText(int kills) {
        killCountText.text = kills.ToString();
    }
}
