using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Game/Weapon")]
public class WeaponDataSO : ScriptableObject
{
    public string weaponName; 
    public WeaponCategory category;
    public GameObject weaponPrefab;

    public List<StatValue> baseStats = new List<StatValue>();

    public List<WeaponUpgradeSO> availableUpgrades = new List<WeaponUpgradeSO>();
}
