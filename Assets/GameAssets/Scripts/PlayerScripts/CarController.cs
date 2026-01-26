using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private InputManager _inputManager; 

    public enum Axle{
        Front,
        Rear
    }

    [System.Serializable]
    public struct Wheel{
        public GameObject wheelModel; 
        public WheelCollider wheelCollider;
        public Axle axle;
    }

    [SerializeField] private float maxAcceleration = 18000f; 
    [SerializeField] private float brakeAcceleration = 200000f;

    [SerializeField] private float maxSpeed = 80.0f; 

    [SerializeField] private float turnSensitivity = 1.5f;
    [SerializeField] private float maxSteerAngle = 30.0f;
    
    [SerializeField] private float steerSmoothness = 0.1f;
    
    [SerializeField] private float coastingFriction = 200.0f; 

    public Vector3 centerOfMass; 

    public List<Wheel> wheels;

    private Rigidbody _carRigidbody;

    private void Awake() {
        _inputManager = InputManager.Instance;
        _carRigidbody = GetComponent<Rigidbody>();
        _carRigidbody.centerOfMass = centerOfMass;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        AnimateWheels();
    }

    private void FixedUpdate() {
        if (PlayerState.Instance.GetCurrentState() == PlayerState.PlayerStates.Paused)
            return; 

        if (PlayerState.Instance.GetCurrentState() == PlayerState.PlayerStates.Alive) {
            Brake();
            Move(); 
            LimitSpeed();
            Steer();
        }

        CoastingBrake();

    }

    private void Move() {
        float moveInput = NormalizeInput(_inputManager.MoveInput.y);
        foreach (var wheel in wheels) {

            if (wheel.axle == Axle.Rear) { 
                if (moveInput != 0) {
                    wheel.wheelCollider.motorTorque = moveInput * maxAcceleration;
                }
                else {
                    wheel.wheelCollider.motorTorque = 0;
                }
            }
        }
    }

    private float NormalizeInput(float input) {
        if (input > 0) return 1f; 
        if (input < 0) return -1f;
        return 0f; 
    }

    private void Steer() {
        foreach(var wheel in wheels) {
            if (wheel.axle == Axle.Front) {
                float steerAngle = _inputManager.MoveInput.x * maxSteerAngle * turnSensitivity;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, steerSmoothness);
            }
        }
    }
    
    public void Brake() {
        if (_inputManager.IsBraking) {
            foreach (var wheel in wheels) {
                wheel.wheelCollider.brakeTorque = brakeAcceleration;
            }
        }
        else {
            foreach (var wheel in wheels) {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }


        if (IsInputOppositeToMovement()) {
            foreach (var wheel in wheels) {
                wheel.wheelCollider.brakeTorque = brakeAcceleration;
            }
        }

        
    }

    public void CoastingBrake() {
        if (_inputManager.MoveInput.y == 0 && _carRigidbody.linearVelocity.magnitude > 0) {
            foreach (var wheel in wheels) {
                wheel.wheelCollider.brakeTorque = coastingFriction;
            }
        }
    }
    
    private void LimitSpeed() {
        float maxSpeedAdjusted = (maxSpeed * 1000.0f) / 3600.0f;

        if (_carRigidbody.linearVelocity.magnitude > maxSpeedAdjusted){
            Vector3 targetVelocity = _carRigidbody.linearVelocity.normalized * maxSpeedAdjusted;
            _carRigidbody.linearVelocity = Vector3.Lerp(_carRigidbody.linearVelocity, targetVelocity, 0.2f);
        }
    }

    private void AnimateWheels() {
        foreach (var wheel in wheels) {
            Quaternion quat;
            Vector3 position;
            wheel.wheelCollider.GetWorldPose(out position, out quat);
            wheel.wheelModel.transform.position = position;
            wheel.wheelModel.transform.rotation = quat;
        }
    }

    private bool IsInputOppositeToMovement() {
        float moveInput = NormalizeInput(_inputManager.MoveInput.y);
        if (moveInput == 0f) return false;
        if (_carRigidbody.linearVelocity.magnitude < 0.1f) return false;
        Vector3 carForward = transform.forward;
        Vector3 velocityDirection = _carRigidbody.linearVelocity.normalized;
        float forwardDot = Vector3.Dot(velocityDirection, carForward);        
        return (forwardDot > 0 && moveInput < 0) || (forwardDot < 0 && moveInput > 0);
    }
}
