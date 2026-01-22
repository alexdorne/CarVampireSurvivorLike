using System.Collections.Generic;
using UnityEngine;

public class UpgradeSelectionManager : Singleton<UpgradeSelectionManager>
{
    [SerializeField] private PlayerWeapons weapons; 

    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private UpgradeOptionUI[] upgradeOptionSlots;

    [SerializeField] private List<WeaponUpgradeSO> availableWeaponUpgrades = new List<WeaponUpgradeSO>();
    [SerializeField] private List<GlobalUpgradeSO> availableGlobalUpgradeSlots = new List<GlobalUpgradeSO>();

    private bool isSelectingUpgrade = false; 

    private void Awake() {
        upgradePanel.SetActive(false);
    }

    private void Start() {
        XPSystem.Instance.OnLevelUp += ShowUpgradeSelection; 
    }

    private void ShowUpgradeSelection(int level) {
        if (isSelectingUpgrade) return; 

        isSelectingUpgrade=true;
        Time.timeScale = 0f; 

        List<object> selectedUpgrades = GetRandomUpgrades(3); 

        for (int i = 0; i < upgradeOptionSlots.Length; i++) {
            if (i < selectedUpgrades.Count) {
                upgradeOptionSlots[i].Setup(selectedUpgrades[i], this);
                upgradeOptionSlots[i].gameObject.SetActive(true);
            }
            else {
                upgradeOptionSlots[i].gameObject.SetActive(false); 
            }
        }

        upgradePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


    }

    private List<object> GetRandomUpgrades(int count) {
        List<object> allUpgrades = new List<object>();

        if (weapons != null) {
            foreach (var weaponUpgrade in availableWeaponUpgrades) {
                if (weapons.HasWeapon(weaponUpgrade.targetWeapon)) {
                    allUpgrades.Add(weaponUpgrade);
                }
            }
        }

        foreach (var globalUpgrade in availableGlobalUpgradeSlots) {
            allUpgrades.Add(globalUpgrade);
        }

        List<object> selected = new List<object>(); 
        count = Mathf.Min(count, allUpgrades.Count);

        for (int i = 0; i < count; i++) {
            if (allUpgrades.Count == 0) break; 

            int randomIndex = Random.Range(0, allUpgrades.Count);
            selected.Add(allUpgrades[randomIndex]);
            allUpgrades.RemoveAt(randomIndex);
        }

        return selected;
    }

    public void SelectUpgrade(object upgrade) {
        if (upgrade is WeaponUpgradeSO weaponUpgrade) {
            weapons?.ApplyWeaponUpgrade(weaponUpgrade); 
        }
        else if (upgrade is GlobalUpgradeSO globalUpgrade) {
            weapons?.ApplyGlobalUpgrade(globalUpgrade);
        }

        upgradePanel.SetActive(false);
        Cursor.visible = false; 
        Time.timeScale = 1.0f;
        isSelectingUpgrade = false;
    }
}
