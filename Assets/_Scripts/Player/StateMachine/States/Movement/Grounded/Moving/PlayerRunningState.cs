using UnityEngine;

public class PlayerRunningState : PlayerMovingState
{
    public override string Name => "Running";

    public PlayerRunningState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void OnEnter()
    {
        base.OnEnter();

        SetAnimatorRunningState(true);
        _movementStateMachine.SpeedModifier = 1f;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        CheckMovementInput();
    }

    protected override void OnExit()
    {
        base.OnExit();


    }

    private void CheckMovementInput()
    {
        if (_movementStateMachine.MovementInput == Vector2.zero)
        {
            _movementStateMachine.ChangeState(_movementStateMachine.IdleState);
        }
        else if (_movementStateMachine.MovementInput.magnitude < RunningThreshold)
        {
            _movementStateMachine.ChangeState(_movementStateMachine.WalkState);
        }
    }
}
