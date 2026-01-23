using UnityEngine;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    InputSystem_Actions inputActions;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool IsAccelerating { get; private set; }
    public bool IsReversing { get; private set; }
    public bool IsBraking { get; private set; }

    private void Awake() {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    private void Update() { 
        MoveInput = inputActions.Player.Move.ReadValue<Vector2>();
        LookInput = inputActions.Player.Look.ReadValue<Vector2>();
        IsAccelerating = inputActions.Player.Accelerate.IsPressed();
        IsReversing = inputActions.Player.Reverse.IsPressed();
        IsBraking = inputActions.Player.Brake.IsPressed();
    }

    private void OnDestroy() {
        inputActions.Player.Disable();
        inputActions.Dispose(); 
    }

}
