using UnityEngine;

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

        _movementStateMachine.IsGrounded = false;
        SetAnimatorAirborneState(true);
    }

    protected override void OnExit()
    {
        base.OnExit();

        SetAnimatorAirborneState(false);
    }

    protected override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();

        CheckIfStillAirborne();
    }

    protected void CheckIfStillAirborne()
    {
        if (_movementStateMachine.Player.Rigidbody.linearVelocity.y >= 0) return;

        if (GroundCheck())
        {
            _movementStateMachine.IsGrounded = true;
            Land();
            _movementStateMachine.ChangeState(GetGroundedState());
        }
    }
}
