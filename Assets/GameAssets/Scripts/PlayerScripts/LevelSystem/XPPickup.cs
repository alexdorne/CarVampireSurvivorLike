using UnityEngine;

public class XPPickup : MonoBehaviour
{
    [SerializeField] private float xpAmount = 10f;
    [SerializeField] private float magnetRadius = 5f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private string poolName = "XPPickups";

    private Transform player; 
    private bool isBeingPulled = false;

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
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if (distanceToPlayer <= 0.5f) {
                XPSystem.Instance.AddXP(xpAmount);
                PoolManager.Instance.Return(poolName, gameObject);
                isBeingPulled = false;
            }
        }
    }

    public void Initialize(float xp) {
        xpAmount = xp;
        isBeingPulled = false;
    }

}
