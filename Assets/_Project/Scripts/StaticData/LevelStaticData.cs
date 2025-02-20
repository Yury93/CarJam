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
        [field:SerializeField] public Direction SetupDirection { get; private set; }

        [Button("RndCars")]
        public void RandomCars()
        {
            int num = 0;
            foreach (var item in Cars.GridItems)
            {
                item.SetRandomDirection();
                item.Number = num;
                num++;
            }
        }
        [Button("SetupDirection")]
        public void SetupSelectedDirection()
        {
            int num = 0;
            foreach (var item in Cars.GridItems)
            {
                item.Direction = SetupDirection;
                item.Number = num;
                num++;
            }
        }
    }
}
