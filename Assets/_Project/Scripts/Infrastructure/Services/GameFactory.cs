using _Project.Scripts.GameLogic;
using _Project.Scripts.GridSystem; 
using _Project.Scripts.Infrastructure.Services.PersonPool;
using _Project.Scripts.Infrastructure.States;
using _Project.Scripts.StaticData;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Services
{
    public class GameFactory : IGameFactory
    {
        public const string MAIN_WINDOW_PATH = "Menu/MainWindow";
        public const string PERSON = "Game/Person/Person";
        public const string PATH_BUILDER = "Game/PathBuilder";
        public const string MAP_TAG = "CarContent";
        private IAssetProvider _assetProvider;
        private IPersonPool _personPool;
        private IStateMachine _stateMachine;
        public GameFactory(IAssetProvider assetProvider, IPersonPool personPool, IStateMachine stateMachine)
        {
            this._assetProvider = assetProvider;
            this._personPool = personPool;
            this._stateMachine = stateMachine;
        }
        public IMainWindow CreateMainWindow(States.IStateMachine stateMachine)
        {
            GameObject go = _assetProvider.instatiate(MAIN_WINDOW_PATH);
            IMainWindow mainWindow = go.GetComponent<IMainWindow>();
            mainWindow.Construct(stateMachine);
            mainWindow.Init();
            return mainWindow;
        }
        public void CreateLevel(IStaticData staticData)
        {
            LevelStaticData levelStaticData = staticData.GetLevelData(0);
            var map = GameObject.FindGameObjectWithTag(MAP_TAG);
            GameObject.FindAnyObjectByType<MiniUI>().Construct(stateMachine:_stateMachine );

            IGrid grid = CreateGrid(levelStaticData, map);

            var carPath = _assetProvider.instatiate(PATH_BUILDER).GetComponent<PathBuilder>();
            List<ICarData> cars = CreateCars(levelStaticData, map, grid,carPath);
            SetupMapPosition(levelStaticData, map);

            _personPool.CreatePool(cars);
             
            var personSpawner = GameObject.FindAnyObjectByType<PersonSpawner>();
            personSpawner.Construct(_personPool, this);
            personSpawner.SetCarStands(carPath.Stands);
            personSpawner.SpawnGroupPersons();
        }
        public IPerson CreatePerson(Vector3 position, Quaternion identity)
        {
            return _assetProvider.instatiate(PERSON, position).GetComponent<IPerson>();
        }
        private IGrid CreateGrid(LevelStaticData levelStaticData, GameObject map)
        {
            IGrid grid = new _Project.Scripts.GridSystem.Grid(_assetProvider);
            grid.CreateGrid(map.transform, levelStaticData);
            return grid;
        }
        private List<ICarData> CreateCars(LevelStaticData levelStaticData, GameObject map, IGrid grid, PathBuilder carPath)
        {
            List<ICarData> cars = new List<ICarData>();
            foreach (var gridItem in levelStaticData.Cars.GridItems)
            {
                var car = CreateCar(gridItem, grid, levelStaticData, map.transform,carPath);
                if (car != null)
                { 
                    cars.Add(car);
                }
            }
            return cars;
        }
        private Car CreateCar(IGridDirectionItem gridDirection, 
            _Project.Scripts.GridSystem.IGrid grid,
            LevelStaticData _levelData,
            Transform content, 
            PathBuilder carPath)
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
                    var mover = carInstance.GetComponent<CarMover>();
                    mover.SetPathBuilder(carPath);

                    grid.MarkCells(gridItem.Id, gridDirection, carInstance.Size);
               
                    return car; 
                }
            }
            return null;
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
        private static void SetupMapPosition(LevelStaticData levelStaticData, GameObject map)
        {
            map.transform.localRotation *= Quaternion.Euler(new Vector3(0, levelStaticData.Grid.GridRotate, 0));
            map.transform.position += levelStaticData.Grid.OffsetPosition;
        } 
    }
}