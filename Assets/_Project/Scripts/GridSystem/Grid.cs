using _Project.Scripts.Infrastructure.Services;
using _Project.Scripts.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.GridSystem
{
    [System.Serializable]
    public class Grid<TShape> : IGrid where TShape : IShape
    {
        public const string   GRID_POINT = "GridPoint";
        private IAssetProvider _assetProvider;
        private LevelStaticData _levelData;
        public List<GridPoint> GridItems { get; set; } = new List<GridPoint>();
        public Grid(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        public async Task<List<GridPoint>> CreateGrid(Transform parent, LevelStaticData levelData)
        {
            this._levelData = levelData;
            GridItems = new List<GridPoint>();
            int idItem = 0;

            int gridSize = levelData.Grid.GridSize;
            for (int line = 0; line < gridSize; line++)
            {
                for (int column = 0; column < gridSize; column++)
                {
                    if (IsPointInsideShape(line, column, gridSize))
                    {
                        if (_assetProvider != null && parent != null)
                        {
                            var task =  _assetProvider.instatiateAsync(GRID_POINT, parent);
                            await task;
                            GridPoint cellInstance = task.Result.GetComponent<GridPoint>();
                            cellInstance.transform.localScale = new Vector3(levelData.Grid.CellSize, levelData.Grid.CellSize, levelData.Grid.CellSize);

                            Vector3 cellSize = GetSizeCell(cellInstance.gameObject);
                            SetupPosition(cellInstance.gameObject, cellSize, line, column, gridSize);
                            GridItems.Add(cellInstance);
                            cellInstance.Init(cellInstance.transform.localPosition, column, line, idItem);
                            idItem++; 
                        }
                    }
                }
            }
             
            return GridItems;
        }
        public bool CanPlace(int gridItemId, Direction direction, int size)
        {
            GridPoint startItem = GridItems.FirstOrDefault(i => i.Id == gridItemId);
            if (startItem == null || !startItem.IsFree)
            {

                return false;
            }

            int startLine = startItem.Line;
            int startColumn = startItem.Column;

            for (int step = 0; step < size; step++)
            {
                int checkLine, checkColumn;
                TranslateToPoint(direction, startLine, startColumn, step, out checkLine, out checkColumn);

                if (checkLine < 0 || checkLine >= _levelData.Grid.GridSize || checkColumn < 0 || checkColumn >= _levelData.Grid.GridSize)
                {
                    // Debug.LogError("Выход за границы сетки");
                    return false;
                }

                GridPoint cellToCheck = GridItems.FirstOrDefault(i => i.Line == checkLine && i.Column == checkColumn);
                if (cellToCheck == null || !cellToCheck.IsFree)
                {
                    // Debug.LogError("Нет места под эту машинку");
                    return false;
                }
            }

            return true;
        }
        public void MarkCells(int gridItemId, IGridDirectionItem directionEntity, int size)
        {
            GridPoint startItem = GridItems.FirstOrDefault(i => i.Id == gridItemId);

            int startLine = startItem.Line;
            int startColumn = startItem.Column;

            for (int step = 0; step < size; step++)
            {
                int checkLine, checkColumn;
                TranslateToPoint(directionEntity.Direction, startLine, startColumn, step, out checkLine, out checkColumn);

                GridPoint cellToMark = GridItems.FirstOrDefault(i => i.Line == checkLine && i.Column == checkColumn);
                if (cellToMark != null)
                {
                    cellToMark.SetFree(false);
                    cellToMark.CarId = directionEntity.Id;
                }
            }
        }
        public void TranslateToPoint(Direction direction, int startLine, int startColumn, int step, out int checkLine, out int checkColumn)
        {
            checkLine = startLine;
            checkColumn = startColumn;
            switch (direction)
            {
                case Direction.forward:
                    checkLine -= step;
                    break;
                case Direction.back:
                    checkLine += step;
                    break;
                case Direction.left:
                    checkColumn -= step;
                    break;
                case Direction.right:
                    checkColumn += step;
                    break;
            }
        }
        public Vector3 GetSizeCell(GameObject cellInstance)
        {
            MeshRenderer collider = cellInstance.GetComponent<MeshRenderer>();
            Vector3 cellSize = Vector3.zero;
            cellSize = collider.bounds.size;

            return cellSize;
        }
         

        private bool IsPointInsideShape(int line, int column, int gridSize)
        { 
            int halfSize = gridSize / 2;
            return Math.Abs(line - halfSize) + Math.Abs(column - halfSize) <= halfSize;
        } 
        private void SetupPosition(GameObject cellInstance, Vector3 cellSize, int line, int column, int gridSize)
        {
            float centrCellWidth = cellSize.x * 0.5f;
            float centrCellHeight = cellSize.y * 0.5f;
             
            int halfSize = gridSize / 2;
            float x = (column - halfSize) * (cellSize.x + _levelData.Grid.Space);
            float z = (line - halfSize) * (cellSize.y + _levelData.Grid.Space);

            cellInstance.transform.localPosition = new Vector3(x, 0,z);
        }
    } 
    public interface IShape
    {
         bool IsPointInsideShape(int line, int column, int gridSize);
         void SetupPosition(GameObject cellInstance, Vector3 cellSize, int line, int column, int gridSize);
    }
    public class Romb : IShape
    {
        public bool IsPointInsideShape(int line, int column, int gridSize)
        {
            return true;
        }

        public void SetupPosition(GameObject cellInstance, Vector3 cellSize, int line, int column, int gridSize)
        {
            return;
        }
    }
}
