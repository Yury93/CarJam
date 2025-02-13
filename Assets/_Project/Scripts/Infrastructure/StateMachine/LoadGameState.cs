using _Project.Scripts.Infrastructure.Services;
using System;

namespace _Project.Scripts.Infrastructure.States
{
    internal class LoadGameState : IState
    {
        private GameStateMachine _gameStateMachine;
        private IStaticData _staticData;
        private IGameFactory _gameFactory;
        private SceneLoader _sceneLoader;

        public LoadGameState(GameStateMachine gameStateMachine, IStaticData staticData, IGameFactory gameFactory, SceneLoader sceneLoader)
        {
            this._gameStateMachine = gameStateMachine;
            this._staticData = staticData;
            this._gameFactory = gameFactory;
            this._sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            int currentScene = Saver.Saver.GetCurrentScene();
            _sceneLoader.Load(_staticData.GetScene(currentScene).SceneKey, onLoaded: () => OnLoaded());
          
        }

        private void OnLoaded()
        {
            _gameFactory.CreateGrid(_staticData);
            _gameStateMachine.Enter<GameLoopState>(); 
        }

        public void Exit()
        {
             
        }
    }
}