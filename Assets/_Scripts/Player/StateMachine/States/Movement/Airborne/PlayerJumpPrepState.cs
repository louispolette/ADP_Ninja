using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpPrepState : PlayerGroundedState
{
    public override string Name => "JumpPrep";

    public PlayerJumpPrepState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void OnEnter()
    {
        base.OnEnter();

        _movementStateMachine.HasJumpInputReleasedInJump = _movementStateMachine.Player.JumpAction.WasReleasedThisFrame();

        PlayerAnimationHandler.OnJumpPrepEnd += ChangeToJumpingState;
        SetAnimatorJumpPrep();
    }

    protected override void OnExit()
    {
        base.OnExit();

        PlayerAnimationHandler.OnJumpPrepEnd -= ChangeToJumpingState;
    }

    private void JumpInitialize()
    {
        _movementStateMachine.IsAirborneFromJump = true;
        _movementStateMachine.HasJumpBeenCanceled = false;
    }

    private void ChangeToJumpingState()
    {
        JumpInitialize();
        _movementStateMachine.ChangeState(_movementStateMachine.FallState);
        DoJumpImpulse();
    }

    private void OnJumpInputReleased(InputAction.CallbackContext context)
    {
        _movementStateMachine.HasJumpInputReleasedInJump = true;
    }

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();

        _movementStateMachine.Player.JumpAction.canceled += OnJumpInputReleased;
    }

    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();

        _movementStateMachine.Player.JumpAction.canceled -= OnJumpInputReleased;
    }
}
