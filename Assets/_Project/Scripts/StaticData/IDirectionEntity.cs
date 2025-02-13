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
    public interface IGridDirectionEntity : IDirectionEntity
    {
        public int Id { get; set; }
    }
    public interface IDirectionEntity
    {
        public Direction Direction { get;  set; }
        Quaternion GetTransformDirection();
    }
}
