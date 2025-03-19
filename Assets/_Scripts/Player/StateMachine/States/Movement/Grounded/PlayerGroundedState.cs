using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    protected override void OnEnter()
    {
        base.OnEnter();

        _movementStateMachine.IsGrounded = true;
    }

    protected override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();

        CheckIfStillGrounded();
        Float();
    }

    protected void CheckIfStillGrounded()
    {
        if (!GroundCheck())
        {
            _movementStateMachine.ChangeState(_movementStateMachine.FallState);
        }
    }

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();

        _movementStateMachine.Player.SprintAction.started += OnSprintInputPressed;
        _movementStateMachine.Player.CrouchAction.started += OnCrouchInputPressed;
        _movementStateMachine.Player.InteractAction.started += OnInteractInputPressed;
        _movementStateMachine.Player.OpenMenuAction.started += OnToggleMenuInputPressed;
    }

    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();

        _movementStateMachine.Player.SprintAction.started -= OnSprintInputPressed;
        _movementStateMachine.Player.CrouchAction.started -= OnCrouchInputPressed;
        _movementStateMachine.Player.InteractAction.started -= OnInteractInputPressed;
        _movementStateMachine.Player.OpenMenuAction.started -= OnToggleMenuInputPressed;
    }

    private void Float()
    {
        PlayerController player = _movementStateMachine.Player;
        BoxCollider collider = _movementStateMachine.Player.Collider;

        Ray ray = new Ray(player.transform.TransformPoint(collider.center), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, player.FloatingGroundDetectionRange, player.GroundCheckLayerMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 vel = player.Rigidbody.linearVelocity;
            Vector3 rayDir = Vector3.down;

            float rayDirVel = Vector3.Dot(rayDir, vel);

            float x = hit.distance - player.FloatingRideHeight;

            float springForce = (x * player.SpringStrength) - (rayDirVel * player.SpringDamper);

            player.Rigidbody.AddForce(rayDir * springForce);
        }
    }

    #region input methods

    protected virtual void OnSprintInputPressed(InputAction.CallbackContext context)
    {
        if (_movementStateMachine.MovementInput == Vector2.zero) return;

        _movementStateMachine.ChangeState(_movementStateMachine.SprintingState);
    }

    protected virtual void OnCrouchInputPressed(InputAction.CallbackContext context)
    {
        if (_movementStateMachine.MovementInput == Vector2.zero)
        {
            _movementStateMachine.ChangeState(_movementStateMachine.CrouchIdleState);
        }
        else
        {
            _movementStateMachine.ChangeState(_movementStateMachine.CrouchWalkState);
        }
    }

    protected virtual void OnInteractInputPressed(InputAction.CallbackContext context)
    {
        _movementStateMachine.Player.Interact();
    }

    protected virtual void OnToggleMenuInputPressed(InputAction.CallbackContext context)
    {
        _movementStateMachine.Player.OpenMenu();
    }

    #endregion
}
