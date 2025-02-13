using _Project.Scripts.GameLogic;
using _Project.Scripts.GridSystem;
using _Project.Scripts.StaticData;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Services
{
    public class GameFactory : IGameFactory
    {
        public const string MAIN_WINDOW_PATH = "Menu/MainWindow";
        public const string GRID_POINT_PATH = "Grid/GridPoint";
        public const string MAP_TAG = "Map";
        private IAssetProvider assetProvider;

        public GameFactory(IAssetProvider assetProvider)
        {
            this.assetProvider = assetProvider;
        }
        public IMainWindow CreateMainWindow(States.IStateMachine stateMachine)
        {
            GameObject go = assetProvider.instatiate(MAIN_WINDOW_PATH);
            IMainWindow mainWindow = go.GetComponent<IMainWindow>();
            mainWindow.Construct(stateMachine);
            mainWindow.Init();
            return mainWindow;
        }
        public void CreateGrid(IStaticData staticData)
        {
            GameObject gridPoint = assetProvider.instatiate(GRID_POINT_PATH);
            IGrid grid = new _Project.Scripts.GridSystem.Grid(gridPoint.GetComponent<GridPoint>());

            LevelStaticData levelStaticData = staticData.GetLevelData(0);

            var map  = GameObject.FindGameObjectWithTag(MAP_TAG);
            grid.CreateGrid(map.transform, levelStaticData);

            foreach (var carEntity in levelStaticData.Cars.CarEntities)
            {
                CreateCars(carEntity,grid,levelStaticData,map.transform);
            }

            map.transform.localRotation = Quaternion.Euler(new Vector3(0, levelStaticData.Grid.GridRotate, 0));
        }
        private void CreateCars(IGridDirectionEntity gridDirection, _Project.Scripts.GridSystem.IGrid grid, LevelStaticData _levelData,Transform content)
        {
            foreach (var gridItem in grid.GridItems)
            {
                Car car = _levelData.Cars.CarPrefabs.First(c => c.Id == gridDirection.Id);
                bool canPlace = grid.CanPlace(gridItem.Id, gridDirection.Direction, car.Size);
                if (canPlace)
                {
                    Car carInstance = GameObject.Instantiate(car, content);
                    carInstance.Init(gridDirection);
                    SetupCarPosition(gridDirection, gridItem, carInstance);

                    grid.MarkCells(gridItem.Id, gridDirection, carInstance.Size);

                    break;
                }
            }
        }
        private void SetupCarPosition(IDirectionEntity directionEntity, GridPoint gridItem, Car carInstance)
        {
            Vector3 targetPosition = gridItem.Position;
            Quaternion startRotation = directionEntity.GetTransformDirection();
            carInstance.transform.rotation = startRotation;
            Vector3 startPositionOffset = carInstance.GetDirection;
            Vector3 worldStartPosition = carInstance.transform.TransformPoint(startPositionOffset);
            Vector3 correctedPosition = targetPosition - (worldStartPosition - carInstance.transform.position);
            carInstance.transform.position = correctedPosition;
        }
    }
}