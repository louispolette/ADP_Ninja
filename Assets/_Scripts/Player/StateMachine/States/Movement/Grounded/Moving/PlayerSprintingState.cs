using NUnit.Framework.Internal;
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

        //_movementStateMachine.Player.SprintAction.started += TestS;
        //_movementStateMachine.Player.SprintAction.performed += TestP;
        _movementStateMachine.Player.SprintAction.canceled += OnSprintInputCanceled;
    }

    private void TestP(InputAction.CallbackContext c) => Debug.Log("perform");

    private void TestS(InputAction.CallbackContext c) => Debug.Log("started");

    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();

        //_movementStateMachine.Player.SprintAction.started -= TestS;
        //_movementStateMachine.Player.SprintAction.performed -= TestP;
        _movementStateMachine.Player.SprintAction.canceled -= OnSprintInputCanceled;
    }

    protected override void OnSprintInputPressed(InputAction.CallbackContext context)
    {
        
    }

    private void OnSprintInputCanceled(InputAction.CallbackContext context)
    {
        _movementStateMachine.ChangeState(GetMovingState());
    }

    private void CheckMovementInput()
    {
        if (_movementStateMachine.MovementInput != Vector2.zero) return;

        _movementStateMachine.ChangeState(_movementStateMachine.IdleState);
    }
}
