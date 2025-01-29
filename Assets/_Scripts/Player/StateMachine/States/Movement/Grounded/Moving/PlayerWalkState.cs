using UnityEngine;

public class PlayerWalkState : PlayerMovementState
{
    public override string Name => "Walking";

    public PlayerWalkState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }
}
