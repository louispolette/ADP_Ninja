using UnityEngine;

public abstract class State
{
    public virtual string Name { get; } = "Unnamed State";

    protected StateMachine _stateMachine;

    public State(StateMachine stateMachine)
    {
        _stateMachine = stateMachine; //On récupère une ref à la state machine qui utilise cet état
    }

    /// <summary>
    /// Appelé lorsqu'on entre dans cet état
    /// </summary>
    public void Enter()
    {
        OnEnter();
    }

    public void HandleInput()
    {
        OnHandleInput();
    }

    /// <summary>
    /// Appelé chaque frame lorsqu'on est dans cet état
    /// </summary>
    public void Update()
    {
        OnUpdate();
    }

    /// <summary>
    /// Appelé chaque FixedUpdate lorsqu'on est dans cet état
    /// </summary>
    public void PhysicsUpdate()
    {
        OnPhysicsUpdate();
    }

    /// <summary>
    /// Appelé lorsqu'on sort de cet état
    /// </summary>
    public void Exit()
    {
        OnExit();
    }

    protected virtual void OnEnter() { }

    protected virtual void OnHandleInput() { }

    protected virtual void OnUpdate() { }

    protected virtual void OnPhysicsUpdate() { }

    protected virtual void OnExit() { }
}
