using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    #region exposed fields

    [field : Space]

    [field: SerializeField] public string CurrentMovementState { get; private set; } = "None";
    [field: SerializeField] public string PreviousMovementState { get; private set; } = "None";

    [field : Header("Movement")]

    [field: SerializeField] public float BaseMovementSpeed { get; private set; } = 5f;
    [field: SerializeField] public float Acceleration { get; private set; } = 1f;
    [field: SerializeField] public float Deceleration { get; private set; } = 1f;
    [field: SerializeField] public float WalkSpeedMultiplier { get; private set; } = 0.75f;
    [field: SerializeField] public float SprintSpeedMultiplier { get; private set; } = 1.5f;
    [field: SerializeField] public float CrouchSpeedMultiplier { get; private set; } = 0.5f;
    [field: SerializeField] public float RunningThreshold { get; private set; }
    [field: SerializeField] public float TurningSmoothTime { get; private set; }

    [field: Header("Jumping")]

    [field: SerializeField] public float JumpForce { get; private set; } = 5f;
    [field: SerializeField, Min(0f)] public float AirDeceleration { get; private set; } = 0f;
    [field: SerializeField, Min(0f)] public float AirAcceleration { get; private set; } = 5f;
    [field: SerializeField, Min(0f)] public float JumpCancelExtraGravity { get; private set; } = 0.5f;
    [field: SerializeField, Min(0f)] public float MinimumVelocityForJumpCancel { get; private set; } = 3f;

    [field: Space]

    [field: SerializeField, Min(0f)] public float JumpBufferDuration { get; private set; } = 0.15f;


    [field: Header("GroundCheck")]

    [field: SerializeField] public float GroundCheckWidth { get; private set; } = 1f;
    [field: SerializeField] public float GroundCheckYOrigin { get; private set; }
    [field: SerializeField] public LayerMask GroundCheckLayerMask { get; private set; }

    [field : Header("Floating Collision")]

    [field: SerializeField] public float FloatingGroundDetectionRange { get; private set; } = 1f;
    [field: SerializeField] public float FloatingRideHeight { get; private set; } = 0.75f;

    [field: Space]

    [field: SerializeField] public float SpringStrength { get; private set; } = 1f;
    [field: SerializeField] public float SpringDamper { get; private set; } = 1f;

    [field: Header("References")]

    [field: SerializeField] public CinemachineInputAxisController CameraInput { get; private set; }
    [field: SerializeField] public CameraController CameraEffectController { get; private set; }


    #endregion

    #region state machine

    private PlayerMovementStateMachine _movementStateMachine;

    #endregion

    #region components
    public Transform CameraTransform { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public PlayerInput Input { get; private set; }
    public Animator Animator { get; private set; }
    public CapsuleCollider Collider { get; private set; }
    public PlayerInteractionHandler InteractionHandler { get; private set; }
    public PlayerAnimationHandler AnimationHandler { get; private set; }
    #endregion

    #region input caching
    public InputActionMap PlayerActionMap { get; private set; }
    public InputActionMap UIActionMap { get; private set; }

    public InputAction MoveAction { get; private set; }
    public InputAction JumpAction { get; private set; }
    public InputAction SprintAction { get; private set; }
    public InputAction CrouchAction { get; private set; }
    public InputAction InteractAction { get; private set; }
    public InputAction OpenMenuAction { get; private set; }
    public InputAction CloseMenuAction { get; private set; }
    #endregion

    #region input variables
    public Vector2 MovementInput { get; private set; }
    public bool IsHoldingSprintInput { get; private set; } = false;
    public bool IsHoldingCrouchInput { get; private set; } = false;
    #endregion

    private void Awake()
    {
        Instance = this;

        Rigidbody = GetComponent<Rigidbody>();
        Input = GetComponent<PlayerInput>();
        Animator = GetComponent<Animator>();
        Collider = GetComponentInChildren<CapsuleCollider>();
        InteractionHandler = GetComponentInChildren<PlayerInteractionHandler>();
        AnimationHandler = GetComponent<PlayerAnimationHandler>();

        CameraTransform = Camera.main.transform;

        InputSetup();
        StateMachinesSetup();

        void InputSetup()
        {
            Cursor.lockState = CursorLockMode.Locked;

            PlayerActionMap = Input.actions.FindActionMap("Player");
            UIActionMap = Input.actions.FindActionMap("UI");

            MoveAction = PlayerActionMap.FindAction("Move");
            JumpAction = PlayerActionMap.FindAction("Jump");
            SprintAction = PlayerActionMap.FindAction("Sprint");
            CrouchAction = PlayerActionMap.FindAction("Crouch");
            InteractAction = PlayerActionMap.FindAction("Interact");
            OpenMenuAction = PlayerActionMap.FindAction("OpenMenu");

            CloseMenuAction = UIActionMap.FindAction("CloseMenu");

            Input.onActionTriggered += ReadAction;

            CloseMenuAction.started += OnCloseMenuInput;
        }

        void StateMachinesSetup()
        {
            _movementStateMachine = new PlayerMovementStateMachine(this);
        }
    }

    private void Start()
    {
        StartStateMachines();
    }


    private void Update()
    {
        _movementStateMachine.HandleInput();
        _movementStateMachine.Update();

        CurrentMovementState = _movementStateMachine.CurrentState.Name;

        if (_movementStateMachine.PreviousState != null)
        {
            PreviousMovementState = _movementStateMachine.PreviousState.Name;
        }
    }

    private void FixedUpdate()
    {
        _movementStateMachine.PhysicsUpdate();
    }

    private void StartStateMachines()
    {
        _movementStateMachine.Start();
    }

    public void Interact()
    {
        InteractionHandler.Interact();
    }

    #region input methods

    private void ReadAction(InputAction.CallbackContext context)
    {
        InputAction action = context.action;

        if (action == MoveAction)
        {
            OnMove(context);
        }
        else if (action == SprintAction)
        {
            OnSprint(context);
        }
        else if (action == CrouchAction)
        {
            OnCrouch(context);
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MovementInput = context.ReadValue<Vector2>();
        }
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsHoldingSprintInput = true;
        }
        if (context.canceled)
        {
            IsHoldingSprintInput = false;
        }
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsHoldingCrouchInput = true;
        }
        if (context.canceled)
        {
            IsHoldingCrouchInput = false;
        }
    }

    private void OnCloseMenuInput(InputAction.CallbackContext context) => CloseMenu();

    public void ResetMovementInput()
    {
        MovementInput = Vector2.zero;
    }

    #endregion

    public void OpenMenu()
    {
        ResetMovementInput();

        SwitchToUIInput();
        InventoryManager.Instance.ShowInventoryUI();
    }

    public void CloseMenu()
    {
        SwitchToPlayerInput();
        InventoryManager.Instance.HideInventoryUI();
    }

    [ContextMenu("UI Input")]
    public void SwitchToUIInput()
    {
        Input.SwitchCurrentActionMap(UIActionMap.name);
        CameraInput.enabled = false;
    }

    public void SwitchToPlayerInput()
    {
        Input.SwitchCurrentActionMap(PlayerActionMap.name);
        CameraInput.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * GroundCheckYOrigin, GroundCheckWidth);

        Vector3 center;

        if (Collider != null)
        {
            center = transform.TransformPoint(Collider.center);
        }
        else
        {
            center = transform.position;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(center, center + Vector3.down * FloatingGroundDetectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(center + Vector3.right * 0.1f, center + Vector3.down * FloatingRideHeight + Vector3.right * 0.1f);
    }
}
