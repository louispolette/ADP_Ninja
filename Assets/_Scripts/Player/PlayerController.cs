using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Space]

    [SerializeField] private string _currentMovementState = "None";

    private PlayerMovementStateMachine _movementStateMachine;

    public Rigidbody Rigidbody { get; private set; }
    public PlayerInput Input { get; private set; }

    private InputAction _moveAction;
    private InputAction _jumpAction;

    public Vector2 MovementInput { get; private set; }

    private void Awake()
    {
        _movementStateMachine = new PlayerMovementStateMachine(this);

        Rigidbody = GetComponent<Rigidbody>();
        Input = GetComponent<PlayerInput>();

        InputSetup();

        void InputSetup()
        {
            Input.onActionTriggered += ReadAction;

            _moveAction = Input.currentActionMap.FindAction("Move");
            _jumpAction = Input.currentActionMap.FindAction("Jump");
        }
    }

    private void Start()
    {
        _movementStateMachine.Start();
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

    private void ReadAction(InputAction.CallbackContext context)
    {
        InputAction action = context.action;

        if (action == _moveAction)
        {
            OnMove(context);
        }
        else if (action == _jumpAction)
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
