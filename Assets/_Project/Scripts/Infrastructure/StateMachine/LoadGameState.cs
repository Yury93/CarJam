﻿using _Project.Scripts.Infrastructure.Services;
using _Project.Scripts.Infrastructure.Services.PersonPool;
using System;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.States
{
    internal class LoadGameState : IState
    {
        private GameStateMachine _gameStateMachine;
        private IStaticData _staticData;
        private IGameFactory _gameFactory;
        private SceneLoader _sceneLoader;
        private IPersonPool _personPool;

        public LoadGameState(GameStateMachine gameStateMachine, 
            IStaticData staticData, 
            IGameFactory gameFactory,
            IPersonPool personPool, 
            SceneLoader sceneLoader)
        {
            this._gameStateMachine = gameStateMachine;
            this._staticData = staticData;
            this._gameFactory = gameFactory;
            this._sceneLoader = sceneLoader;
            this._personPool = personPool;
        } 
        public async void Enter()
        {
            await _gameFactory.ReleaseAssetsAsync();
            _sceneLoader.Load("Loaded", onLoaded: () => OnLoadScene()); 
        } 
        private void OnLoadScene()
        {
            int currentScene = Saver.Saver.GetCurrentScene();
            _sceneLoader.Load(_staticData.GetScene(currentScene).SceneKey, onLoaded: () => OnLoadedLevel());
        } 
        private void OnLoadedLevel()
        {
            _gameFactory.CreateLevelAsync(_staticData);
            _gameStateMachine.Enter<GameLoopState>(); 
        } 
        public void Exit()
        {   
        }
    }
}