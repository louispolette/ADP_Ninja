using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouchWalkState : PlayerMovingState
{
    public override string Name => "Crouch Walking";

    public PlayerCrouchWalkState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void OnEnter()
    {
        base.OnEnter();

        _movementStateMachine.SpeedModifier = _movementStateMachine.Player.CrouchSpeedMultiplier;
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
        _movementStateMachine.ChangeState(GetMovingState());
    }

    private void CheckMovementInput()
    {
        if (_movementStateMachine.MovementInput == Vector2.zero)
        {
            _movementStateMachine.ChangeState(_movementStateMachine.CrouchIdleState);
        }
    }
}
