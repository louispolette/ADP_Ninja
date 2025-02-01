using System;
using UnityEngine;

public abstract class PlayerMovementState : State
{
    protected PlayerMovementStateMachine _movementStateMachine;
    protected Vector2 MovementInput { get; private set; }

    protected float _baseSpeed = 5f;
    protected float _speedModifier = 1f;

    protected float MovementSpeed => _baseSpeed * _speedModifier;
    protected Vector3 MovementDirection => new Vector3(MovementInput.x, 0f, MovementInput.y);

    public PlayerMovementState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
        _movementStateMachine = stateMachine;
    }

    protected override void OnHandleInput()
    {
        ReadMovementInput();
    }

    protected override void OnPhysicsUpdate()
    {
        Move();
    }

    private void ReadMovementInput()
    {
        MovementInput = _movementStateMachine.Player.MovementInput;
    }

    private void Move()
    {
        if (MovementInput == Vector2.zero || _speedModifier == 0f) return;

        Vector3 currentHorizontalVelocity = GetHorizontalVelocity();

        _movementStateMachine.Player.Rigidbody.AddForce(MovementDirection * MovementSpeed - currentHorizontalVelocity, ForceMode.VelocityChange);
    }

    protected Vector3 GetHorizontalVelocity()
    {
        Vector3 horizontalVel = _movementStateMachine.Player.Rigidbody.linearVelocity;
        horizontalVel.y = 0f;
        return horizontalVel;
    }
}
