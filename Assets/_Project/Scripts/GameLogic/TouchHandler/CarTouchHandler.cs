using _Project.Scripts.Helper;
using System;
using UnityEngine;

namespace _Project.Scripts.GameLogic.TouchHandler
{
    public class CarTouchHandler : TouchHandler
    {
        public Action<CarMover> ovverideTouchCar;
        private void Start()
        {
            Init(Constants.CAR_LAYER);
        }
        public override void Init(string layerMaskName)
        {
            base.Init(layerMaskName);
        }
        public override void HandleTouch(RaycastHit hit)
        {
            if (ovverideTouchCar != null)
            {
               var mover = hit.collider.gameObject.GetComponent<CarMover>();
                mover.SetTouch();
                ovverideTouchCar.Invoke(mover);
            }
            else
                hit.collider.gameObject.GetComponent<CarMover>().Move();
        }
    }
}