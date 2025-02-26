using _Project.Scripts.Infrastructure.Services;
using _Project.Scripts.Infrastructure.Services.PersonPool;
using _Project.Scripts.StaticData;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameLogic
{
    public interface ICarData : IGridItem
    {
        public int Size { get; } 
        public MaterialProperty MaterialProperty { get; }
        public List<Transform> Placements { get; set; }
        public int CountPlace => Placements.Count;
        public int Number { get; set; }
        public Renderer RendererCar { get; }
        void SwitchColorType(MyMaterial myMaterial);
        void SetupPerson(IPerson person);
        int GetCountFreePlacment();
    }
}