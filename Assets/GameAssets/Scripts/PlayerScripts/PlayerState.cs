using UnityEngine;

public class PlayerState : Singleton<PlayerState>
{
    public enum PlayerStates {
        Alive, 
        Dead,
        Paused
    }

    private PlayerStates currentState;

    public PlayerStates GetCurrentState() {
        return currentState;
    }

    public void SetPlayerState(PlayerStates newState) {
        currentState = newState;
    }
}
