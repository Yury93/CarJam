using _Project.Scripts.Infrastructure.Services.PersonPool;
using UnityEngine;

namespace _Project.Scripts.GameLogic
{
    public interface IPerson : IPathMovable, IRaycastChecker
    {
        Transform MyTransform { get; }
        float Speed { get; }
        void Init(PersonEntity personEntity);
    }
}
