using UnityEngine;

public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected virtual void OnStartMove()
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
