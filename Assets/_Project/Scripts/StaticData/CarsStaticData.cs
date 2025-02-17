using _Project.Scripts.GameLogic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.StaticData
{
    [Serializable]
    public class CarsStaticData
    {
        [field: SerializeField] public List<GridItem> GridItems { get; private set; }
        [field: SerializeField] public List<Car> CarPrefabs { get; private set; }
    }
}
