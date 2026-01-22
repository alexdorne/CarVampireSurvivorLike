using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TMP_Text levelText;

    private void Start() {
        XPSystem.Instance.OnXPChanged += UpdateXPSlider;
        XPSystem.Instance.OnLevelUp += UpdateLevelText;
    }

    private void UpdateXPSlider(float currentXP, float XPToNextLevel) {
        xpSlider.maxValue = XPToNextLevel;
        xpSlider.value = currentXP;
    }

    private void UpdateLevelText(int level) {
        if (levelText != null) {
            levelText.text = $"Level {level}";
        }
    }
}
