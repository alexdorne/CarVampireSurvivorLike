using UnityEngine;

public class CarSounds : MonoBehaviour
{
    public float _minSpeed; 
    public float _maxSpeed; 
    private float _currentSpeed; 

    private Rigidbody _carRigidbody;
    private AudioSource _engineAudioSource;

    [SerializeField] private float _minPitch = 0.8f;
    [SerializeField] private float _maxPitch = 2.0f;
    private float _pitchFromCar;

    private void Start() {
        _engineAudioSource = GetComponent<AudioSource>();
        _carRigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        EngineSound();
    }

    private void EngineSound() {
        _currentSpeed = _carRigidbody.linearVelocity.magnitude;
        _pitchFromCar = _carRigidbody.linearVelocity.magnitude / 50f; 

        if (_currentSpeed > _minSpeed && _currentSpeed < _maxSpeed) {
            _engineAudioSource.pitch = _minPitch + _pitchFromCar;
        }
        if (_currentSpeed >= _maxSpeed) {
            _engineAudioSource.pitch = _maxPitch;
        }
    }
}
