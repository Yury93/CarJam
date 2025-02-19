using _Project.Scripts.Helper;
using _Project.Scripts.Infrastructure.Services.PersonPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameLogic
{
    public class Person : MonoBehaviour, IPerson
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private float _radius;
        [SerializeField] private float _space = 0.45f;
        private int _layerMask;
        private List<Transform> _path;
        private Coroutine _corMove;
        private RaycastHit[] _raycastHits;
        private int _currentPathIndex = 0;
        private readonly string _materialColor = "_BaseColor";
        [field: SerializeField] public PersonAnimator PersonAnimator { get; private set; }
        [field: SerializeField] public float Speed { get; private set; } = 4f;
        [field: SerializeField] public PersonEntity PersonEntity { get; private set; }
        public Transform MyTransform => gameObject.transform;

        public Color Color =>  PersonEntity.Color;

        public bool InCar { get  ; set  ; }

        public void Init(PersonEntity personEntity)
        {
            this.PersonEntity = personEntity;
            _renderer.material.SetColor(_materialColor, personEntity.Color);

            _layerMask = 1 << LayerMask.NameToLayer(Constants.PERSON_LAYER); 
        }
        public void SetPath(List<Transform> path)
        {
            _path = path;
            _currentPathIndex = 0;
        }
        public void MoveToPath()
        {
            if (InCar) return;

            if ( _corMove != null)
            {
                StopCoroutine(_corMove);
            }
            _corMove = StartCoroutine(CorMove());
        }
        public int CheckForward()
        {
            _raycastHits = new RaycastHit[1];
            int countHits = LaunchRaycast(transform.TransformDirection(Vector3.forward), _space);
            if (countHits == 0)
                countHits = LaunchRaycast(transform.TransformDirection(Vector3.right), _space / 3); ;
            return countHits;
        }
        public int LaunchRaycast(Vector3 direction, float distance)
        {
            return Physics.RaycastNonAlloc(transform.position + Vector3.up,
                direction,
                _raycastHits, distance, _layerMask);
        }

        private IEnumerator CorMove()
        {
            PersonAnimator.PlayRun();
            while (_currentPathIndex < _path.Count)
            {
                Vector3 nextPoint = Vector3.zero;
                nextPoint = _path[_currentPathIndex].position;

                while (Vector3.Distance(MyTransform.position, nextPoint) > Constants.EPSILON)
                {
                    if (CheckForward() == 0)
                    {
                        MyTransform.forward = nextPoint - transform.position;
                        MyTransform.position = Vector3.MoveTowards(MyTransform.position, nextPoint, Speed * Time.deltaTime); 
                    }
                    else
                    {
                        PersonAnimator.PlayIdle();
                    }
                    yield return null;
                }
                _currentPathIndex++;
            }
            PersonAnimator.PlayIdle();
        }
       
    }
}
 
