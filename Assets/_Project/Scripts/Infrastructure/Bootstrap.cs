
using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Infrastructure
{
    public class Bootstrap : MonoBehaviour , ICoroutineRunner
    {
        public Game game;

        private void Awake()
        {
            game = new Game (Services.ServiceLocator.Container,new SceneLoader(this));
            DontDestroyOnLoad(gameObject);
        }
         
    }
}