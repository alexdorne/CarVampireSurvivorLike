using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeOptionUI : MonoBehaviour
{
    [SerializeField]private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text upgradeText;
    [SerializeField] private TMP_Text upgradeText2; 
    [SerializeField] private Button selectButton;

    private object upgrade; 
    private UpgradeSelectionManager manager; 

    public void Setup(object upgrade, UpgradeSelectionManager manager) {
        this.upgrade = upgrade;
        this.manager = manager;

        if (upgrade is WeaponUpgradeSO weaponUpgrade) {
            icon.sprite = weaponUpgrade.icon;
            nameText.text = weaponUpgrade.upgradeName;
            
            int numberOfUpgrades = weaponUpgrade.statModifiers.Count;

            upgradeText.text = $"{weaponUpgrade.upgradedStat1}: {weaponUpgrade.statModifiers[0].value.ToString()}";

            if (numberOfUpgrades > 1) {
                upgradeText2.text = $"{weaponUpgrade.upgradedStat2}: {weaponUpgrade.statModifiers[1].value.ToString()}"; 
            }
            else {
                upgradeText2.gameObject.SetActive(false);
            }
        }
        else if (upgrade is GlobalUpgradeSO globalUpgrade) {
            icon.sprite= globalUpgrade.icon;
            nameText.text = globalUpgrade.upgradeName;
            upgradeText.text = $"{globalUpgrade.upgradedStat}: {globalUpgrade.statModifiers[0].value.ToString()}"; 

        }

        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(OnSelectClicked);

    }

    private void OnSelectClicked() {
        manager.SelectUpgrade(upgrade); 
    }

}
