using UnityEngine;

public abstract class State
{
    public virtual string Name { get; } = "Unnamed State";

    protected StateMachine _stateMachine;

    public State(StateMachine stateMachine)
    {
        _stateMachine = stateMachine; //On r�cup�re une ref � la state machine qui utilise cet �tat
    }

    /// <summary>
    /// Appel� lorsqu'on entre dans cet �tat
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
    /// Appel� chaque frame lorsqu'on est dans cet �tat
    /// </summary>
    public void Update()
    {
        OnUpdate();
    }

    /// <summary>
    /// Appel� chaque FixedUpdate lorsqu'on est dans cet �tat
    /// </summary>
    public void PhysicsUpdate()
    {
        OnPhysicsUpdate();
    }

    /// <summary>
    /// Appel� lorsqu'on sort de cet �tat
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
