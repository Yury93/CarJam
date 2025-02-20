using _Project.Scripts.StaticData;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameLogic
{
    public interface ICarData : IGridItem
{
    public int Size { get; }
    public Color Color { get; }
     public ColorTag ColorTag { get; }
    public List<Transform> Placements { get; set; }
    public int CountPlace => Placements.Count;
    public int Number { get;   set; } 
    }
}