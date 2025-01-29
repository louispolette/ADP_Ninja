using UnityEngine;

public abstract class PlayerMovementState : State
{
    public PlayerMovementState(StateMachine stateMachine) : base(stateMachine) { }
}
