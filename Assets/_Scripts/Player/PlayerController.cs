using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region exposed fields

    [Space]

    [SerializeField] private string _currentMovementState = "None";

    [field : Header("Movement Stats")]

    [field: SerializeField] public float BaseMovementSpeed { get; private set; } = 5f;
    [field: SerializeField] public float SprintSpeedMultiplier { get; private set; } = 1.5f;
    [field: SerializeField] public float CrouchSpeedMultiplier { get; private set; } = 0.5f;
    [field: SerializeField] public float RunningThreshold { get; private set; }
    [field: SerializeField] public float TurningSmoothTime { get; private set; }

    #endregion

    private PlayerMovementStateMachine _movementStateMachine;

    #region components
    public Transform CameraTransform { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public PlayerInput Input { get; private set; }
    public Animator Animator { get; private set; }
    #endregion

    #region input caching
    public InputActionMap PlayerActionMap { get; private set; }
    public InputActionMap UIActionMap { get; private set; }

    public InputAction MoveAction { get; private set; }
    public InputAction JumpAction { get; private set; }
    public InputAction SprintAction { get; private set; }
    public InputAction CrouchAction { get; private set; }
    public InputAction InteractAction { get; private set; }
    public InputAction ToggleMenuAction { get; private set; }
    #endregion

    /*[field: SerializeField]*/
    public Vector2 MovementInput { get; private set; }
    //public float magnitude;
    public bool IsHoldingSprintInput { get; private set; } = false;


    [Space] public float jumpForce = 1f;


    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Input = GetComponent<PlayerInput>();
        Animator = GetComponent<Animator>();

        CameraTransform = Camera.main.transform;

        InputSetup();
        StateMachinesSetup();

        void InputSetup()
        {
            Cursor.lockState = CursorLockMode.Locked;

            Input.onActionTriggered += ReadAction;

            PlayerActionMap = Input.actions.FindActionMap("Player");
            UIActionMap = Input.actions.FindActionMap("UI");

            MoveAction = PlayerActionMap.FindAction("Move");
            JumpAction = PlayerActionMap.FindAction("Jump");
            SprintAction = PlayerActionMap.FindAction("Sprint");
            CrouchAction = PlayerActionMap.FindAction("Crouch");
            InteractAction = PlayerActionMap.FindAction("Interact");
            ToggleMenuAction = PlayerActionMap.FindAction("OpenMenu");
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

        _currentMovementState = _movementStateMachine.CurrentState.Name;
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
        Debug.Log("Interact");
    }

    #region input methods

    private void ReadAction(InputAction.CallbackContext context)
    {
        InputAction action = context.action;

        if (action == MoveAction)
        {
            OnMove(context);
        }
        else if (action == JumpAction)
        {
            OnJump(context);
        }
        else if (action == SprintAction)
        {
            OnSprint(context);
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MovementInput = context.ReadValue<Vector2>();
            //magnitude = MovementInput.magnitude;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump();
        }
        if (context.canceled)
        {
            //Debug.Log("STOPPED JUMP");
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

    public void OpenMenu()
    {
        Input.currentActionMap = Input.currentActionMap = UIActionMap;
        InventoryManager.Instance.ShowInventoryUI();
    }

    public void CloseMenu()
    {
        Input.currentActionMap = Input.currentActionMap = PlayerActionMap;
        InventoryManager.Instance.HideInventoryUI();
    }

    #endregion

    // Jump à l'arrache
    private void Jump()
    {
        Rigidbody.linearVelocity = new Vector3(Rigidbody.linearVelocity.x, jumpForce, Rigidbody.linearVelocity.z);
    }
}
