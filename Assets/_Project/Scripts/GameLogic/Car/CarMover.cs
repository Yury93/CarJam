using _Project.Scripts.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace _Project.Scripts.GameLogic
{
    public class CarMover : MonoBehaviour,ICommandMove, IRaycastChecker
    {
        [SerializeField] private float _speed = 10;
        [SerializeField] private float _rotationSpeed = 100;
        [SerializeField] private float _forwardRaycastDistance = 1f;
        private int _carLayer; 
        private List<Vector3> _originalPath,_currentPath;
        private Vector3 _startPosition;
        private List<Quaternion> _originalRotations; 
        private RaycastHit[] _raycastHits;
        private Coroutine _corMove;
        private CarStand _carStand;
        [SerializeField] private PathBuilder _carPath;
        public bool IsMoveProcess { get; private set; }
        [field: SerializeField] public bool IsStand { get; set; }

        private void Start()
        { 
            _carLayer = 1 << LayerMask.NameToLayer(Constants.CAR_LAYER); 

            _startPosition = transform.position;
            _originalPath = new List<Vector3>();
        }
        public void SetStandTarget(CarStand freeStand)
        {
            _carStand = freeStand;
            _carStand.Free = false;
            //_carStand.WaitCar();
        }
        public void SetPathBuilder(PathBuilder carPath)
        {
            _carPath = carPath;
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
            if (IsMoveProcess || _carStand &&
                Vector3.Distance(_carStand.transform.position, transform.position) <= Constants.EPSILON)
                return; 

            CreateNewPath(_carPath.GetPath(this)); 
            if (_corMove != null) StopCoroutine(_corMove);
            _corMove = StartCoroutine(CorMove(_originalPath));
            StartCoroutine(CorCheckCarForward());
        }
        [Button("cancelMove")]
        public void CancelMove()
        {
            if (_originalPath != null && _originalPath.Count > 0)
            {
                _currentPath = new List<Vector3>(_originalPath);
                _currentPath.Reverse();
                if (_corMove != null) StopCoroutine(_corMove);
                _corMove = StartCoroutine(CorMove(_currentPath,isBack:true));
            }
        }
        public IEnumerator CorMove(List<Vector3> path,bool isBack = false)
        {
            int pathIndex = 0;
            IsMoveProcess = true;
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
                     
                    //float step = _speed * Time.deltaTime;
                    //transform.position = Vector3.Lerp(transform.position, nextPoint, step);

                    yield return null;
                }
                pathIndex++;
            }
            IsMoveProcess = false;
        }
        private IEnumerator CorCheckCarForward()
        {
     
            while (IsMoveProcess && _carStand )
            { 
                CarMover carMover;
                if (CheckForward() != 0 && _raycastHits[0].collider.TryGetComponent<CarMover>(out carMover))
                {
                    if (carMover && carMover.IsMoveProcess == false)
                    {
                        IsMoveProcess = false;
                        if (_carStand != null)
                        {
                            _carStand.Free = true;
                            _carStand = null;
                        }
                        if (_corMove != null) StopCoroutine(_corMove);
                        _corMove = StartCoroutine(CorMove(new List<Vector3> { _startPosition }, isBack: true));
           
                        break;
                    }
                }
                yield return null;
            }
        }

        public int LaunchRaycast(Vector3 direction, float distance)
        {
            _raycastHits = new RaycastHit[1];
            return Physics.RaycastNonAlloc(transform.position + Vector3.up/2,
                direction + Vector3.up / 2,
                _raycastHits, distance, _carLayer);
        }
        private Quaternion GetTargetRotation(bool isBack, Vector3 direction)
        {
            Quaternion targetRotation;
            if (isBack) targetRotation = Quaternion.LookRotation(-direction);
            else targetRotation = Quaternion.LookRotation(direction);
            return targetRotation;
        } 
        public int CheckForward()
        {
            Debug.DrawLine(transform.position + Vector3.up/2, 
                transform.position + 
                transform.TransformDirection(Vector3.forward + Vector3.up / 2), 
                Color.red, _forwardRaycastDistance);

            return LaunchRaycast(transform.TransformDirection(Vector3.forward), _forwardRaycastDistance);
        } 
       
    }  
}