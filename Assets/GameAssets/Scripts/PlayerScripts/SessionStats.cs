using System;
using UnityEngine;

public class SessionStats : Singleton<SessionStats>
{
    private int kills; 

    public event Action<int> OnKillsChange; 

    public void AddToKills(int count) {
        kills += count; 
        OnKillsChange.Invoke(kills);
    }
}
