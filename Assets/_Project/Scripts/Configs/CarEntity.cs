using System;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class CarEntity
    {
        public enum StartCarDirection
        {
            right,
            left,
            forward,
            back,
        }
        [field: SerializeField] public int Id { get;  set; }
        [field: SerializeField] public GameObject Car { get; private set; }
        [field: SerializeField] public StartCarDirection StartDirection { get; private set; }

        public Vector3 GetStartDirection()
        {
            if (StartDirection == CarEntity.StartCarDirection.back) return Vector3.back;
            if (StartDirection == CarEntity.StartCarDirection.forward) return Vector3.forward;
            if (StartDirection == CarEntity.StartCarDirection.right) return Vector3.right;
            return Vector3.left;
        }
       
    } 
}
