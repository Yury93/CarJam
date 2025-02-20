using _Project.Scripts.Infrastructure.Services;
using _Project.Scripts.Infrastructure.Services.PersonPool;
using Unity.VisualScripting;
using UnityEngine;


namespace _Project.Scripts.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private IStateMachine _stateMachine;
        public BootstrapState(IStateMachine stateMachine, Services.ServiceLocator services)
        {
            _stateMachine = stateMachine;
            var assetProvider = new AssetProvider();
            services.RegisterSingle<IAssetProvider>(assetProvider);
            services.RegisterSingle<IPersonPool>(new PersonPool());
            services.RegisterSingle<IGameFactory>(new GameFactory(assetProvider,services.Single<IPersonPool>(),stateMachine));
            services.RegisterSingle<IStaticData>(new _Project.Scripts.Infrastructure.Services.StaticData());
          
        }
        public void Enter()
        {
             _stateMachine.Enter<LoadMainState>();
        }

        public void Exit()
        {
            
        }
    }

}