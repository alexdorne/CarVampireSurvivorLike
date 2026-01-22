using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Global Upgrade", menuName = "Game/Upgrades/Global Upgrade")]
public class GlobalUpgradeSO : ScriptableObject
{
    public string upgradeName;
    public string upgradedStat; 
    public string description; 
    public Sprite icon; 

    public bool filterByCategory; 
    public WeaponCategory categoryFilter;

    public List<StatModifier> statModifiers = new List<StatModifier>();
}
