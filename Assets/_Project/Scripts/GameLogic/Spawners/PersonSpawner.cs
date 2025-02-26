using _Project.Scripts.Helper;
using _Project.Scripts.Infrastructure.Services;
using _Project.Scripts.Infrastructure.Services.PersonPool;
using _Project.Scripts.StaticData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;

namespace _Project.Scripts.GameLogic
{
    public class PersonSpawner : MonoBehaviour
    {
        [SerializeField] private int _maxPersonOnLevel = 42;
        [SerializeField] private List<IPerson> _personsOnLevel = new List<IPerson>();
        [SerializeField] private float _spawnDelay = 0.1f;
        [SerializeField] private List<Transform> _path;
        [SerializeField] private List<CarStand> _carStands;
        public List<IPerson> PersonOnLevel => _personsOnLevel;
        private Coroutine _corSpawn;
        private IPersonPool _personPool;
        private IGameFactory _gameFactory;
        private int _number = 0;
        private IPerson _perviousPerson;
        public void Construct(IPersonPool personPool, IGameFactory gameFactory)
        {
            this._personPool = personPool;
            this._gameFactory = gameFactory;
        }
        public void SetCarStands(List<CarStand> carStands)
        {
            _carStands = carStands;
            foreach (var stand in carStands)
            {
                stand.onStopCar += CheckStopCar;
            }
        }
        public void CheckStopCar(Car car)
        {
            StartCoroutine(CorOnStop());

            IEnumerator CorOnStop()
            {
                yield return new WaitForSeconds(1f);
                while (HasPersonColor())
                {
                    yield return StartCoroutine(CorSitPerson(car));
                }
            }

            IEnumerator CorSitPerson(Car car)
            {
                foreach (var stand in _carStands)
                {
                    if (stand.Free || stand.Car == null) continue;

                    var continuousGroup = FindContinuousGroup(stand.Car.MaterialProperty.ColorTag);

                    if (continuousGroup.Count > 0)
                    {
                        foreach (var person in continuousGroup)
                        {
                            if (stand.Car.IsFull == false)
                            {
                                stand.Car.SetupPerson(person); 
                                _personsOnLevel.Remove(person);
                                stand.Car.OnSitPerson(person);
                                SpawnGroupPersons();
                            }
                        }

                        if (stand.Car.IsFull)
                        {
                            stand.CarExit();
                            yield return new WaitForSeconds(1f);
                        }
                    }
                }
            }
        }
        private List<IPerson> FindContinuousGroup(ColorTag targetColor)
        {
            var continuousGroup = new List<IPerson>(); 
            foreach (var person in _personsOnLevel)
            { 
                if (person.ColorTag == targetColor)
                { 
                    continuousGroup.Add(person);
                }
                else
                { 
                    break;
                }
            }

            return continuousGroup;
        }
        private bool HasPersonColor()
        {
            foreach (var stand in _carStands)
            {
                if (stand.Car == null) continue;
                 
                var continuousGroup = FindContinuousGroup(stand.Car.MaterialProperty.ColorTag);

                if (continuousGroup.Count > 0)
                { 
                    return true;
                }
            }
             
            return false;
        }

        public void SpawnGroupPersons()
        {
            var poolPersons = _personPool.GetPersonsGroup();
            if (poolPersons.Count == 0)
            {
                Debug.LogError("В пуле больше нет человечков");
                MiniUIInfo.instance.ShowQueue(0);
                return;
            }

            if (_corSpawn != null) StopCoroutine(_corSpawn);
            _corSpawn = StartCoroutine(CorSpawn(poolPersons, _spawnDelay));
        }
        private IEnumerator CorSpawn(List<MaterialProperty> materialProperties, float delay)
        { 
            foreach (var materialProperty in materialProperties)
            {
                if (_personsOnLevel.Count < _maxPersonOnLevel)
                {
                    yield return new WaitUntil(() => _perviousPerson == null
                     || _perviousPerson != null
                     && Vector3.Distance(_perviousPerson.MyTransform.position, transform.position) > _perviousPerson.Space);

                    var task = _gameFactory.CreatePersonAsync(transform.position, Quaternion.identity);
                    yield return new WaitUntil(() => task.IsCompleted);
                    var person = task.Result;
                    person.MyTransform.SetParent(this.transform);
                    person.Init(materialProperty);
                    person.SetPath(_path);
                    person.MoveToPath();
                    person.Number = _number;
                    _personsOnLevel.Add(person);
                    _personPool.RemovePersonEntity(materialProperty);
                    _number++;
                    _perviousPerson = person;
                    yield return null;
                } 
            }
        }
        [Button("cal log")]
        public void CallLog()
        {
            foreach (var item in _personsOnLevel)
            {
                Debug.Log("Номер чловечека " + item.Number);
            }
        }
    }
}