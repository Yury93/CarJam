

using _Project.Scripts.Infrastructure.Services;
using System;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private GameStateMachine _gameStateMachine;
        private IStaticData _staticData;
        private IGameFactory _gameFactory;
        private SceneLoader _sceneLoader;

        public GameLoopState(GameStateMachine gameStateMachine, IStaticData staticData, IGameFactory gameFactory, SceneLoader sceneLoader)
        {
            this._gameStateMachine = gameStateMachine;
            this._staticData = staticData;
            this._gameFactory = gameFactory;
            this._sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            Debug.LogError("Добрались до гейм лупа");
        }

        private void OnLoaded()
        {
             
        }

        public void Exit()
        {
     
        }
    }
}