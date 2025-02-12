using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovingState : PlayerGroundedState
{
    public PlayerMovingState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void OnToggleMenuInputPressed(InputAction.CallbackContext context)
    {
        
    }
}
