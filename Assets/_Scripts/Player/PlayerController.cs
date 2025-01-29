using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Space]

    [SerializeField] private string _currentState = "None";

    private PlayerMovementStateMachine _movementStateMachine;

    private void Awake()
    {
        _movementStateMachine = new PlayerMovementStateMachine();
    }

    private void Start()
    {
        _movementStateMachine.Start();
    }

    private void Update()
    {
        _movementStateMachine.HandleInput();
        _movementStateMachine.Update();

        _currentState = _movementStateMachine.CurrentState.Name;
    }

    private void FixedUpdate()
    {
        _movementStateMachine.PhysicsUpdate();
    }
}
