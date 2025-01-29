using UnityEngine;

public class PlayerIdleState : PlayerMovementState
{
    public override string Name => "Idle";

    public PlayerIdleState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }
}
