using _Project.Scripts.Configs;
using _Project.Scripts.Helper;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GridSystem
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        public List<GameObject> cars = new List<GameObject>();
         
        [Button("Create Grid")]
        public void CreateGrid()
        {
            int carIndex = 0;
            cars.ForEach(c => Destroy(c.gameObject));
            cars.Clear();
            for (int line = 0; line < levelData.Line; line++)
            {
                for (int column = 0; column < levelData.Collumn; column++)
                {
                    if (carIndex >= levelData.Cars.Count)
                    {
                        carIndex = 0;
                    }
                    CarEntity carEntity = levelData.Cars[carIndex];
                    GameObject car = Instantiate(carEntity.Car, Vector3.zero, Quaternion.identity);

                    Vector3 carSize = GetCarSize(car);
                    float carMaxSize = Mathf.Max(carSize.x, carSize.z);
                    float carMinSize = Mathf.Min(carSize.x, carSize.z);
                    Vector3 spawnPosition = new Vector3(column * carMaxSize, 0, line * carMaxSize) + levelData.OffsetPosition;

                    car.transform.position = spawnPosition;

                    cars.Add(car);
                    carIndex++;
                }
            }
        }
        public Vector3 GetCarSize(GameObject carPrefab)
        {
            Collider collider = carPrefab.GetComponent<Collider>();
            if (collider != null)
            {
                return collider.bounds.size;
            }

            return Vector3.one;
        }
    }
}