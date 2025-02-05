using UnityEngine;

public class PlayerStoppingState : PlayerGroundedState
{
    public override string Name => "Stopping";

    public PlayerStoppingState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void OnEnter()
    {
        base.OnEnter();

        _movementStateMachine.ChangeState(_movementStateMachine.IdleState);

        //_movementStateMachine.SpeedModifier = 0f;
    }
}
