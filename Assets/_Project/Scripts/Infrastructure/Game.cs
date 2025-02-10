using System;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public GameStateMachine _stateMachine;
    public Game(Services services)
    {
        _stateMachine = new GameStateMachine(services);
        _stateMachine.Enter<BootstrapState>();
     
    }
}
public class BootstrapState : IState
{
    public BootstrapState (Services services)
    {
        services.RegisterSingle<IGameFactory>(new GameFactory());
    }
    public void Enter()
    {
        Debug.Log("Вошли в состояние " + this.GetType().Name);
    }

    public void Exit()
    {
        Debug.Log("Вышли из состония " + this.GetType().Name);
    }
}
public class GameFactory : IGameFactory
{

}
public interface IGameFactory : IService
{

}
public class GameStateMachine : IStateMachine
{
    private readonly Dictionary<Type, IState> _states;
    private IState _currentState;
    public GameStateMachine(Services services)
    {
        _states = new Dictionary<Type, IState>()
        {
            [(typeof(BootstrapState))] = new BootstrapState(services)
        };
    }
    public void Enter<State>()  
    {
        if(_currentState != null)
        {
            _currentState.Exit();
            _currentState = null;
        }
        _currentState = _states[typeof(State)]as IState;
        _currentState.Enter();
    } 
}
public interface IStateMachine
{
    void Enter<IState>();
}
public interface IState
{ 
    public void Enter();
    public void Exit();
}

