using DG.Tweening;
using UnityEngine;

public class XPPickup : MonoBehaviour {
    [SerializeField] private float xpAmount = 10f;
    [SerializeField] private float magnetRadius = 5f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float accelerationRate = 5f;
    [SerializeField] private string poolName = "XPPickups";
    [SerializeField] private float distanceBeforeCollected;

    [Header("Animation")]
    [SerializeField] private float cycleLength = 2f;
    [SerializeField] private float moveDistance = 0.1f;

    private Transform player;
    private bool isBeingPulled = false;
    private Vector3 velocity = Vector3.zero;
    private float currentSpeed = 0f;

    private Tween moveTween;
    private Tween rotateTween;

    private void Start() {
        moveTween = transform.DOMove(new Vector3(transform.position.x, transform.position.y + moveDistance, transform.position.z), cycleLength).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        rotateTween = transform.DORotate(new Vector3(0, 360, 0), cycleLength * 2, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    private void Update() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= magnetRadius) {
            isBeingPulled = true;
            StopAnimations();
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

    private void StopAnimations() {
        moveTween?.Kill();
        rotateTween?.Kill();
    }

    public void Initialize(float xp) {
        xpAmount = xp;
        isBeingPulled = false;
        velocity = Vector3.zero;
        currentSpeed = 0f;
    }
}
