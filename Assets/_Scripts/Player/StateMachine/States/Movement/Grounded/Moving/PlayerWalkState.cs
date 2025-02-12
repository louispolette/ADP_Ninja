using UnityEngine;

public class PlayerWalkState : PlayerMovingState
{
    public override string Name => "Walking";

    public PlayerWalkState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void OnEnter()
    {
        base.OnEnter();

        _movementStateMachine.SpeedModifier = 1f;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        CheckMovementInput();
    }

    private void CheckMovementInput()
    {
        if (_movementStateMachine.MovementInput == Vector2.zero)
        {
            _movementStateMachine.ChangeState(_movementStateMachine.IdleState);
        }
        else if (_movementStateMachine.MovementInput.magnitude >= RunningThreshold)
        {
            _movementStateMachine.ChangeState(_movementStateMachine.RunningState);
        }
    }
}
