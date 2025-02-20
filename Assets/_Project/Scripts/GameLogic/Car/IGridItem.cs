using _Project.Scripts.StaticData;
using UnityEngine;

namespace _Project.Scripts.GameLogic
{
    public interface IGridItem
{
    int Id { get; set; }
    Vector3 GetDirection { get; }
    void Init(IGridDirectionItem dirEntity);
    
}
}