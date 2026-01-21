using System.Collections;
using UnityEngine;

public class ProjectileBehavior : WeaponBehavior
{
    private float attackTimer; 

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

    public virtual void Fire() {
        // Start Coroutine or logic to fire projectiles
    }

}
