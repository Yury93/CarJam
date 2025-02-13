
namespace _Project.Scripts.Infrastructure.States
{
    public interface IStateMachine
    {
        void Enter<IState>();
    }

}