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

        public Quaternion GetStartRotation()
        {
            if (StartDirection == CarEntity.StartCarDirection.back) return Quaternion.Euler(Vector3.up * 180);
            if (StartDirection == CarEntity.StartCarDirection.forward) return Quaternion.Euler(Vector3.zero);
            if (StartDirection == CarEntity.StartCarDirection.right) return Quaternion.Euler(Vector3.up * 90);
            return Quaternion.Euler(Vector3.up * -90);
        }
       
    } 
}
