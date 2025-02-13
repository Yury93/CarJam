using _Project.Scripts.Infrastructure.States;
using System;
using UnityEditor.Toolbars;

namespace _Project.Scripts.Infrastructure.Services
{
    public interface IGameFactory : IService
    {
        void CreateGrid(IStaticData staticData);
        IMainWindow CreateMainWindow(IStateMachine _stateMachine);
    }
}