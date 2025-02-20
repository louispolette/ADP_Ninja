using UnityEngine;

public class PlayerJumpingState : PlayerAirborneState
{
    public override string Name => "Jumping";

    public PlayerJumpingState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }
}
