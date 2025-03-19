using UnityEngine;

public abstract class StateMachine
{
    protected State _currentState;
    protected State _previousState;

    public string _stateName { get; private set; } = "None";

    protected abstract State InitialState { get; }

    public State CurrentState => _currentState;
    public State PreviousState => _previousState;

    public virtual void Start()
    {
        if (InitialState == null)
        {
            Debug.LogError($"The initial state of {GetType().Name} has not been set !");
            return;
        }

        ChangeState(InitialState);
    }

    public void HandleInput()
    {
        if (_currentState != null)
        {
            _currentState.HandleInput();
        }
    }

    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.Update();
        }
    }

    public void PhysicsUpdate()
    {
        if (_currentState != null)
        {
            _currentState.PhysicsUpdate();
        }
    }

    /// <summary>
    /// Permet de transitionner vers un autre �tat
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(State newState, bool allowChangeIntoSelf = true)
    {
        if (_currentState == newState)
        {
            if (!allowChangeIntoSelf)
            {
                return;
            }
        }
        else
        {
            _previousState = _currentState;
        }

        if (_currentState != null)
        {
            _currentState.Exit();           // On appelle la m�thode de sortie de l'ancien �tat
        }

        _currentState = newState;
        _currentState.Enter();              // Puis on appelle la m�thode d'entr�e du nouveau
        _stateName = _currentState.Name;
    }
}
