using _Project.Scripts.GameLogic;
using _Project.Scripts.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameLogic
{
    public class CarStand : MonoBehaviour
    {
        [SerializeField] private List<Transform> _backMovePoints;
        [field: SerializeField] public Transform RoadPointIn { get; set; }
        [field: SerializeField] public bool Free { get; set; } = true;
        [field: SerializeField] public bool WaitCarProcess { get; set; }
        [field: SerializeField] public Car Car { get; set; }
        public event Action<Car> onStopCar;
        private Coroutine _corWaiter;
        public void SetWaitCar(bool waitCarProcess)
        {
            WaitCarProcess = waitCarProcess;
        }
        public void CarExit()
        {
            StartCoroutine(CorSetFreeStand()); 
            IEnumerator CorSetFreeStand()
            { 
                var carMover = Car.GetComponent<CarMover>(); 
                carMover.GetComponent<Collider>().enabled = false;
                carMover.OnExit();
                Free = true;
                Car = null;
                var path = new List<Vector3>();
                foreach (var item in _backMovePoints)
                {
                    path.Add(item.position);
                } 
                yield return  StartCoroutine(carMover.CorMove(path, true));
           
                path.Clear();
                path.Add(carMover.transform.position + Vector3.right * 30);
                yield return  StartCoroutine(carMover.CorMove(path));
                
                Destroy(carMover.gameObject);

                
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (1 << other.gameObject.layer == 1 << LayerMask.NameToLayer(Constants.CAR_LAYER))
            {
                if (_corWaiter != null) StopCoroutine(_corWaiter);
                SetWaitCar(true);
                Car = other.gameObject.GetComponent<Car>();
                var mover = other.gameObject.GetComponent<CarMover>();
                mover.IsStand = true;
                onStopCar?.Invoke(Car);
            }
        }
    }
}