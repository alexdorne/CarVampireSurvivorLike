using Unity.Cinemachine;
using Unity.Cinemachine.TargetTracking;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private CinemachineOrbitalFollow _orbitalFollow;
    private InputManager _inputManager;

    private float _inputTimer;
    [SerializeField] private float _delayBeforeRecentering = 2f;

    private void Awake() {
        _orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
        _inputManager = InputManager.Instance;
    }

    private void Update() {
        if (_inputManager.LookInput != Vector2.zero) {
            _inputTimer = 0f;
            //_orbitalFollow.TrackerSettings.BindingMode = BindingMode.LockToTarget;
            

            
        } else {
            _inputTimer += Time.deltaTime;
            if (_inputTimer >= _delayBeforeRecentering) {
                //_orbitalFollow.TrackerSettings.BindingMode = BindingMode.LazyFollow;
            }
        }
    }


}
