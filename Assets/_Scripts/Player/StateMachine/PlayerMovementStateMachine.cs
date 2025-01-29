using UnityEngine;

public class PlayerMovementStateMachine : StateMachine
{
    protected override State InitialState => new PlayerIdleState(this);

    public PlayerController Player { get; }

    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerRunningState RunningState { get; }
    public PlayerSprintingState SprintingState { get; }

    public PlayerMovementStateMachine(PlayerController playerController)
    {
        Player = playerController;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunningState = new PlayerRunningState(this);
        SprintingState = new PlayerSprintingState(this);
    }
}
