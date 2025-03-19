using UnityEngine;

public class PlayerJumpPrepState : PlayerGroundedState
{
    public override string Name => "JumpPrep";

    public PlayerJumpPrepState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void OnEnter()
    {
        base.OnEnter();

        PlayerAnimationHandler.OnJumpPrepEnd += ChangeToJumpingState;
        SetAnimatorJumpPrep();
    }

    protected override void OnExit()
    {
        base.OnExit();

        PlayerAnimationHandler.OnJumpPrepEnd -= ChangeToJumpingState;
    }

    private void ChangeToJumpingState()
    {
        _movementStateMachine.ChangeState(_movementStateMachine.FallState);
        _movementStateMachine.IsAirborneFromJump = true;
        DoJumpImpulse();
    }
}
