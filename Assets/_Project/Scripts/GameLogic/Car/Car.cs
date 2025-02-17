using _Project.Scripts.StaticData;
using System.Collections.Generic;
using UnityEngine;
using static _Project.Scripts.StaticData.GridItem;

namespace _Project.Scripts.GameLogic
{
    public class Car : MonoBehaviour, ICarData
    {
        [SerializeField] private Transform _forwardPoint, _rightPoint;
        [field: SerializeField] public int Id { get; set; }
        [field: SerializeField] public int Size { get; private set; } = 1;
        [field:SerializeField] public Color Color { get; private set; } = Color.white;
        [field: SerializeField] public List<Transform> Placements { get; set; }
        [field: SerializeField] public int CountPlace => Placements.Count;
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
        public virtual void Init(IGridDirectionItem dirEntity)
        {
            this.DirectionEntity = dirEntity;
        }

    }
}
public interface ICarData : IGridItem
{
    public int Size { get; }
    public Color Color { get; }
    public List<Transform> Placements { get; set; }
    public int CountPlace => Placements.Count;
}
public interface IGridItem
{
    int Id { get; set; }
    Vector3 GetDirection { get; }
    void Init(IGridDirectionItem dirEntity);
    
}
