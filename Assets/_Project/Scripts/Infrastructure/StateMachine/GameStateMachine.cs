using _Project.Scripts.Infrastructure.Services;
using _Project.Scripts.Infrastructure.Services.PersonPool;
using System;
using System.Collections.Generic;


namespace _Project.Scripts.Infrastructure.States
{ 
    public class GameStateMachine : IStateMachine
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;
        public GameStateMachine(Services.ServiceLocator services, SceneLoader sceneLoader)
        {
            _states = new Dictionary<Type, IState>()
            {
                [(typeof(BootstrapState))] = new BootstrapState(this,services),
                [(typeof(LoadMainState))] = new LoadMainState(this, services.Single<IStaticData>()
                  , services.Single<IGameFactory>(), sceneLoader),
                [(typeof(LoadGameState))] = new LoadGameState(this, services.Single<IStaticData>()
                  , services.Single<IGameFactory>(), services.Single<IPersonPool>(), sceneLoader),
                [(typeof(GameLoopState))] = new GameLoopState(this, services.Single<IStaticData>()
                  , services.Single<IGameFactory>(), sceneLoader)
            };
        }
        public void Enter<State>()
        {
            if (_currentState != null)
            {
                _currentState.Exit();
                _currentState = null;
            }
            _currentState = _states[typeof(State)] as IState;
            _currentState.Enter();
        }
    }

}