using _Project.Scripts.StaticData;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.GridSystem
{
    public interface IGrid
    {
        List<GridPoint> GridItems { get; set; }
        Task<List<GridPoint>> CreateGrid(Transform parent, LevelStaticData levelData);
        bool CanPlace(int gridItemId, Direction direction, int size);
        void MarkCells(int gridItemId, IGridDirectionItem carEntity, int size);
        void TranslateToPoint(Direction direction, int startLine, int startColumn, int step, out int checkLine, out int checkColumn);
        Vector3 GetSizeCell(GameObject cellInstance);
    } 
}
