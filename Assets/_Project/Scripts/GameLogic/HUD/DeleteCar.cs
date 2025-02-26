using _Project.Scripts.GameLogic;
using _Project.Scripts.GameLogic.TouchHandler;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace _Project.Scripts.Infrastructure.Services.PersonPool
{
    public class DeleteCar : MonoBehaviour
    {
        [SerializeField] private Button deleteCarButton;
        [SerializeField] private CarTouchHandler carTouchHandler;
        [SerializeField] private FlyObject flyObject;
      
        public void Init(FlyObject flyObject)
        {
            this.flyObject = flyObject;
            deleteCarButton.onClick.AddListener(ClickDeleteCar);
        }
        private void ClickDeleteCar()
        {
            carTouchHandler.ovverideTouchCar = HandleDeleteCar;
        } 
        private void HandleDeleteCar(CarMover mover)
        { 
            carTouchHandler.ovverideTouchCar = null;
            flyObject.ToCar(mover.GetComponent<Car>());
        } 
    }
}