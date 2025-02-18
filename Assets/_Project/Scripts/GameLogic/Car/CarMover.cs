using _Project.Scripts.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
 
namespace _Project.Scripts.GameLogic
{
    public class CarMover : MonoBehaviour,ICommandMove
    { 
        private int _carLayer;
        private int _mapLayer;
        private List<Vector3> _originalPath,_currentPath;
        [SerializeField] private float _speed = 10;
        [SerializeField] private float _rotationSpeed = 100;
        private List<Quaternion> _originalRotations;
        private bool isMoveProcess;
        private void Start()
        { 
            _carLayer = 1 << LayerMask.NameToLayer(Constants.CAR_LAYER);
            _mapLayer = 1 << LayerMask.NameToLayer(Constants.MAP_LAYER);

            _originalPath = new List<Vector3>();
        }
        public void CreateNewPath(List<Vector3> path)
        {
            _currentPath = new List<Vector3>(); 
            _currentPath.Add(transform.position);
            _currentPath.AddRange(path); 
            _originalPath = _currentPath; 
        }
        [Button("Move")]
        public void Move()
        {
            CreateNewPath(new List<Vector3> {transform.position + transform.forward * 3,transform.position + transform.right * 3 });
            StartCoroutine(CorMove(_originalPath));
        }
        [Button("cancelMove")]
        public void CancelMove()
        {
            if (_originalPath != null && _originalPath.Count > 0)
            {
                _currentPath = new List<Vector3>(_originalPath);
                _currentPath.Reverse();
                StartCoroutine(CorMove(_currentPath,isBack:true));
            }
        }
        private IEnumerator CorMove(List<Vector3> path,bool isBack = false)
        {
            int pathIndex = 0;
            isMoveProcess = true;
            _originalRotations = new List<Quaternion>();

            while (pathIndex < path.Count)
            {
                Vector3 nextPoint = path[pathIndex];
                Vector3 direction = (nextPoint - transform.position).normalized;

                Quaternion targetRotation = GetTargetRotation(isBack, direction); 
                _originalRotations.Add(targetRotation);

                while (Vector3.Distance(transform.position, nextPoint) > Constants.EPSILON)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                    transform.position = Vector3.MoveTowards(transform.position, nextPoint, _speed * Time.deltaTime);
                    yield return null;
                }
                pathIndex++;
            }
            isMoveProcess = false;
        }

        private Quaternion GetTargetRotation(bool isBack, Vector3 direction)
        {
            Quaternion targetRotation;
            if (isBack) targetRotation = Quaternion.LookRotation(-direction);
            else targetRotation = Quaternion.LookRotation(direction);
            return targetRotation;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject != null && 1 << collision.gameObject.layer == _carLayer)
            {
                 //CancelMove();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject != null && 1 << other.gameObject.layer == _mapLayer)
            { 
                float carPositionX = other.transform.position.x; 
                float triggerCenterX = transform.position.x;
                 
                if (carPositionX > triggerCenterX)
                {
                    Debug.Log("Машинка ближе к левому краю"); 

                }
                else
                {
                    Debug.Log("Машинка ближе к правому краю"); 
                }
            }
        }
    }  
}