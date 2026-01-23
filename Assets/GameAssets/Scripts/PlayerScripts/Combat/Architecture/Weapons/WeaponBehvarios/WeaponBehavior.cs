using UnityEngine;

public abstract class WeaponBehavior : MonoBehaviour
{
    protected Weapon weapon;

    public virtual void Initialize(Weapon weapon)
    {
        this.weapon = weapon;

    }

    private void Update() {

        if (PlayerState.Instance.GetCurrentState() == PlayerState.PlayerStates.Alive) { 
            OnWeaponUpdate();                
        }
    }

    public void HitEnemy(GameObject enemy, float damage, Vector3 knockback) {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(Mathf.RoundToInt(damage));
            
            enemy.transform.position += knockback;
        }
    }



    public abstract void OnWeaponUpdate();  

    private float GetWeaponStat(StatTypeEnum statType) => weapon.GetStat(StatTypes.Instance.GetStatTypeSO(statType));

    protected float GetAttackSpeed() => GetWeaponStat(StatTypeEnum.AttackSpeed);
    protected float GetAttackDamage() => GetWeaponStat(StatTypeEnum.AttackDamage);
    protected float GetCriticalChance() => GetWeaponStat(StatTypeEnum.CriticalChance);
    protected float GetCriticalDamage() => GetWeaponStat(StatTypeEnum.CriticalDamage);
    protected int GetProjectileCount() => (int)GetWeaponStat(StatTypeEnum.ProjectileCount);
    protected float GetProjectileRange() => GetWeaponStat(StatTypeEnum.ProjectileRange);
    protected float GetSize() => GetWeaponStat(StatTypeEnum.Size);   
    protected float GetKnockback() => GetWeaponStat(StatTypeEnum.Knockback);
    protected float GetProjectileSpeed() => GetWeaponStat(StatTypeEnum.ProjectileSpeed);




}
