using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State _currentState;

    [SerializeField] private string _stateName = "None"; // Variable d'affichage (ne pas changer)

    protected abstract State InitialState { get; }

    public State CurrentState => _currentState;

    protected virtual void Start()
    {
        if (InitialState == null)
        {
            Debug.LogError($"The initial state of {GetType().Name} has not been set !");
            return;
        }

        ChangeState(InitialState);
    }

    void Update()
    {
        if (_currentState != null)
        {
            _currentState.Update();
        }
    }

    /// <summary>
    /// Permet de transitionner vers un autre �tat
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(State newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();           // On appelle la m�thode de sortie de l'ancien �tat
        }
        _currentState = newState;
        _currentState.Enter();              // Puis on appelle la m�thode d'entr�e du nouveau
        _stateName = _currentState.Name;
    }
}
