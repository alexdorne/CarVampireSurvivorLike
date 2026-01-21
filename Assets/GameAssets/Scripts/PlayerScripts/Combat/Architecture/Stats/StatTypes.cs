using System.Collections.Generic;
using UnityEngine;

public class StatTypes : Singleton<StatTypes>
{
    private List<StatTypeSO> allStatTypes = new List<StatTypeSO>();

    private void Awake() {
        LoadStatTypes();
    }

    private void LoadStatTypes() {
        allStatTypes.Clear(); 
        StatTypeSO[] statTypes = Resources.LoadAll<StatTypeSO>("ScriptableObjects/StatTypes");
        allStatTypes.AddRange(statTypes);
    }

    public StatTypeSO GetStatTypeSO(StatTypeEnum name) {

        foreach (var statType in allStatTypes) {
            if (statType.statName == name.ToString()) {
                return statType;
            }
        }
        Debug.LogError($"StatTypeSO with name {name} not found!");
        return null;
    }
}

public enum StatTypeEnum
{
    AttackSpeed,
    AttackDamage,
    CriticalChance,
    CriticalDamage,
    ProjectileCount,
    ProjectileRange,
    Size,
    Knockback,
    ProjectileSpeed
}   
