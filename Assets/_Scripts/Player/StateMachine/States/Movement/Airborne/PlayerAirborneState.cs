using UnityEngine;

public class PlayerAirborneState : PlayerMovementState
{
    public override string Name => "Airborne";

    public PlayerAirborneState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void OnEnter()
    {
        base.OnEnter();

        SetAnimatorAirborneState(true);
        //Debug.Log("Airborne");
    }

    protected override void OnExit()
    {
        base.OnExit();

        SetAnimatorAirborneState(false);
        //Debug.Log("Grounded");
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
            _movementStateMachine.ChangeState(GetGroundedState());
        }
    }
}
