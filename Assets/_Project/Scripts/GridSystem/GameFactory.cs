using _Project.Scripts.StaticData;
using _Project.Scripts.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;  

namespace _Project.Scripts.GridSystem
{
    public class GameFactory : MonoBehaviour
    {
        [SerializeField] private LevelStaticData _levelData;
        [SerializeField] private GridSystem.Grid _grid;
        [SerializeField] private Transform _content;
        public List<Car> cars = new List<Car>();
    
        [Button("Create Grid")] 
        public void CreateGrid()
        {
            _grid.CreateGrid(this.transform, _levelData);
            foreach (var carEntity in _levelData.Cars.CarEntities)
            {
                CreateCar(carEntity);
            }
            _content.localRotation = Quaternion.Euler(new Vector3(0, _levelData.Grid.GridRotate, 0));
        } 
        private void CreateCar(CarEntity carEntity)
        {
            foreach (var gridItem in _grid.GridItems)
            {
                Car car = _levelData.Cars.CarPrefabs.First(c => c.Id == carEntity.Id);
                bool canPlace = _grid.CanPlace(gridItem.Id, carEntity.Direction, car.Size);
                if (canPlace)
                {
                    Car carInstance = Instantiate(car, _content);
                    carInstance.Init(carEntity);
                    SetupCarPosition(carEntity, gridItem, carInstance);

                    cars.Add(carInstance);
                    _grid.MarkCellsForCar(gridItem.Id, carEntity, carInstance.Size);

                    break;
                }
            }
        } 
        private void SetupCarPosition(CarEntity carEntity, GridPoint gridItem, Car carInstance)
        {
            Vector3 targetPosition = gridItem.Position;
            Quaternion startRotation = carEntity.GetStartRotation();
            carInstance.transform.rotation = startRotation;
            Vector3 startPositionOffset = carInstance.CarDirection;
            Vector3 worldStartPosition = carInstance.transform.TransformPoint(startPositionOffset);
            Vector3 correctedPosition = targetPosition - (worldStartPosition - carInstance.transform.position);
            carInstance.transform.position = correctedPosition;
        }
    }
}