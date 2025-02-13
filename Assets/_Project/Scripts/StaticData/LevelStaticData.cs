using _Project.Scripts.Helper; 
using UnityEngine;

namespace _Project.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "Level", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        [field:SerializeField] public CarsStaticData Cars {  get; private set; }
        [field: SerializeField] public GridStaticData Grid { get; private set; }
        [field: SerializeField] public int Id { get; set; } = 0;
        [field: SerializeField] public string LevelName { get; private set; } = "Level";
 

        [Button("RndCars")]
        public void RandomCars()
        {
            foreach (var item in Cars.CarEntities)
            {
                item.RndTest();
            }
        }

    }
}
