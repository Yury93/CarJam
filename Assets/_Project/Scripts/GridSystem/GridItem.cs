using UnityEngine;

namespace _Project.Scripts.GridSystem
{
    public class GridItem : MonoBehaviour
    {
        [field: SerializeField] public int Column { get; private set; }
        [field: SerializeField] public int Line { get; private set; }
        [field: SerializeField] public Vector3 Size { get; private set; }
        [field: SerializeField] public Vector3 Position { get; private set; }
        public void SetPlace(Vector3 size, Vector3 position ,int column, int line)
        {
            Column = column;
            Line = line;
            Position = position;
        }
    }
}