using UnityEngine;

[System.Serializable]
public class PlayerMovementStateMachine : StateMachine
{
    protected override State InitialState => new PlayerIdleState(this);

    public PlayerController Player { get; }

    #region cached states
    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerRunningState RunningState { get; }
    public PlayerSprintingState SprintingState { get; }
    public PlayerCrouchIdleState CrouchIdleState { get; }
    public PlayerCrouchWalkState CrouchWalkState { get; }
    public PlayerJumpPrepState JumpPrepState { get; }
    public PlayerFallState FallState { get; }
    #endregion

    #region shared state variables
    public Vector2 MovementInput { get; set; }
    public float SpeedModifier { get; set; } = 1f;
    public float CurrentExtraGravity { get; set; } = 1f;
    public bool IsSprinting { get; set; } = false;
    public bool IsCrouching { get; set; } = false;
    public bool IsGrounded { get; set; } = false;
    public bool IsAirborneFromJump { get; set; } = false;

    private Vector3 _currentTargetRotation;
    private Vector3 _dampedTargetRotationCurrentVelocity;
    private Vector3 _dampedTargetRotationPassedTime;
    private Vector3 _timeToReachTargetRotation;

    public ref Vector3 CurrentTargetRotation => ref _currentTargetRotation;
    public ref Vector3 DampedTargetRotationCurrentVelocity => ref _dampedTargetRotationCurrentVelocity;
    public ref Vector3 DampedTargetRotationPassedTime => ref _dampedTargetRotationPassedTime;
    public ref Vector3 TimeToReachTargetRotation => ref _timeToReachTargetRotation;
    #endregion

    public PlayerMovementStateMachine(PlayerController playerController)
    {
        Player = playerController;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunningState = new PlayerRunningState(this);
        SprintingState = new PlayerSprintingState(this);
        CrouchIdleState = new PlayerCrouchIdleState(this);
        CrouchWalkState = new PlayerCrouchWalkState(this);
        JumpPrepState = new PlayerJumpPrepState(this);
        FallState = new PlayerFallState(this);
    }
}
