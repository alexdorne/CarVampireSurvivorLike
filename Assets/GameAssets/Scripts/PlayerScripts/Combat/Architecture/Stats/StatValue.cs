using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatValue
{
    public StatTypeSO statType; 
    public float baseValue;
    [System.NonSerialized] private List<StatModifier> modifiers = new List<StatModifier>();

    public float GetFinalValue()
    {
        float additive = baseValue;
        float multiplicative = 1f;
        
        foreach (var mod in modifiers)
        {
            if (mod.isMultiplicative)
            {
                multiplicative *= mod.value;
            }
            else
            {
                additive += mod.value;
            }
        }

        return additive * multiplicative;

    }

    public void AddModifier(StatModifier modifier) => modifiers.Add(modifier);
    public void RemoveModifier(StatModifier modifier) => modifiers.Remove(modifier);
    public void ClearModifiers() => modifiers.Clear();
}
