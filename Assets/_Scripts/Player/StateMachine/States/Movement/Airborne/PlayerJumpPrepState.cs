using UnityEngine;

public class PlayerJumpPrepState : PlayerGroundedState
{
    public override string Name => "JumpPrep";

    public PlayerJumpPrepState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void OnEnter()
    {
        base.OnEnter();

        PlayerAnimationHandler.OnJumpPrepEnd += JumpPrepEnd;
        SetAnimatorJumpPrep();
    }

    protected override void OnExit()
    {
        base.OnExit();

        PlayerAnimationHandler.OnJumpPrepEnd -= JumpPrepEnd;
    }

    private void JumpPrepEnd()
    {
        _movementStateMachine.ChangeState(_movementStateMachine.FallState);
        DoJumpImpulse();
    }
}
