using UnityEngine;

public class UpgradeCollider : MonoBehaviour
{
    public bool isGlobalUpgrade;

    public GlobalUpgradeSO globalUpgrade;
    public WeaponUpgradeSO weaponUpgrade;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {

            if (isGlobalUpgrade) {
                other.gameObject.GetComponent<PlayerWeapons>().ApplyGlobalUpgrade(globalUpgrade);
            }
            else {
                other.gameObject.GetComponent<PlayerWeapons>().ApplyWeaponUpgrade(weaponUpgrade);
            }

            gameObject.SetActive(false);

        }
    }
}
