using System;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private Animator _animator;

    public static Action OnJumpPrepEnd { get; set; }

    #region animator parameter caching

    public int ParamMovementInput { get; private set; }
    public int ParamJump { get; private set; }
    public int ParamIsSprinting { get; private set; }
    public int ParamIsRunning { get; private set; }
    public int ParamIsCrouching { get; private set; }
    public int ParamIsAirborne { get; private set; }
    public int ParamPrepareJump { get; private set; }
    public int ParamLand { get; private set; }

    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        CacheAnimatorParameterNames();

        void CacheAnimatorParameterNames()
        {
            ParamMovementInput = Animator.StringToHash("movementInput");
            ParamIsSprinting = Animator.StringToHash("isSprinting");
            ParamIsRunning = Animator.StringToHash("isRunning");
            ParamIsCrouching = Animator.StringToHash("isCrouching");
            ParamIsAirborne = Animator.StringToHash("isAirborne");
            ParamPrepareJump = Animator.StringToHash("prepareJump");
            ParamLand = Animator.StringToHash("land");
        }
    }

    #region animation event methods

    public void JumpPrepEnd()
    {
        OnJumpPrepEnd?.Invoke();
    }

    #endregion
}
