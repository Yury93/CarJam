using _Project.Scripts.StaticData;
using UnityEngine;
using static _Project.Scripts.StaticData.CarEntity;

namespace _Project.Scripts.GameLogic
{
    public class Car : MonoBehaviour, IGridItem
    {
        [SerializeField] private Transform _forwardPoint, _rightPoint;
        [field: SerializeField] public int Id { get; set; }
        [field: SerializeField] public int Size { get; private set; } = 1;
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

        public IGridDirectionEntity DirectionEntity { get; private set; }
        public virtual void Init(IGridDirectionEntity dirEntity)
        {
            this.DirectionEntity = dirEntity;
        }

    }
} 
public interface IGridItem
{
    int Id { get; set; }
    Vector3 GetDirection { get; }
    void Init(IGridDirectionEntity dirEntity);
    
}
