using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintingState : PlayerGroundedState
{
    public override string Name => "Sprinting";

    public PlayerSprintingState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void OnEnter()
    {
        base.OnEnter();

        _movementStateMachine.SpeedModifier = _movementStateMachine.Player.SprintSpeedMultiplier;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        CheckMovementInput();
    }



    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();

        _movementStateMachine.Player.SprintAction.canceled += OnSprintInputCanceled;
    }

    protected override void OnSprintInputPressed(InputAction.CallbackContext context)
    {
        
    }

    private void OnSprintInputCanceled(InputAction.CallbackContext context)
    {
        StartWalkingOrRunning();
    }

    private void CheckMovementInput()
    {
        if (_movementStateMachine.MovementInput != Vector2.zero) return;

        StartWalkingOrRunning();
    }
}
