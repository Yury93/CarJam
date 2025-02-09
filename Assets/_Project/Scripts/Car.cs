using _Project.Scripts.Configs;
using UnityEngine;
using static _Project.Scripts.Configs.CarEntity;

public class Car : MonoBehaviour
{
    [SerializeField] private Transform forwardPoint, rightPoint;
    [field: SerializeField] public int Id {  get; private set; } 
    [field: SerializeField] public int Size { get; private set; } = 1;
    public Vector3 CarDirection
    {
        get
        {
            if (CarEntity.Direction == Direction.forward) return forwardPoint.position;
            if (CarEntity.Direction == Direction.back) return  -forwardPoint.position;
            if (CarEntity.Direction == Direction.right) return rightPoint.position;
            else return -rightPoint.position;
        }
    }

    
    public CarEntity CarEntity { get; private set; }
    public void Init(CarEntity carEntity)
    {
        this.CarEntity = carEntity;   
    }
}
