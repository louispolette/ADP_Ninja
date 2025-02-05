using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region exposed fields

    [Space]

    [SerializeField] private string _currentMovementState = "None";

    [field : Header("Movement Stats")]

    [field: SerializeField] public float BaseMovementSpeed { get; private set; } = 5f;
    [field: SerializeField] public float RunningThreshold { get; private set; }
    [field: SerializeField] public float TurningSmoothTime { get; private set; }

    #endregion

    private PlayerMovementStateMachine _movementStateMachine;

    #region components
    public Transform CameraTransform { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public PlayerInput Input { get; private set; }
    #endregion

    #region input action caching
    public InputAction MoveAction { get; private set; }
    public InputAction JumpAction { get; private set; }
    public InputAction SprintAction { get; private set; }
    public InputAction CrouchAction { get; private set; }
    #endregion

    public Vector2 MovementInput { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Input = GetComponent<PlayerInput>();

        CameraTransform = Camera.main.transform;

        InputSetup();
        StateMachinesSetup();

        void InputSetup()
        {
            Cursor.lockState = CursorLockMode.Locked;

            Input.onActionTriggered += ReadAction;

            MoveAction = Input.currentActionMap.FindAction("Move");
            JumpAction = Input.currentActionMap.FindAction("Jump");
            SprintAction = Input.currentActionMap.FindAction("Sprint");
            CrouchAction = Input.currentActionMap.FindAction("Crouch");
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
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MovementInput = context.ReadValue<Vector2>();
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("JUMP");
        }
    }
}
