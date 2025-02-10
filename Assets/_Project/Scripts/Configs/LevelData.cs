using _Project.Scripts.Helper;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "Level", menuName = "Configs/Levels", order = 51)]
    public class LevelData : ScriptableObject
    {
        [field: SerializeField] public List<CarEntity> CarEntities { get; private set; }
        [field:SerializeField] public List<Car> Cars { get; private set; } 
        [field: SerializeField] public string LevelName { get; private set; } = "Level ";
        [field: Range(0, 3), SerializeField] public float Space { get; private set; } = 1.1f;
        [field: SerializeField] public int Columns { get; private set; } = 5;
        [field: SerializeField] public int Lines { get; private set; } = 5;
        [field: SerializeField] public float CellSize { get; private set; } = 1;
        [Button("RndCars")]
        public void RandomCars()
        {
            foreach (var item in CarEntities)
            {
                item.RndTest();
            }
        }

    }
}
