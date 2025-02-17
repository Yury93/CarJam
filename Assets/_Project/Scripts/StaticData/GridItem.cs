using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.StaticData
{
   
    [Serializable]
    public class GridItem : IGridDirectionItem
    { 
        [field: SerializeField] public int Id { get;  set; } 
        [field: SerializeField] public Direction Direction { get;  set; }
 
 
        public Quaternion GetTransformDirection()
        {
            if (Direction == Direction.back) return Quaternion.Euler(Vector3.up * 180);
            if (Direction ==  Direction.forward) return Quaternion.Euler(Vector3.zero);
            if (Direction ==  Direction.right) return Quaternion.Euler(Vector3.up * 90);
            return Quaternion.Euler(Vector3.up * -90); 
        }
        public void SetRandomDirection()
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
