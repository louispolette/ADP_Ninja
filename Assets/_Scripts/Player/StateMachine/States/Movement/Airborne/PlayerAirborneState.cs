using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerAirborneState : PlayerMovementState
{
    public override string Name => "Airborne";

    public PlayerAirborneState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override float Acceleration => AirAcceleration;
    protected override float Deceleration => AirDeceleration;

    protected override void OnEnter()
    {
        base.OnEnter();

        if (_movementStateMachine.SpeedModifier == 0f)
        {
            _movementStateMachine.SpeedModifier = 1f;
        }

        if (_movementStateMachine.IsGrounded)
        {
            OnEnterAirborneState();
        }

        _movementStateMachine.IsGrounded = false;
        SetAnimatorAirborneState(true);

        if (!_movementStateMachine.HasJumpBeenCanceled && _movementStateMachine.HasJumpInputReleasedInJump)
        {
            CancelJump();
        }
    }

    private void OnEnterAirborneState()
    {
        _movementStateMachine.HasBufferedJumpCancel = false;
        _movementStateMachine.HasReachedJumpCancelThreshold = false;
        _movementStateMachine.CurrentExtraGravity = 0f;
    }

    protected override void OnExit()
    {
        base.OnExit();

        SetAnimatorAirborneState(false);
    }

    protected override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();

        CheckIfJumpCancelThresholdReached();
        TryUseBufferedJumpCancel();
        ApplyExtraGravity();
        CheckIfStillAirborne();
    }

    protected void CheckIfStillAirborne()
    {
        if (_movementStateMachine.Player.Rigidbody.linearVelocity.y >= 0) return;

        if (GroundCheck())
        {
            Land();
            _movementStateMachine.ChangeState(GetGroundedState());
        }
    }

    #region jump cancel
    private void ApplyExtraGravity()
    {
        Vector3 vel = _movementStateMachine.Player.Rigidbody.linearVelocity;
        vel.y -= _movementStateMachine.CurrentExtraGravity;
        _movementStateMachine.Player.Rigidbody.linearVelocity = vel;
    }

    private void CancelJump()
    {
        if (_movementStateMachine.HasJumpBeenCanceled) return;

        if (!_movementStateMachine.HasReachedJumpCancelThreshold &&
            _movementStateMachine.Player.Rigidbody.linearVelocity.y <
            _movementStateMachine.Player.MinimumVelocityForJumpCancel)
        {
            BufferJumpCancel();
            return;
        }

        _movementStateMachine.HasJumpBeenCanceled = true;
        _movementStateMachine.CurrentExtraGravity = JumpCancelExtraGravity;
    }

    private void BufferJumpCancel()
    {
        _movementStateMachine.HasBufferedJumpCancel = true;
    }

    private void TryUseBufferedJumpCancel()
    {
        if (_movementStateMachine.HasJumpBeenCanceled) return;

        if (_movementStateMachine.HasBufferedJumpCancel)
        {
            _movementStateMachine.HasBufferedJumpCancel = false;
            CancelJump();
        }
    }

    private void CheckIfJumpCancelThresholdReached()
    {
        if (_movementStateMachine.Player.Rigidbody.linearVelocity.y >=
            _movementStateMachine.Player.MinimumVelocityForJumpCancel)
        {
            _movementStateMachine.HasReachedJumpCancelThreshold = true;
        }
    }

    #endregion

    #region input

    protected void OnJumpInputReleased(InputAction.CallbackContext context)
    {
        if (_movementStateMachine.IsJumping)
        {
            _movementStateMachine.HasJumpInputReleasedInJump = true;
        }

        CancelJump();
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

#endregion
}
