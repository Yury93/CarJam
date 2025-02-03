using _Project.Scripts.Helper;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "Level", menuName = "Configs/Levels", order = 51)]
    public class LevelData : ScriptableObject
    {
        [field: SerializeField] public string LevelName { get; private set; } = "Level ";
        [field: SerializeField] public int Line { get; private set; } = 3;
        [field: SerializeField] public int Collumn { get; private set; } = 4;
        [field: SerializeField] public Vector3 OffsetPosition { get; private set; }   
        [field:SerializeField] public List<CarEntity> Cars { get; private set; }

        [Button("Set ids")]
        public void SetIds()
        {
            for (int i = 0; i < Cars.Count; i++)
            {
                Cars[i].Id = i;
            }
        } 
    }
}
