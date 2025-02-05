using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();

        _movementStateMachine.Player.SprintAction.started += OnSprintInputPressed;
    }

    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();

        _movementStateMachine.Player.SprintAction.started -= OnSprintInputPressed;
    }

    /// <summary>
    /// Changes the state to walking or running based on the movement input
    /// </summary>
    protected virtual void StartWalkingOrRunning()
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

    protected virtual void OnSprintInputPressed(InputAction.CallbackContext context)
    {
        if (_movementStateMachine.MovementInput == Vector2.zero) return;

        _movementStateMachine.ChangeState(_movementStateMachine.SprintingState);
    }
}
