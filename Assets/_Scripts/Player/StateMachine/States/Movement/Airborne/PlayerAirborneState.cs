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

        _movementStateMachine.CurrentExtraGravity = 0f;
        _movementStateMachine.IsGrounded = false;
        SetAnimatorAirborneState(true);
    }

    protected override void OnExit()
    {
        base.OnExit();

        _movementStateMachine.CurrentExtraGravity = 0f;
        SetAnimatorAirborneState(false);
    }

    protected override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();

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
            _movementStateMachine.IsGrounded = true;
            Land();
            _movementStateMachine.ChangeState(GetGroundedState());
            _movementStateMachine.IsAirborneFromJump = false;
        }
    }

    protected void CancelJumpVelocity(InputAction.CallbackContext context)
    {
        if (!_movementStateMachine.IsAirborneFromJump) return;

        _movementStateMachine.CurrentExtraGravity = JumpCancelExtraGravity;
        Debug.Log("Jump Canceled");
    }

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();

        _movementStateMachine.Player.JumpAction.canceled += CancelJumpVelocity;
    }

    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();

        _movementStateMachine.Player.JumpAction.canceled -= CancelJumpVelocity;
    }
}
