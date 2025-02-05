using UnityEngine;

public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    /// <summary>
    /// Called when the player starts moving on the ground. 
    /// Changes the state to walking or running
    /// </summary>
    protected virtual void OnStartMoving()
    {
        if (_movementStateMachine.MovementInput.magnitude >= RunningThreshold)
        {
            _movementStateMachine.ChangeState(_movementStateMachine.RunningState);
        }
        else
        {
            _movementStateMachine.ChangeState(_movementStateMachine.WalkState);
        }
    }
}
