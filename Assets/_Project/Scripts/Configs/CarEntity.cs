using System;
using UnityEngine;

namespace _Project.Scripts.StaticData
{
    public enum Direction
    {
        right,
        left,
        forward,
        back,
    }
    [Serializable]
    public class CarEntity
    { 
        [field: SerializeField] public int Id { get;  set; } 
        [field: SerializeField] public Direction Direction { get; private set; }
     
        public Quaternion GetStartRotation()
        {
            if (Direction == Direction.back) return Quaternion.Euler(Vector3.up * 180);
            if (Direction ==  Direction.forward) return Quaternion.Euler(Vector3.zero);
            if (Direction ==  Direction.right) return Quaternion.Euler(Vector3.up * 90);
            return Quaternion.Euler(Vector3.up * -90); 
        }
        public void RndTest()
        {
            var rndDirection = UnityEngine.Random.Range(0, 5);
            if (rndDirection == 1) Direction = Direction.right;
            else if (rndDirection == 2) Direction = Direction.left;
            else if (rndDirection == 3) Direction = Direction.forward;
            else Direction = Direction.back;

            var rndCarId = UnityEngine.Random.Range(0, 8);
           if(rndCarId != 7)
            {
                Id = rndCarId;
            } 
            else
            {
                Id = 0;
            }
        }
       
    } 
}
