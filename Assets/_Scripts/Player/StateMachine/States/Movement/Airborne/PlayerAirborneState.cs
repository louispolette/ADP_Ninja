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
            Debug.Log("Immediate Cancel");
            CancelJump();
        }
    }

    private void OnEnterAirborneState()
    {
        _movementStateMachine.HasBufferedJumpCancel = false;
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

        CheckForBufferedJumpCancel();
        ApplyExtraGravity();
        CheckIfStillAirborne();
    }

    private void ApplyExtraGravity()
    {
        Vector3 vel = _movementStateMachine.Player.Rigidbody.linearVelocity;
        vel.y -= _movementStateMachine.CurrentExtraGravity;
        _movementStateMachine.Player.Rigidbody.linearVelocity = vel;
    }

    protected void CheckIfStillAirborne()
    {
        if (_movementStateMachine.Player.Rigidbody.linearVelocity.y >= 0) return;

        if (GroundCheck())
        {
            Land();
            _movementStateMachine.ChangeState(GetGroundedState());
            _movementStateMachine.IsAirborneFromJump = false;
        }
    }

    private void CancelJump()
    {
        if (_movementStateMachine.HasJumpBeenCanceled) return;

        if (_movementStateMachine.Player.Rigidbody.linearVelocity.y <
            _movementStateMachine.Player.MinimumVelocityForJumpCancel)
        {
            BufferJumpCancel();
            return;
        }

        Debug.Log("Jump Canceled");
        _movementStateMachine.HasJumpBeenCanceled = true;
        _movementStateMachine.CurrentExtraGravity = JumpCancelExtraGravity;
    }

    private void BufferJumpCancel()
    {
        Debug.Log($"Jump Buffered because velocity was only {_movementStateMachine.Player.Rigidbody.linearVelocity.y}");
        _movementStateMachine.HasBufferedJumpCancel = true;
    }

    private void CheckForBufferedJumpCancel()
    {
        if (_movementStateMachine.HasJumpBeenCanceled) return;

        if (_movementStateMachine.HasBufferedJumpCancel)
        {
            Debug.Log("Buffered jump used");

            _movementStateMachine.HasBufferedJumpCancel = false;
            CancelJump();
        }
    }

    protected void OnJumpInputReleased(InputAction.CallbackContext context)
    {
        if (_movementStateMachine.IsAirborneFromJump)
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
}
