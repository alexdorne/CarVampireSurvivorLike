using System.Collections;
using UnityEngine;

public class ProjectileBehavior : WeaponBehavior
{
    private float attackTimer; 

    [SerializeField] private float minShotAngle = -30f;
    [SerializeField] private float maxShotAngle = 30f;

    public override void Initialize(Weapon weapon) {
        base.Initialize(weapon);
        attackTimer = 0f;

    }
    public override void OnWeaponUpdate() {
        attackTimer -= Time.deltaTime; 

        if (attackTimer <= 0f) {
            Fire(); 
            attackTimer = 1f / GetAttackSpeed();
        }
    }  

    
    public Vector3 GetConstrainedRandomDirection() {
        float randomAngle = Random.Range(minShotAngle, maxShotAngle);
        float randomHeight = Random.Range(-GetProjectileRange() * 0.2f, GetProjectileRange() * 0.2f);
        
        Vector3 baseDirection = transform.forward * GetProjectileRange();
        Vector3 rotated = Quaternion.Euler(randomAngle, 0f, 0f) * baseDirection;
        rotated.y += randomHeight;
        
        return transform.position + rotated;
    }

    public virtual void Fire() {
        // Start Coroutine or logic to fire projectiles
    }

}
