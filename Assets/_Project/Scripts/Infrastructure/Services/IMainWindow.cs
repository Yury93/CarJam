using _Project.Scripts.Infrastructure.States;

namespace _Project.Scripts.Infrastructure.Services
{
    public interface IMainWindow : IWindow
    {
        void Construct(IStateMachine stateMachine);
      
    }
}