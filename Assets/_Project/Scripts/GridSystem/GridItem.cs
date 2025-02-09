using System;
using UnityEngine;

namespace _Project.Scripts.GridSystem
{
    public class GridItem : MonoBehaviour
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public int Column { get; private set; }
        [field: SerializeField] public int Line { get; private set; } 
        [field: SerializeField] public Vector3 Position { get; private set; }
        [field: SerializeField] public bool IsFree { get; private set; } = true;
        public int CarId;

        public void Init(Vector3 position ,int column, int line, int number)
        {
            Column = column;
            Line = line;
            Position = position;
            this.Id = number;
        } 
        public void SetFree(bool free)
        {
            IsFree = free;
        }
    }
}