using UnityEngine;
using UnityEngine.AI;
    
public class EnemyNavigation : MonoBehaviour
{
    Transform player;
    [SerializeField] private float stoppingDistance = 1.5f;
    
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private void Awake() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = true;
    }

    private void Update() {
        if (player == null) return;
        
        _navMeshAgent.SetDestination(player.position);
    }

    private void OnAnimatorMove() {
        Vector3 position = _animator.rootPosition;
        _navMeshAgent.nextPosition = position;
        transform.position = position;
        transform.rotation = _animator.rootRotation;
    }
}
