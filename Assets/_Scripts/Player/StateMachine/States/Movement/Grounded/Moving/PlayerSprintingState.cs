using UnityEngine;

public class PlayerSprintingState : PlayerMovementState
{
    public override string Name => "Sprinting";

    public PlayerSprintingState(StateMachine stateMachine) : base(stateMachine) { }
}
