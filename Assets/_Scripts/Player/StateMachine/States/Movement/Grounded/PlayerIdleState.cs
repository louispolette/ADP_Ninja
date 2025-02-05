using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public override string Name => "Idle";

    public PlayerIdleState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void OnEnter()
    {
        base.OnEnter();

        _movementStateMachine.SpeedModifier = 0f;

        ResetVelocity();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        CheckMovementInput();
    }

    private void CheckMovementInput()
    {
        if (_movementStateMachine.MovementInput != Vector2.zero)
        {
            OnStartMove();
        }
    }
}
