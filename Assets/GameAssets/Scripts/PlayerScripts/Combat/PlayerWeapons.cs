using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] private List<Weapon> allWeapons = new List<Weapon>();
    private List<Weapon> equippedWeapons = new List<Weapon>();
    private List<GlobalUpgradeSO> appliedGlobalUpgrades = new List<GlobalUpgradeSO>();

    private void Awake() {
        foreach (var weapon in allWeapons) {
            EquipWeapon(weapon.weaponData);
        }
    }

    public void EquipWeapon(WeaponDataSO weaponData) {
        Weapon weapon = allWeapons.Find(w => w.weaponData == weaponData);

        if (weapon == null) {
            Debug.LogWarning($"Weapon {weaponData.weaponName} not found in allWeapons list.");
            return; 
        }

        weapon.Initialize(weaponData);
        weapon.gameObject.SetActive(true);
        equippedWeapons.Add(weapon);

        foreach (var globalUpgrade in appliedGlobalUpgrades) {
            ApplyGlobalUpgradeToWeapon(weapon, globalUpgrade);
        }
    }

    public void ApplyGlobalUpgrade(GlobalUpgradeSO upgrade) {
        appliedGlobalUpgrades.Add(upgrade);

        foreach (var weapon in equippedWeapons) {

            if (upgrade.filterByCategory && weapon.weaponData.category != upgrade.categoryFilter) {
                continue; 
            }

            ApplyGlobalUpgradeToWeapon(weapon, upgrade);
        }
    }

    private void ApplyGlobalUpgradeToWeapon(Weapon weapon, GlobalUpgradeSO upgrade) {

        foreach (var mod in upgrade.statModifiers) {
            weapon.ApplyStatModifier(mod);
        }

    }

    public void ApplyWeaponUpgrade(WeaponUpgradeSO upgrade) {
        var weapon = equippedWeapons.Find(w => w.weaponData == upgrade.targetWeapon);
        weapon?.ApplyUpgrade(upgrade);


    }
}
