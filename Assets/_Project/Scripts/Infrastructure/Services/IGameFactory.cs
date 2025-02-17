using _Project.Scripts.GameLogic;
using _Project.Scripts.Infrastructure.States; 
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Services
{
    public interface IGameFactory : IService
    {
        void CreateLevel(IStaticData staticData);
        IMainWindow CreateMainWindow(IStateMachine _stateMachine);
        IPerson CreatePerson(Vector3 position, Quaternion identity);
    }
}