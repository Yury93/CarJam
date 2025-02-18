using _Project.Scripts.Helper;
using UnityEngine;

namespace _Project.Scripts.GameLogic.TouchHandler
{
    public class TouchHandler : MonoBehaviour, ITouchHandler
    {
        [SerializeField] private float _raycastDistance = 100;
        private Camera _mainCamera;
        private int _layerMask;
        public float RaycastDistance => _raycastDistance;
        public int LayerMaskValue => _layerMask;

        public virtual void Init(string layerMaskName)
        {
            _mainCamera = Camera.main;
            _layerMask = 1 << LayerMask.NameToLayer(layerMaskName);
        }
        public virtual void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                LaunchRaycast();
            }
        }
        public virtual void HandleTouch(RaycastHit hit)
        {
            throw new System.NotImplementedException();
        }
        private void LaunchRaycast()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, RaycastDistance, layerMask: LayerMaskValue))
            {
                HandleTouch(hit);
            }
        }
    }
    public interface ITouchHandler
    {
        public float RaycastDistance { get; }
        public int LayerMaskValue { get; }
        void HandleTouch(RaycastHit hit);
    }
}