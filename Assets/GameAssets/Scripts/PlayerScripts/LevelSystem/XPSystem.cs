using System;
using UnityEngine;

public class XPSystem : SingletonPersistent<XPSystem> {

    [SerializeField] private float baseXPRequired = 100f; 
    [SerializeField] private float xpScaling = 1.2f;

    private int currentLevel = 1; 
    private float currentXP = 0f;
    private float xpToNextLevel; 

    public event Action<int> OnLevelUp; 
    public event Action<float, float> OnXPChanged;

    public override void Awake() {
        base.Awake();
        xpToNextLevel = baseXPRequired;
    }

    public void AddXP(float amount) {
        currentXP += amount;
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
        while (currentXP >= xpToNextLevel) {
            LevelUp();
        }
    }

    private void LevelUp() {
        currentXP -= xpToNextLevel;
        currentLevel++;

        xpToNextLevel = baseXPRequired * Mathf.Pow(xpScaling, currentLevel - 1);

        OnLevelUp?.Invoke(currentLevel);
        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
    }

    public int GetLevel() => currentLevel;
    public float GetCurrentXP() => currentXP;
    public float GetXPToNextLevel() => xpToNextLevel;
    public float GetXPProgress() => currentXP / xpToNextLevel;
}
