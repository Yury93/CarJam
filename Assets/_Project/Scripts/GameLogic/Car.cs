using _Project.Scripts.StaticData;
using UnityEngine;
using static _Project.Scripts.StaticData.CarEntity;

public class Car : MonoBehaviour
{
    [SerializeField] private Transform _forwardPoint, _rightPoint;
    [field: SerializeField] public int Id {  get; private set; } 
    [field: SerializeField] public int Size { get; private set; } = 1;
    public Vector3 CarDirection
    {
        get
        {
            if (CarEntity.Direction == Direction.forward) return _forwardPoint.position;
            if (CarEntity.Direction == Direction.back) return  -_forwardPoint.position;
            if (CarEntity.Direction == Direction.right) return _rightPoint.position;
            else return -_rightPoint.position;
        }
    }

    
    public CarEntity CarEntity { get; private set; }
    public void Init(CarEntity carEntity)
    {
        this.CarEntity = carEntity;   
    }
}
