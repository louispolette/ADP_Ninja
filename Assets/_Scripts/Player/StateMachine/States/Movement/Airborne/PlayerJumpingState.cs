using UnityEngine;

public class PlayerJumpingState : PlayerMovementState
{
    public override string Name => "Jumping";

    public PlayerJumpingState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }


}
