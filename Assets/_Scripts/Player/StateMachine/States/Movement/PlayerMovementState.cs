using System;
using UnityEngine;

public abstract class PlayerMovementState : State
{
    protected PlayerMovementStateMachine _movementStateMachine;

    protected float RunningThreshold => _movementStateMachine.Player.RunningThreshold;
    protected float MovementSpeed =>   _movementStateMachine.Player.BaseMovementSpeed
                                     * _movementStateMachine.SpeedModifier
                                     * _movementStateMachine.MovementInput.magnitude;
    protected Vector3 MovementDirection => new Vector3(_movementStateMachine.MovementInput.x,
                                                       0f,
                                                       _movementStateMachine.MovementInput.y);

    public PlayerMovementState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
        _movementStateMachine = stateMachine;

        InitializeData();
    }

    private void InitializeData()
    {
        _movementStateMachine.TimeToReachTargetRotation.y = _movementStateMachine.Player.TurningSmoothTime;
    }

    #region state methods
    protected override void OnEnter()
    {
        AddInputActionCallbacks();
    }

    protected override void OnExit()
    {
        RemoveInputActionCallbacks();
    }

    protected override void OnHandleInput()
    {
        ReadMovementInput();
    }

    protected override void OnPhysicsUpdate()
    {
        Move();
    }
    #endregion

    #region horizontal movement

    /// <summary>
    /// Handles Movement
    /// </summary>
    private void Move()
    {
        if (_movementStateMachine.MovementInput == Vector2.zero || _movementStateMachine.SpeedModifier == 0f) return;

        UpdateTargetRotation(MovementDirection);
        RotateTowardsTargetRotation();

        Vector3 targetRotationDirection = GetTargetRotationDirection(_movementStateMachine.CurrentTargetRotation.y);
        Vector3 currentHorizontalVelocity = GetHorizontalVelocity();

        _movementStateMachine.Player.Rigidbody.AddForce(targetRotationDirection * MovementSpeed - currentHorizontalVelocity,
                                                        ForceMode.VelocityChange);
    }

    protected Vector3 GetHorizontalVelocity()
    {
        Vector3 horizontalVel = _movementStateMachine.Player.Rigidbody.linearVelocity;
        horizontalVel.y = 0f;
        return horizontalVel;
    }

    protected void ResetVelocity()
    {
        Vector3 newVel = _movementStateMachine.Player.Rigidbody.linearVelocity;
        newVel.x = 0f;
        newVel.z = 0f;
        _movementStateMachine.Player.Rigidbody.linearVelocity = newVel;
    }

    #endregion

    #region rotation
    /// <summary>
    /// Updates the target rotation from a direction
    /// </summary>
    /// <returns>The new target rotation</returns>
    protected void UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
    {
        float directionAngle = GetDirectionAngle(direction);
        
        if (shouldConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }

        if (directionAngle != _movementStateMachine.CurrentTargetRotation.y)
        {
            _movementStateMachine.CurrentTargetRotation.y = directionAngle;
            _movementStateMachine.DampedTargetRotationPassedTime.y = 0f;
        }
    }

    /// <summary>
    /// Adds the camera's rotation to the given angle
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private float AddCameraRotationToAngle(float angle)
    {
        angle += _movementStateMachine.Player.CameraTransform.eulerAngles.y;

        if (angle > 360f)
        {
            angle -= 360f;
        }

        return angle;
    }

    /// <summary>
    /// Returns the world space euler angle of the given direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private float GetDirectionAngle(Vector3 direction)
    {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        if (directionAngle < 0f)
        {
            directionAngle += 360f;
        }

        return directionAngle;
    }

    /// <summary>
    /// Executes the smooth rotation towards the target rotation
    /// </summary>
    protected void RotateTowardsTargetRotation()
    {
        float currentYAngle = _movementStateMachine.Player.Rigidbody.rotation.eulerAngles.y;

        if (currentYAngle == _movementStateMachine.CurrentTargetRotation.y) return;

        float smoothedAngle = Mathf.SmoothDampAngle(currentYAngle,
                                                    _movementStateMachine.CurrentTargetRotation.y,
                                                    ref _movementStateMachine.DampedTargetRotationCurrentVelocity.y,
                                                    _movementStateMachine.TimeToReachTargetRotation.y - _movementStateMachine.DampedTargetRotationPassedTime.y);

        Quaternion targetRotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        _movementStateMachine.Player.Rigidbody.MoveRotation(targetRotation);

        _movementStateMachine.DampedTargetRotationPassedTime.y += Time.deltaTime;
    }

    /// <summary>
    /// Returns the direction represented by the given Y-axis angle
    /// </summary>
    /// <param name="targetAngle"></param>
    /// <returns></returns>
    protected Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }
    #endregion

    #region input
    private void ReadMovementInput()
    {
        _movementStateMachine.MovementInput = _movementStateMachine.Player.MovementInput;
    }

    protected virtual void AddInputActionCallbacks() { }

    protected virtual void RemoveInputActionCallbacks() { }
    #endregion
}
