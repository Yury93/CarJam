using _Project.Scripts.Infrastructure.Services;
using _Project.Scripts.Infrastructure.Services.PersonPool;
using _Project.Scripts.StaticData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace _Project.Scripts.GameLogic
{
    public class Car : MonoBehaviour, ICarData
    {
        [SerializeField] private Transform _forwardPoint, _rightPoint;
        private int markPlaceCount;
        [field: SerializeField] public int Id { get; set; }
        [field: SerializeField] public int Size { get; private set; } = 1; 
        [field: SerializeField] public List<Transform> Placements { get; set; }
        [field: SerializeField] public int CountPlace => Placements.Count;
        [field: SerializeField] public int Number { get;  set; } = 0; 
        [field:SerializeField] public Renderer RendererCar { get; private set; }
        [field: SerializeField] public MaterialProperty MaterialProperty { get; private set; }
        public CarMover CarMover { get; private set; }
        public Vector3 GetDirection
        {
            get
            {
                if (DirectionEntity.Direction == Direction.forward) return _forwardPoint.position;
                if (DirectionEntity.Direction == Direction.back) return -_forwardPoint.position;
                if (DirectionEntity.Direction == Direction.right) return _rightPoint.position;
                else return -_rightPoint.position;
            }
        } 
        public IGridDirectionItem DirectionEntity { get; private set; }
        public bool IsFull => Placements.Count <= markPlaceCount;
         public int GetCountFreePlacment()
        {
           var count = Placements.Count - markPlaceCount;
            Debug.Log(" в машинке свободных мест " + count);
            return count;
        }
        public virtual void Init(IGridDirectionItem dirEntity)
        {
            this.DirectionEntity = dirEntity;
            Number = dirEntity.Number;
            CarMover = GetComponent<CarMover>();
        }
        public void SetupPerson(IPerson person)
        { 
            person.Speed = 0;
            person.MyTransform.SetParent(Placements[markPlaceCount]);
            person.InCar = true;
            person.MyTransform.localPosition = Vector3.zero;
            person.MyTransform.localRotation = Quaternion.identity;
            person.StopCoroutineMove();
            person.MyTransform.GetComponent<PersonAnimator>().PlaySit();
            markPlaceCount++; 
        } 
        public void SwitchColorType(MyMaterial myMaterial)
        {
            MaterialProperty = myMaterial.MaterialProperty; 
            RendererCar.material = myMaterial.Material;
        }

        public void OnSitPerson(IPerson person)
        {
             
        }
    }
}