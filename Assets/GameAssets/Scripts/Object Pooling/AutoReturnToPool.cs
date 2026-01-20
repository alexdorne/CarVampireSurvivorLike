using UnityEngine;

public class AutoReturnToPool : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private string poolName; 

    private float timer;

    private void OnEnable() {
        timer = lifetime;
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            ReturnToPool();
        }
    }

    public void ReturnToPool() {
        var poolable = GetComponent<IPoolable>();
        poolable?.OnReturnToPool();

        PoolManager.Instance.Return(poolName, this);
    }
}
