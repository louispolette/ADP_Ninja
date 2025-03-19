using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerMovementState : State
{
    protected PlayerMovementStateMachine _movementStateMachine;

    protected float RunningThreshold => _movementStateMachine.Player.RunningThreshold;
    protected float TargetSpeed =>   _movementStateMachine.Player.BaseMovementSpeed
                                     * _movementStateMachine.SpeedModifier
                                     * _movementStateMachine.MovementInput.magnitude;
    protected virtual float Acceleration => _movementStateMachine.Player.Acceleration;
    protected virtual float Deceleration => _movementStateMachine.Player.Deceleration;
    protected virtual float JumpForce => _movementStateMachine.Player.JumpForce;
    protected virtual float AirDeceleration => _movementStateMachine.Player.AirDeceleration;
    protected virtual float AirAcceleration => _movementStateMachine.Player.AirAcceleration;
    protected virtual float JumpCancelExtraGravity => _movementStateMachine.Player.JumpCancelExtraGravity;

    protected float GroundCheckWidth => _movementStateMachine.Player.GroundCheckWidth;
    protected float GroundCheckYOrigin => _movementStateMachine.Player.GroundCheckYOrigin;
    protected LayerMask GroundCheckLayerMask => _movementStateMachine.Player.GroundCheckLayerMask;
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

    protected override void OnUpdate()
    {
        base.OnUpdate();

        SetAnimatorVelocity(_movementStateMachine.Player.MovementInput.magnitude);
    }

    protected override void OnPhysicsUpdate()
    {
        Move();
    }
    #endregion

    #region state finding

    /// <summary>
    /// Returns the walking, running or sprinting state based on input
    /// </summary>
    protected virtual PlayerGroundedState GetGroundedState()
    {
        if (_movementStateMachine.Player.IsHoldingSprintInput && _movementStateMachine.CurrentState != _movementStateMachine.SprintingState)
        {
            return _movementStateMachine.SprintingState;
        }

        if (_movementStateMachine.Player.IsHoldingCrouchInput && !_movementStateMachine.IsCrouching)
        {
            return _movementStateMachine.CrouchIdleState;
        }

        if (_movementStateMachine.MovementInput.magnitude >= RunningThreshold)
        {
            return _movementStateMachine.RunningState;
        }
        else
        {
            return _movementStateMachine.WalkState;
        }
    }

    #endregion

    #region input
    private void ReadMovementInput()
    {
        _movementStateMachine.MovementInput = _movementStateMachine.Player.MovementInput;
    }

    protected virtual void AddInputActionCallbacks()
    {
        _movementStateMachine.Player.JumpAction.started += OnJumpInputPressed;
    }

    protected virtual void RemoveInputActionCallbacks()
    {
        _movementStateMachine.Player.JumpAction.started -= OnJumpInputPressed;
    }

    protected virtual void OnJumpInputPressed(InputAction.CallbackContext context)
    {
        TryJump();
    }
    #endregion

    #region horizontal movement

    /// <summary>
    /// Handles Movement
    /// </summary>
    private void Move()
    {
        bool moveInputIsZero = _movementStateMachine.MovementInput == Vector2.zero;

        UpdateTargetRotation(MovementDirection);

        // Player stops immediatly for some reason
        if (!moveInputIsZero)
        {
            RotateTowardsTargetRotation();
        }

        Vector3 targetRotationDirection = GetTargetRotationDirection(_movementStateMachine.CurrentTargetRotation.y);
        Vector3 currentHorizontalVelocity = GetHorizontalVelocity();

        Vector3 velDiff = targetRotationDirection * TargetSpeed - currentHorizontalVelocity;
        Vector3 force;

        if (TargetSpeed >= currentHorizontalVelocity.magnitude)
        {
            force = velDiff * Acceleration;
        }
        else
        {
            force = velDiff * Deceleration;
        }

        _movementStateMachine.Player.Rigidbody.AddForce(force, ForceMode.Acceleration);

        //_movementStateMachine.Player.Rigidbody.AddForce(targetRotationDirection * MovementSpeed - currentHorizontalVelocity,
                                                        //ForceMode.VelocityChange);
    }

    protected Vector3 GetHorizontalVelocity()
    {
        Vector3 horizontalVel = _movementStateMachine.Player.Rigidbody.linearVelocity;
        horizontalVel.y = 0f;
        return horizontalVel;
    }

    protected void ResetHorizontalVelocity()
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

    #region jumping

    protected void DoJumpImpulse()
    {
        ResetVerticalVelocity();
        _movementStateMachine.Player.Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);
    }

    protected void TryJump()
    {
        if (GroundCheck())
        {
            InitiateJumpPrep();
        }
    }

    protected void InitiateJumpPrep()
    {
        _movementStateMachine.ChangeState(_movementStateMachine.JumpPrepState); 
    }

    protected void ResetVerticalVelocity()
    {
        _movementStateMachine.Player.Rigidbody.linearVelocity = new Vector3(_movementStateMachine.Player.Rigidbody.linearVelocity.x,
                                                                             0f,
                                                                             _movementStateMachine.Player.Rigidbody.linearVelocity.z);
    }

    protected bool GroundCheck()
    {
        Collider[] hitObjects = Physics.OverlapSphere(_movementStateMachine.Player.transform.position + Vector3.up * GroundCheckYOrigin,
                                                      GroundCheckWidth,
                                                      GroundCheckLayerMask,
                                                      QueryTriggerInteraction.Ignore);

        return hitObjects.Length > 0;
    }

    protected void Land()
    {
        SetAnimatorLand();
    }

    #endregion

    #region animation

    protected void SetAnimatorVelocity(float newValue)
    {
        _movementStateMachine.Player.Animator.SetFloat(_movementStateMachine.Player.AnimationHandler.ParamHorizontalSpeed, newValue);

        /*Vector3 vel = _movementStateMachine.Player.Rigidbody.linearVelocity;
        float horizontalVelocityMag = new Vector3(vel.x, 0f, vel.z).magnitude;
        _movementStateMachine.Player.Animator.SetFloat(_movementStateMachine.Player.AnimatorParamVelocity, horizontalVelocityMag);*/
    }

    protected void SetAnimatorCrouchedState(bool newValue)
    {
        _movementStateMachine.Player.Animator.SetBool(_movementStateMachine.Player.AnimationHandler.ParamIsCrouching, newValue);
    }

    protected void SetAnimatorSprintingState(bool newValue)
    {
        _movementStateMachine.Player.Animator.SetBool(_movementStateMachine.Player.AnimationHandler.ParamIsSprinting, newValue);
    }

    protected void SetAnimatorAirborneState(bool newValue)
    {
        _movementStateMachine.Player.Animator.SetBool(_movementStateMachine.Player.AnimationHandler.ParamIsAirborne, newValue);

    }

    protected void SetAnimatorJumpPrep()
    {
        _movementStateMachine.Player.Animator.SetTrigger(_movementStateMachine.Player.AnimationHandler.ParamPrepareJump);
    }

    protected void SetAnimatorLand()
    {
        _movementStateMachine.Player.Animator.SetTrigger(_movementStateMachine.Player.AnimationHandler.ParamLand);
    }

    #endregion
}
