using System.Collections;
using UnityEngine;

public class GunBehavior : ProjectileBehavior
{
    //[SerializeField] private LineRenderer shotLine;
    [SerializeField] private float minShotAngle = -30f;
    [SerializeField] private float maxShotAngle = 30f;


    

    public override void Fire() {
        StartCoroutine(OnFire());
    }


    private IEnumerator OnFire() {
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

        GameObject shotLine = PoolManager.Instance.Get("GunShots");

        LineRenderer lineRenderer = shotLine.GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.enabled = true;
        StartCoroutine(HideShotLineAfterDelay(0.5f, lineRenderer));


    }

    private IEnumerator HideShotLineAfterDelay(float delay, LineRenderer lineRenderer) {
        yield return new WaitForSeconds(delay);
        lineRenderer.enabled = false;
        PoolManager.Instance.Return("GunShots", lineRenderer.gameObject); 

    }


}
