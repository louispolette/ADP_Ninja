using UnityEngine;

public class PlayerSprintingState : PlayerGroundedState
{
    public override string Name => "Sprinting";

    public PlayerSprintingState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }
}
