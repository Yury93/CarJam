using _Project.Scripts.GameLogic;
using _Project.Scripts.GridSystem; 
using _Project.Scripts.Infrastructure.Services.PersonPool;
using _Project.Scripts.Infrastructure.States;
using _Project.Scripts.StaticData;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Services
{
    public class GameFactory : IGameFactory
    {
        public const string MAIN_WINDOW_PATH = "MainWindow";
        public const string PERSON = "Person";
        public const string PATH_BUILDER = "PathBuilder";
        public const string MAP_TAG = "CarContent";
        public const string WHITE_MATERIAL = "CarMaterial";
        private IAssetProvider _assetProvider;
        private IPersonPool _personPool;
        private IStateMachine _stateMachine;
        private IMaterialsPool _materialPool;
        public GameFactory(IAssetProvider assetProvider, IPersonPool personPool, IStateMachine stateMachine, IMaterialsPool poolColors)
        {
            this._assetProvider = assetProvider;
            this._personPool = personPool;
            this._stateMachine = stateMachine;
            this._materialPool = poolColors;
        }
        public async Task<IMainWindow> CreateMainWindowAsync(States.IStateMachine stateMachine)
        {
            var task =_assetProvider.instatiateAsync(MAIN_WINDOW_PATH) ;
            await task;
            var go = task.Result;
            IMainWindow mainWindow = go.GetComponent<IMainWindow>();
            mainWindow.Construct(stateMachine);
            mainWindow.Init();
            return mainWindow;
        }
        public void ReleaseAssets()
        {
            _assetProvider.ReleaseAssets();
        }
        public async void CreateLevelAsync(IStaticData staticData)
        {
            LevelStaticData levelStaticData = staticData.GetLevelData(Saver.Saver.GetLastLevel());
            GameObject map = GameObject.FindGameObjectWithTag(MAP_TAG);
            GameObject.FindAnyObjectByType<MiniUIInfo>().Construct(stateMachine: _stateMachine);
            var helicopter = GameObject.FindAnyObjectByType<FlyObject>();
            var deleteCar  = GameObject.FindAnyObjectByType<DeleteCar>();
            var sortPersons = GameObject.FindAnyObjectByType<SortPersons>();

            await CreateMaterials(levelStaticData);
        
            Task<IGrid> gridTask = CreateGrid(levelStaticData, map);
            await gridTask;

            Task<GameObject> pathBuildTask = _assetProvider.instatiateAsync(PATH_BUILDER);
            await pathBuildTask;

            PathBuilder pathBuilder = pathBuildTask.Result.GetComponent<PathBuilder>();

            helicopter.Init(pathBuilder.Stands);
            deleteCar.Init(helicopter);
            sortPersons.Construct(pathBuilder);

            List<ICarData> cars = CreateCars(levelStaticData, map, gridTask.Result, pathBuilder);
            SetupMapPosition(levelStaticData, map);

            var switcherColor = GameObject.FindAnyObjectByType<SwitcherColorCar>();
            switcherColor.Construct(_materialPool,_personPool);
            switcherColor.Init();

            _personPool.CreatePool(cars);
            
            var personSpawner = GameObject.FindAnyObjectByType<PersonSpawner>();
            personSpawner.Construct(_personPool, this);
            personSpawner.SetCarStands(pathBuilder.Stands);
            personSpawner.SpawnGroupPersons();
        }

        private async Task CreateMaterials(LevelStaticData levelStaticData)
        {
            var materialProperties = levelStaticData.MaterialsData.GetColors();
            foreach (var materialProperty in materialProperties)
            {
                var colorTask = _assetProvider.instatiateMaterialAsync(WHITE_MATERIAL);
                await colorTask;
                var carMaterial = colorTask.Result;
                carMaterial.SetColor( PoolMaterials.MATERIAL_COLOR_KEY,materialProperty.Color);
                _materialPool.AddMaterial(new MyMaterial(carMaterial,
                    new MaterialProperty(materialProperty.Color, materialProperty.ColorTag)));
            }
        }

        public async Task<IPerson> CreatePersonAsync(Vector3 position, Quaternion identity)
        {
            var task = _assetProvider.instatiateAsync(PERSON, position);
            await task;
            var person = task.Result.GetComponent<IPerson>();
            return person;
        }
        private async Task<IGrid> CreateGrid(LevelStaticData levelStaticData, GameObject map)
        {
            IGrid grid = new _Project.Scripts.GridSystem.Grid(_assetProvider);
            await grid.CreateGrid(map.transform, levelStaticData);
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
            LevelStaticData levelData,
            Transform content, 
            PathBuilder carPath)
        {
            
            foreach (var gridItem in grid.GridItems)
            {
                Car car = levelData.Cars.CarPrefabs.First(c => c.Id == gridDirection.Id);
                bool canPlace = grid.CanPlace(gridItem.Id, gridDirection.Direction, car.Size);
                if (canPlace)
                {
                    Car carInstance = GameObject.Instantiate(car, content); 
                    carInstance.Init(gridDirection);
                     
                    SetupCarPosition(gridDirection, gridItem, carInstance);
                    CarMover mover = carInstance.GetComponent<CarMover>();
                    mover.SetPathBuilder(carPath);

                    MaterialProperty materialProp = carInstance.MaterialProperty;
                    MyMaterial myMaterial = _materialPool.GetMaterial(materialProp.ColorTag); 
                    carInstance.SwitchColorType(myMaterial);

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