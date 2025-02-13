using _Project.Scripts.Infrastructure.States;

namespace _Project.Scripts.Infrastructure
{
    public class Game
    {
        public GameStateMachine _stateMachine;
        public Game(Services.ServiceLocator services, SceneLoader sceneLoader)
        {
            _stateMachine = new GameStateMachine(services, sceneLoader);
            _stateMachine.Enter<BootstrapState>(); 
        }
    }
}