using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/EnemyData", order = 1)]
public class EnemyDataSO : ScriptableObject
{
    public string enemyName;
    public int baseMaxHealth;
    public float moveSpeed;
    public int baseDamage;


}
