using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouchIdleState : PlayerGroundedState
{
    public override string Name => "Crouch Idle";

    public PlayerCrouchIdleState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void OnEnter()
    {
        base.OnEnter();

        _movementStateMachine.IsCrouching = true;
        SetAnimatorCrouchedState(true);

        _movementStateMachine.SpeedModifier = 0f;

        ResetHorizontalVelocity();
    }

    protected override void OnExit()
    {
        base.OnExit();

        _movementStateMachine.IsCrouching = false;
        SetAnimatorCrouchedState(false);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        CheckMovementInput();
    }

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();

        _movementStateMachine.Player.CrouchAction.canceled += OnCrouchInputCanceled;
    }

    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();

        _movementStateMachine.Player.CrouchAction.canceled -= OnCrouchInputCanceled;
    }

    private void OnCrouchInputCanceled(InputAction.CallbackContext context)
    {
        _movementStateMachine.ChangeState(_movementStateMachine.IdleState);
    }

    private void CheckMovementInput()
    {
        if (_movementStateMachine.MovementInput != Vector2.zero)
        {
            _movementStateMachine.ChangeState(_movementStateMachine.CrouchWalkState);
        }
    }
}
