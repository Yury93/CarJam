using _Project.Scripts.GameLogic;
using _Project.Scripts.GridSystem;
using _Project.Scripts.Infrastructure.States;
using _Project.Scripts.StaticData;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Services
{
    public interface IGameFactory : IService
    {
    Task<IMainWindow> CreateMainWindowAsync(States.IStateMachine stateMachine);
       void CreateLevelAsync(IStaticData staticData);
         Task<IPerson> CreatePersonAsync(Vector3 position, Quaternion identity);
    }
}