using UnityEngine;

public class XPPickup : MonoBehaviour {
    [SerializeField] private float xpAmount = 10f;
    [SerializeField] private float magnetRadius = 5f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float accelerationRate = 5f;
    [SerializeField] private string poolName = "XPPickups";
    [SerializeField] private float distanceBeforeCollected;

    private Transform player;
    private bool isBeingPulled = false;
    private Vector3 velocity = Vector3.zero;
    private float currentSpeed = 0f;

    private void Update() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= magnetRadius) {
            isBeingPulled = true;
        }

        if (isBeingPulled) {
            float yPosition = transform.position.y;

            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, accelerationRate * Time.deltaTime);

            velocity = Vector3.Slerp(velocity.normalized, directionToPlayer, accelerationRate * Time.deltaTime) * currentSpeed;

            transform.position += velocity * Time.deltaTime;

            transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);

            if (distanceToPlayer <= distanceBeforeCollected) {
                XPSystem.Instance.AddXP(xpAmount);
                PoolManager.Instance.Return(poolName, gameObject);
                isBeingPulled = false;
                currentSpeed = 0f;
            }
        }
    }

    public void Initialize(float xp) {
        xpAmount = xp;
        isBeingPulled = false;
        velocity = Vector3.zero;
        currentSpeed = 0f;
    }
}
