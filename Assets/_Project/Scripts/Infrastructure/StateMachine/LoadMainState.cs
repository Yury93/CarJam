
using _Project.Scripts.Infrastructure.Services;
using System;
using UnityEngine;


namespace _Project.Scripts.Infrastructure.States
{
    public class LoadMainState : IState
    {
        private IStaticData _staticData;
        private IGameFactory _gameFactory;
        private IStateMachine _stateMachine;
        private SceneLoader _sceneLoader;
        public const string MAIN_SCENE = "Main";
        public LoadMainState(IStateMachine stateMachine, IStaticData staticData , IGameFactory gameFactory, SceneLoader sceneLoader)
        {
            this._stateMachine = stateMachine; 
            this._gameFactory = gameFactory;
            this._sceneLoader = sceneLoader;
            this._staticData = staticData;
        }

        public void Enter()
        {
            _staticData.LoadData();
            _sceneLoader.Load( MAIN_SCENE,onLoaded:()=> OnLoaded());
        }
        private void OnLoaded()
        {
            _gameFactory.CreateMainWindow(_stateMachine);
        }
        public void Exit()
        {
            
        }
    }
    
}