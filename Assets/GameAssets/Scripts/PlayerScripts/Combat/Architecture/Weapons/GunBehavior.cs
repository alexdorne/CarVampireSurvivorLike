using System.Collections;
using UnityEngine;

public class GunBehavior : WeaponBehavior
{
    [SerializeField] private LineRenderer shotLine;
    [SerializeField] private float minShotAngle = -30f;
    [SerializeField] private float maxShotAngle = 30f;
    private float attackTimer; 


    public override void Initialize(Weapon weapon) {
        base.Initialize(weapon);
        attackTimer = 0f;

    }

    public override void OnWeaponUpdate() {
        attackTimer -= Time.deltaTime; 

        if (attackTimer <= 0f) {
            StartCoroutine(Fire()); 
            attackTimer = 1f / GetAttackSpeed();
        }
    }

    

    private IEnumerator Fire() {
        int projectileCount = GetProjectileCount();
        float attackDuration = 1f/GetAttackSpeed() * 0.5f;

        Debug.Log($"Attacking with {projectileCount} projectiles over {attackDuration} seconds.");

        for (int i = 0; i < projectileCount; i++) {
            GameObject enemyToShoot = EnemyManager.Instance.GetNearestEnemy(transform.position, GetProjectileRange());

            Vector3 shotDirection; 

            if (enemyToShoot != null) {
                Debug.Log($"Shooting at enemy: {enemyToShoot?.name ?? "None"} situated at {enemyToShoot.transform.position} ");

                shotDirection = enemyToShoot.transform.position;
            }
            else {
                shotDirection = GetConstrainedRandomDirection();
            }

            AudioManager.Instance.PlaySound("GunShot", true);

            ShowShotLine(transform.position, shotDirection);

            Vector3 knockbackDirection;

            if (enemyToShoot != null) {
                knockbackDirection = (enemyToShoot.transform.position - transform.position).normalized;
                HitEnemy(enemyToShoot, GetAttackDamage(), knockbackDirection * GetKnockback());
            }


            yield return new WaitForSeconds(attackDuration / projectileCount);
        }

    }

    private Vector3 GetConstrainedRandomDirection() {
        float randomAngle = Random.Range(minShotAngle, maxShotAngle);
        float randomHeight = Random.Range(-GetProjectileRange() * 0.2f, GetProjectileRange() * 0.2f);
        
        Vector3 baseDirection = transform.forward * GetProjectileRange();
        Vector3 rotated = Quaternion.Euler(randomAngle, 0f, 0f) * baseDirection;
        rotated.y += randomHeight;
        
        return transform.position + rotated;
    }

    private void ShowShotLine(Vector3 start, Vector3 end) {
        shotLine.positionCount = 2;
        shotLine.SetPosition(0, start);
        shotLine.SetPosition(1, end);
        shotLine.enabled = true;
        StartCoroutine(HideShotLineAfterDelay(0.5f));
    }

    private IEnumerator HideShotLineAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        shotLine.enabled = false;
    }


}
