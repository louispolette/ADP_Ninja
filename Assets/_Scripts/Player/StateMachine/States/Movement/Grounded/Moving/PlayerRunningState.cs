using UnityEngine;

public class PlayerRunningState : PlayerMovementState
{
    public override string Name => "Running";

    public PlayerRunningState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }
}
