using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Weapon Upgrade", menuName = "Game/Upgrades/Weapon Upgrade")]
public class WeaponUpgradeSO : ScriptableObject
{
    public string upgradeName; 
    public string upgradedStat1; 
    public string upgradedStat2;
    public string description;
    public Sprite icon; 

    public WeaponDataSO targetWeapon; 

    public List<StatModifier> statModifiers = new List<StatModifier>();

    public bool hasCustomBehavior; 
    public string behaviorScriptName;
}
