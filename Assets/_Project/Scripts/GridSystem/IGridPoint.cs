using UnityEngine;

namespace _Project.Scripts.GridSystem
{
    public interface IGridPoint
    {
        public int Id { get; set; }
        public int Column { get; set; }
        public int Line { get; set; }
        public Vector3 Position { get; set; }
        public bool IsFree { get; set; }
        public void SetFree(bool free);
    }
}