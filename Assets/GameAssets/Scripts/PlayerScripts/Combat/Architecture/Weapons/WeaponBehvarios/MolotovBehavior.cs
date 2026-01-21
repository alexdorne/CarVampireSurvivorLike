using System.Collections;
using UnityEngine;

public class MolotovBehavior : ProjectileBehavior
{
    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private float gravityScale = 1f;

    public override void Fire()
    {
        StartCoroutine(OnFire());
    }

    private IEnumerator OnFire()
    {
        int projectileCount = GetProjectileCount();
        float attackDuration = 1f / GetAttackSpeed() * 0.5f;
        float delayBetweenShots = attackDuration / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            GameObject molotov = PoolManager.Instance.Get("MolotovCocktails");

            molotov.transform.position = transform.position + transform.forward * 1f + Vector3.up * 1f;

            Rigidbody rb = molotov.GetComponent<Rigidbody>();

            GameObject enemyToShoot = EnemyManager.Instance.GetNearestEnemy(transform.position, GetProjectileRange());

            rb.linearVelocity = Vector3.zero;

            if (enemyToShoot != null)
            {
                Vector3 targetPos = enemyToShoot.transform.position;
                Vector3 launchPos = molotov.transform.position;
                Vector3 launchVelocity = CalculateProjectileVelocity(launchPos, targetPos, launchAngle);
                rb.linearVelocity = launchVelocity;
            }
            else
            {
                Vector3 forwardDirection = transform.forward.normalized;
                rb.linearVelocity = forwardDirection * 10f;
            }

            yield return new WaitForSeconds(delayBetweenShots);
        }
    }

    private Vector3 CalculateProjectileVelocity(Vector3 startPos, Vector3 targetPos, float angleInDegrees)
    {
        float gravity = Physics.gravity.magnitude * gravityScale;
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        
        Vector3 displacement = targetPos - startPos;
        float horizontalDistance = new Vector3(displacement.x, 0, displacement.z).magnitude;
        float verticalDistance = displacement.y;

        // Calculate required velocity magnitude
        float sinTheta = Mathf.Sin(angleInRadians);
        float cosTheta = Mathf.Cos(angleInRadians);
        float tanTheta = Mathf.Tan(angleInRadians);

        float velocityMagnitude = Mathf.Sqrt(
            (gravity * horizontalDistance * horizontalDistance) / 
            (2 * cosTheta * cosTheta * (horizontalDistance * tanTheta - verticalDistance))
        );

        // Calculate direction and apply velocity
        Vector3 horizontalDirection = new Vector3(displacement.x, 0, displacement.z).normalized;
        Vector3 launchVelocity = (horizontalDirection * velocityMagnitude * cosTheta) + 
                                  (Vector3.up * velocityMagnitude * sinTheta);

        return launchVelocity;
    }
}
