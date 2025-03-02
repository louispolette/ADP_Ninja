using UnityEngine;

public class PlayerFallState : PlayerAirborneState
{
    public override string Name => "Falling";

    public PlayerFallState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }
}
