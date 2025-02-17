using System;
using UnityEngine;

namespace _Project.Scripts.StaticData
{
    [Serializable] 
    public class GridStaticData
    {
        [field: Range(0, 3), SerializeField] public float Space { get; private set; } = 0.35f;
        [field: SerializeField] public float GridRotate { get; private set; } = 45f;
        [field: SerializeField] public int GridSize { get; private set; } = 12;
        [field: SerializeField] public float CellSize { get; private set; } = 1;
        [field: SerializeField] public Vector3 OffsetPosition { get; private set; } = new Vector3(0,0,0);
    }
}
