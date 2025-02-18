using _Project.Scripts.Helper;
using UnityEngine;

namespace _Project.Scripts.GameLogic.TouchHandler
{
    public class CarTouchHandler : TouchHandler
    {
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
            hit.collider.gameObject.GetComponent<CarMover>().Move();
        }
    }
}