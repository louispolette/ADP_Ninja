using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();

        _movementStateMachine.Player.SprintAction.started += OnSprintInputPressed;
        _movementStateMachine.Player.CrouchAction.started += OnCrouchInputPressed;
        _movementStateMachine.Player.InteractAction.started += OnInteractInputPressed;
        _movementStateMachine.Player.ToggleMenuAction.started += OnToggleMenuInputPressed;
    }

    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();

        _movementStateMachine.Player.SprintAction.started -= OnSprintInputPressed;
        _movementStateMachine.Player.CrouchAction.started -= OnCrouchInputPressed;
        _movementStateMachine.Player.InteractAction.started -= OnInteractInputPressed;
        _movementStateMachine.Player.ToggleMenuAction.started -= OnToggleMenuInputPressed;
    }

    /// <summary>
    /// Returns the walking, running or sprinting state based on input
    /// </summary>
    protected virtual PlayerGroundedState GetMovingState()
    {
        if (_movementStateMachine.Player.IsHoldingSprintInput && _movementStateMachine.CurrentState != _movementStateMachine.SprintingState)
        {
            return _movementStateMachine.SprintingState;
        }

        if (_movementStateMachine.MovementInput.magnitude >= RunningThreshold)
        {
            return _movementStateMachine.RunningState;
        }
        else
        {
            return _movementStateMachine.WalkState;
        }
    }

    protected virtual void OnSprintInputPressed(InputAction.CallbackContext context)
    {
        if (_movementStateMachine.MovementInput == Vector2.zero) return;

        _movementStateMachine.ChangeState(_movementStateMachine.SprintingState);
    }

    protected virtual void OnCrouchInputPressed(InputAction.CallbackContext context)
    {
        if (_movementStateMachine.MovementInput == Vector2.zero)
        {
            _movementStateMachine.ChangeState(_movementStateMachine.CrouchIdleState);
        }
        else
        {
            _movementStateMachine.ChangeState(_movementStateMachine.CrouchWalkState);
        }
    }

    protected virtual void OnInteractInputPressed(InputAction.CallbackContext context)
    {
        _movementStateMachine.Player.Interact();
    }

    protected virtual void OnToggleMenuInputPressed(InputAction.CallbackContext context)
    {
        _movementStateMachine.Player.OpenMenu();
    }
}
