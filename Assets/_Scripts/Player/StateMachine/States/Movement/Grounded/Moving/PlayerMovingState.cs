using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerMovingState : PlayerGroundedState
{
    public PlayerMovingState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }
}
