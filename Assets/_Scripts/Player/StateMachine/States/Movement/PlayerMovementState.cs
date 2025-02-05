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

    private void Move()
    {
        if (_movementStateMachine.MovementInput == Vector2.zero || _movementStateMachine.SpeedModifier == 0f) return;

        float targetRotationYAngle = Rotate(MovementDirection);
        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

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
        _movementStateMachine.Player.Rigidbody.linearVelocity = Vector3.zero;
    }

    #endregion

    #region rotation
    private float Rotate(Vector3 direction)
    {
        float directionAngle = UpdateTargetRotation(direction);
        
        RotateTowardsTargetRotation();

        return directionAngle;
    }

    /// <summary>
    /// Updates the target rotation from a direction
    /// </summary>
    /// <returns></returns>
    protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
    {
        float directionAngle = GetDirectionAngle(direction);
        
        if (shouldConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }

        if (directionAngle != _movementStateMachine.CurrentTargetRotation.y)
        {
            ChangeTargetRotation(directionAngle);
        }

        return directionAngle;


        void ChangeTargetRotation(float targetAngle)
        {
            _movementStateMachine.CurrentTargetRotation.y = targetAngle;

            _movementStateMachine.DampedTargetRotationPassedTime.y = 0f;
        }
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

    protected void RotateTowardsTargetRotation()
    {
        float currentYAngle = _movementStateMachine.Player.Rigidbody.rotation.eulerAngles.y;

        Debug.Log(currentYAngle == _movementStateMachine.CurrentTargetRotation.y);
        if (currentYAngle == _movementStateMachine.CurrentTargetRotation.y) return;

        float smoothedAngle = Mathf.SmoothDampAngle(currentYAngle,
                                                    _movementStateMachine.CurrentTargetRotation.y,
                                                    ref _movementStateMachine.DampedTargetRotationCurrentVelocity.y,
                                                    _movementStateMachine.TimeToReachTargetRotation.y - _movementStateMachine.DampedTargetRotationPassedTime.y);

        Quaternion targetRotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        _movementStateMachine.Player.Rigidbody.MoveRotation(targetRotation);

        _movementStateMachine.DampedTargetRotationPassedTime.y += Time.deltaTime;
    }

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
