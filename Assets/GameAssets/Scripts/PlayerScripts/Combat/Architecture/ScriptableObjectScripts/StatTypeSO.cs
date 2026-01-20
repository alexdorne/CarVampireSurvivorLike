using UnityEngine;

[CreateAssetMenu(fileName = "New Stat Type", menuName = "Stats/Stat Type")]
public class StatTypeSO : ScriptableObject
{
    public string statName;
    public string description;
    public float defaultValue;
    public bool isMultiplicative; 
    public StatTypeEnum statCategory;
}
