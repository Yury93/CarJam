using _Project.Scripts.Helper;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "Level", menuName = "Configs/Levels", order = 51)]
    public class LevelData : ScriptableObject
    {
        [field: SerializeField] public string LevelName { get; private set; } = "Level ";
        [field: SerializeField] public int CarCount { get; private set; } = 1;
        [field: SerializeField] public int Column { get; private set; } = 1;
       
        [field: Range(1, 3), SerializeField] public float Space { get; private set; } = 1.1f;
        [field:SerializeField] public List<CarEntity> Cars { get; private set; }
        
        public int Lines => CarCount/ Column;
       

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
