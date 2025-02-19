using _Project.Scripts.Infrastructure.Services.PersonPool;
using UnityEngine;

namespace _Project.Scripts.GameLogic
{
    public interface IPerson : IPathMovable, IRaycastChecker
    {
        Transform MyTransform { get; }
        float Speed { get; }
        Color Color { get; }
        bool InCar { get; set; }
        void Init(PersonEntity personEntity);
    }
}
