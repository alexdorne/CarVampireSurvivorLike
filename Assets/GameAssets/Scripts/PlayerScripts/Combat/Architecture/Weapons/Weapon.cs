using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponDataSO weaponData;
    private List<StatValue> currentStats = new List<StatValue>();
    private List<WeaponUpgradeSO> appliedUpgrades = new List<WeaponUpgradeSO>();
    [SerializeField] private WeaponBehavior weaponBehavior;

    public void Initialize(WeaponDataSO data) {
        weaponData = data;

        foreach (var stat in weaponData.baseStats) {
            currentStats.Add(new StatValue { statType = stat.statType, baseValue = stat.baseValue });
        }

        Debug.Log($"What is {weaponBehavior}?");

        if (weaponBehavior != null) {
            weaponBehavior.Initialize(this);
        }
    }

    public float GetStat(StatTypeSO statType) {
        var stat = currentStats.Find(s => s.statType == statType);
        return stat?.GetFinalValue() ?? statType.defaultValue;
    }

    public void ApplyUpgrade(WeaponUpgradeSO upgrade) {
        appliedUpgrades.Add(upgrade);
        foreach (var mod in upgrade.statModifiers) {
            ApplyStatModifier(mod); 
        }
    }

    public void ApplyStatModifier(StatModifier modifier) {
        var stat = currentStats.Find(s => s.statType == modifier.statType);
        if (stat != null) {
            stat.AddModifier(modifier);
        }
    }
}
