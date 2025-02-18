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
        [SerializeField] private float Speed = 10;
        [SerializeField] private float RotationSpeed;
        private List<Quaternion> _originalRotations;
        private bool isMoveProcess;
        private void Start()
        { 
            _carLayer = 1 << LayerMask.NameToLayer(Constants.CAR_LAYER);
            _mapLayer = 1 << LayerMask.NameToLayer(Constants.MAP_LAYER);
        }
        //[Button("Move")]
        //public void Move()
        //{ 
        //    _currentPath = new List<Vector3>(); 
             
        //    _currentPath.Add(transform.position + transform.forward * 3f);
        //    _currentPath.Add(transform.position + transform.right * 3f);
             
        //    _originalPath = _currentPath;
        //    StartCoroutine(CorMoveForward(_originalPath));
        //}
        //[Button("cancelMove")]
        //public void CancelMove()
        //{
        //    if (_originalPath != null && _originalPath.Count > 0)
        //    {
        //        _currentPath = new List<Vector3>(_originalPath);
        //        _currentPath.Reverse(); 
        //        StartCoroutine(CorMoveBack(_currentPath));
        //    }
        //}
        //IEnumerator CorMoveForward(List<Vector3> path)
        //{
        //    int pathIndex = 0;
        //    isMoveProcess = true;
        //    _originalRotations = new List<Quaternion>();
        //    while (pathIndex < path.Count)
        //    {
        //        Vector3 nextPoint = path[pathIndex];
        //        while (Vector3.Distance(transform.position, nextPoint) > Constants.EPSILON)
        //        {
        //            Vector3 direction = (nextPoint - transform.position).normalized;
        //            Quaternion targetRotation = Quaternion.LookRotation(direction);
        //            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        //            transform.position = Vector3.MoveTowards(transform.position, nextPoint, Speed * Time.deltaTime);
        //            _originalRotations.Add(targetRotation);
        //            yield return null;
        //        }
        //        pathIndex++;
        //    }
        //    isMoveProcess = false;
        //}

        //IEnumerator CorMoveBack(List<Vector3> path)
        //{
        //    isMoveProcess = true;
        //    int pathIndex = 0;
             
        //    List<Quaternion> reversedRotations = new List<Quaternion>(_originalRotations);
        //    reversedRotations.Reverse();

        //    while (pathIndex < path.Count)
        //    {
        //        Vector3 nextPoint = path[pathIndex];
                 
        //        Quaternion targetRotation = reversedRotations[pathIndex];
        //        while (Quaternion.Angle(transform.rotation, targetRotation) > Constants.EPSILON)
        //        {
        //            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        //            yield return null;
        //        } 
        //        while (Vector3.Distance(transform.position, nextPoint) > Constants.EPSILON)
        //        {
        //            transform.position = Vector3.MoveTowards(transform.position, nextPoint, -Speed * Time.deltaTime); // Отрицательная скорость для заднего хода
        //            yield return null;
        //        }

        //        pathIndex++;
        //    }

        //    isMoveProcess = false;
        //}
        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject != null && 1 << collision.gameObject.layer == _carLayer && isMoveProcess)
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

        public void Move()
        {
            
        }

        public void CancelMove()
        {
            
        }
    } 
    public interface ICommandMove
    {
        void Move();  
        void CancelMove();   
    }
}