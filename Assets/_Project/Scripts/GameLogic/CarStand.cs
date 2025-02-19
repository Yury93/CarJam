using _Project.Scripts.GameLogic;
using _Project.Scripts.Helper;
using System;
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
        public Car Car { get; set; }
        public event Action<Car> onStopCar;

        public void SetWaitCar(bool waitCarProcess)
        {
            WaitCarProcess = waitCarProcess;
        }

        internal void CarExit()
        {
            throw new NotImplementedException();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (1 << other.gameObject.layer == 1 << LayerMask.NameToLayer(Constants.CAR_LAYER))
            {
                SetWaitCar(true);
                Car = other.gameObject.GetComponent<Car>();

                onStopCar?.Invoke(Car);
            }
        }
    }
}