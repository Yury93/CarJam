using _Project.Scripts.Helper;
using _Project.Scripts.Infrastructure.Services;
using _Project.Scripts.Infrastructure.Services.PersonPool;
using _Project.Scripts.StaticData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameLogic
{
    public class Person : MonoBehaviour, IPerson
    {
        [SerializeField] private Renderer _renderer; 
        [SerializeField] private float _space = 0.45f;
        private int _layerMask;
        private List<Transform> _path;
        private Coroutine _corMove;
        private RaycastHit[] _raycastHits;
        private int _currentPathIndex = 0;
         
        [field: SerializeField] public PersonAnimator PersonAnimator { get; private set; }
        [field: SerializeField] public float Speed { get; set; } = 4f;
        [field: SerializeField] public MaterialProperty MaterialProperty { get; private set; }
        [field: SerializeField] public int Number { get; set; } 
        [field: SerializeField] public ColorTag ColorTag => MaterialProperty.ColorTag;
        public Transform MyTransform => gameObject.transform; 
        public Color Color =>  MaterialProperty.Color; 
        public bool InCar { get  ; set  ; }
        public float Space => _space;

        public void Init(MaterialProperty materialProp)
        {
            RefreshColor(materialProp);

            _layerMask = 1 << LayerMask.NameToLayer(Constants.PERSON_LAYER);
        } 
        public void RefreshColor(MaterialProperty materialProp)
        {
            this.MaterialProperty = materialProp;
            _renderer.material.SetColor(PoolMaterials.MATERIAL_COLOR_KEY, materialProp.Color);
        } 
        public void SetPath(List<Transform> path)
        {
            _path = path;
            _currentPathIndex = 0;
        }
        public void MoveToPath()
        {
            if (InCar) return;

            StopCoroutineMove();
            _corMove = StartCoroutine(CorMove());
        }

        public void StopCoroutineMove()
        {
            if (_corMove != null)
            {
                StopCoroutine(_corMove);
            }
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
                        PersonAnimator.PlayRun();
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
 
