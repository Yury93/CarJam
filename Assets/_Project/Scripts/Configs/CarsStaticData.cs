using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.StaticData
{
    [Serializable]
    public class CarsStaticData
    {
        [field: SerializeField] public List<CarEntity> CarEntities { get; private set; }
        [field: SerializeField] public List<Car> CarPrefabs { get; private set; }
    }
}
