﻿using _Project.Scripts.Infrastructure.Services.PersonPool;
using _Project.Scripts.StaticData;
using UnityEngine;

namespace _Project.Scripts.GameLogic
{
    public interface IPerson : IPathMovable, IRaycastChecker
    {
        public int Number { get; set; }
        Transform MyTransform { get; } 
        Color Color { get; }
        ColorTag ColorTag { get; }
        float Speed { get; set; }
        bool InCar { get; set; }
       float Space {  get;  }
        MaterialProperty MaterialProperty { get; }
        void Init(MaterialProperty personEntity);
        void RefreshColor(MaterialProperty materialProp);
        void  StopCoroutineMove();
    }
}
