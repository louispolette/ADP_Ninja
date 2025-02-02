using System;
using UnityEngine;

public abstract class PlayerMovementState : State
{
    protected PlayerMovementStateMachine _movementStateMachine;
    protected Vector2 MovementInput { get; private set; }

    protected float _baseSpeed = 5f;
    protected float _speedModifier = 1f;

    protected Vector3 _currentTargetRotation;
    protected Vector3 _timeToReachTargetRotation;
    protected Vector3 _dampedTargetRotationCurrentVelocity;
    protected Vector3 _dampedTargetRotationPassedTime;

    protected float MovementSpeed => _baseSpeed * _speedModifier;
    protected Vector3 MovementDirection => new Vector3(MovementInput.x, 0f, MovementInput.y);

    public PlayerMovementState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
        _movementStateMachine = stateMachine;

        InitializeData();
    }

    private void InitializeData()
    {
        _timeToReachTargetRotation.y = 0.14f;
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

        float targetRotationYAngle = Rotate(MovementDirection);

        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

        Vector3 currentHorizontalVelocity = GetHorizontalVelocity();

        _movementStateMachine.Player.Rigidbody.AddForce(targetRotationDirection * MovementSpeed - currentHorizontalVelocity, ForceMode.VelocityChange);
    }

    private float Rotate(Vector3 direction)
    {
        float directionAngle = UpdateTargetRotation(direction);

        RotateTowardsTargetRotation();

        return directionAngle;
    }

    private float AddCameraRotationToAngle(float angle)
    {
        angle += _movementStateMachine.Player.CameraTransform.eulerAngles.y;

        if (angle > 360f)
        {
            angle -= 360f;
        }

        return angle;
    }

    private float GetDirectionAngle(Vector3 direction)
    {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        if (directionAngle < 0f)
        {
            directionAngle += 360f;
        }

        return directionAngle;
    }


    #region reusable methods

    protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
    {
        float directionAngle = GetDirectionAngle(direction);

        if (shouldConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }

        if (directionAngle != _currentTargetRotation.y)
        {
            ChangeTargetRotation(directionAngle);
        }

        return directionAngle;


        void ChangeTargetRotation(float targetAngle)
        {
            _currentTargetRotation.y = targetAngle;

            _dampedTargetRotationPassedTime.y = 0f;
        }
    }

    protected Vector3 GetHorizontalVelocity()
    {
        Vector3 horizontalVel = _movementStateMachine.Player.Rigidbody.linearVelocity;
        horizontalVel.y = 0f;
        return horizontalVel;
    }

    protected void RotateTowardsTargetRotation()
    {
        float currentYAngle = _movementStateMachine.Player.Rigidbody.rotation.eulerAngles.y;

        if (currentYAngle == _currentTargetRotation.y) return;

        float smoothedAngle = Mathf.SmoothDampAngle(currentYAngle,
                                                    _currentTargetRotation.y,
                                                    ref _dampedTargetRotationCurrentVelocity.y,
                                                    _timeToReachTargetRotation.y - _dampedTargetRotationPassedTime.y);

        Quaternion targetRotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        _movementStateMachine.Player.Rigidbody.MoveRotation(targetRotation);

        _dampedTargetRotationPassedTime.y += Time.deltaTime;
    }

    protected Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    #endregion
}
