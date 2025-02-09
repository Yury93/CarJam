using _Project.Scripts.Configs;
using _Project.Scripts.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;  

namespace _Project.Scripts.GridSystem
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private Configs.Grid grid;
        [SerializeField] private Transform content;
        public List<Car> cars = new List<Car>();
    
        [Button("Create Grid")] 
        public void CreateGrid()
        {
            grid.CreateGrid(this.transform, levelData);
            foreach (var carEntity in levelData.CarEntities)
            {
                CreateCar(carEntity);
            }
        } 
        private void CreateCar(CarEntity carEntity)
        {
            foreach (var gridItem in grid.GridItems)
            {
                Car car = levelData.Cars.First(c => c.Id == carEntity.Id);
                bool canPlace = grid.CanPlace(gridItem.Id, carEntity.Direction, car.Size);
                if (canPlace)
                { 
                    Car carInstance = Instantiate(car, content);
                    carInstance.Init(carEntity); 
                    Vector3 targetPosition = gridItem.Position;
                     
                    Quaternion startRotation = carEntity.GetStartRotation();
                    carInstance.transform.rotation = startRotation;
                     
                    Vector3 startPositionOffset = carInstance.CarDirection;
                     
                    Vector3 worldStartPosition = carInstance.transform.TransformPoint(startPositionOffset);
                     
                    Vector3 correctedPosition = targetPosition - (worldStartPosition - carInstance.transform.position);
                     
                    carInstance.transform.position = correctedPosition;
                     
                    cars.Add(carInstance); 
                    grid.MarkCellsAsOccupied(gridItem.Id, carEntity, carInstance.Size);  

                    // Отладка
                    Debug.Log($"Target Position: {targetPosition}");
                    Debug.Log($"Corrected Position: {correctedPosition}");
                    Debug.Log($"StartPoint Local Position: {startPositionOffset}");
                    Debug.Log($"World Start Position: {worldStartPosition}");

                    break;
                }
            }
        }  
    }
}