using UnityEngine;

public class PlayerWalkState : PlayerMovementState
{
    public override string Name => "Walking";

    public PlayerWalkState(StateMachine stateMachine) : base(stateMachine) { }
}
