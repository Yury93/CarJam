using _Project.Scripts.Infrastructure.Services;
using _Project.Scripts.Infrastructure.Services.PersonPool;
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
        private Coroutine corSpawn;
        private IPersonPool _personPool;
        private IGameFactory _gameFactory;
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
                stand.onStopCar += OnStopCar;
            }
        }

        private void OnStopCar(Car car)
        {
            Debug.Log("Человечки садятся в транспорт. Если мест в машинке нет, то машинка уезжает");
        }

        public void SpawnGroupPersons()
        {
            var poolPersons = _personPool.GetPersonsGroup();
            if (poolPersons.Count == 0)
            {
                Debug.LogError("В пуле больше нет человечков");
                return;
            }

            if (corSpawn != null) StopCoroutine(corSpawn);

            corSpawn = StartCoroutine(CorSpawn(poolPersons, _spawnDelay));
        }
        private IEnumerator CorSpawn(List<PersonEntity> persons, float delay)
        {
            foreach (var personItem in persons)
            {
                if (_personsOnLevel.Count < _maxPersonOnLevel)
                {
                    yield return new WaitForSeconds(delay);
                    var person = _gameFactory.CreatePerson(transform.position, Quaternion.identity);
                    person.MyTransform.SetParent(this.transform);
                    person.Init(personItem);
                    person.SetPath(_path);
                    person.MoveToPath();
                    _personsOnLevel.Add(person);
                    _personPool.RemovePersonEntity(personItem);
                }
            }
        }
    }
}