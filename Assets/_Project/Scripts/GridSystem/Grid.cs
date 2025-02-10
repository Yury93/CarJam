using _Project.Scripts.StaticData; 
using System; 
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.GridSystem
{
    [System.Serializable]
    public class Grid
    {  
        [SerializeField] private GridPoint _cellPrefab;
        private LevelStaticData _levelData;
        public List<GridPoint> GridItems { get; private set; } = new List<GridPoint>();

        public List<GridPoint> CreateGrid(Transform parent, LevelStaticData levelData)
        {
            this._levelData = levelData;
            GridItems = new List<GridPoint>();
            int idItem = 0;

            int gridSize = levelData.Grid.GridSize;
            for (int line = 0; line < gridSize; line++)
            {
                for (int column = 0; column < gridSize; column++)
                {
                    if (IsInsideRhomb(line, column, gridSize))
                    {
                        if (_cellPrefab != null && parent != null)
                        {
                            GridPoint cellInstance = GameObject.Instantiate(_cellPrefab, parent);
                            cellInstance.transform.localScale = new Vector3(levelData.Grid.CellSize, levelData.Grid.CellSize, levelData.Grid.CellSize);

                            Vector3 cellSize = GetSizeCell(cellInstance.gameObject);
                            SetupLocalPositionForRhomb(cellInstance.gameObject, cellSize, line, column, gridSize);
                            GridItems.Add(cellInstance);
                            cellInstance.Init(cellInstance.transform.localPosition, column, line, idItem);
                            idItem++; 
                        }
                    }
                }
            }
             
            return GridItems;
        } 
        private bool IsInsideRhomb(int line, int column, int gridSize)
        { 
            int halfSize = gridSize / 2;
            return Math.Abs(line - halfSize) + Math.Abs(column - halfSize) <= halfSize;
        }

        private void SetupLocalPositionForRhomb(GameObject cellInstance, Vector3 cellSize, int line, int column, int gridSize)
        {
            float centrCellWidth = cellSize.x * 0.5f;
            float centrCellHeight = cellSize.y * 0.5f;
             
            int halfSize = gridSize / 2;
            float x = (column - halfSize) * (cellSize.x + _levelData.Grid.Space);
            float z = (line - halfSize) * (cellSize.y + _levelData.Grid.Space);

            cellInstance.transform.localPosition = new Vector3(x, 0,z);
        }
        public bool CanPlace(int gridItemId,Direction direction ,int size)
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
                TranslatePoint(direction, startLine, startColumn, step, out checkLine, out checkColumn);

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
        public void MarkCellsForCar(int gridItemId, CarEntity carEntity, int size)
        {
            GridPoint startItem = GridItems.FirstOrDefault(i => i.Id == gridItemId);
             
            int startLine = startItem.Line;
            int startColumn = startItem.Column;

            for (int step = 0; step < size; step++)
            {
                int checkLine, checkColumn;
                TranslatePoint(carEntity.Direction , startLine, startColumn, step, out checkLine, out checkColumn);
                 
                GridPoint cellToMark = GridItems.FirstOrDefault(i => i.Line == checkLine && i.Column == checkColumn);
                if (cellToMark != null)
                {
                    cellToMark.SetFree(false);
                    cellToMark.CarId = carEntity.Id;
                }
            }
        }

        private void TranslatePoint(Direction direction, int startLine, int startColumn, int step, out int checkLine, out int checkColumn)
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
        private Vector3 GetSizeCell(GameObject cellInstance)
        {
            MeshRenderer collider = cellInstance.GetComponent<MeshRenderer>();
            Vector3 cellSize = Vector3.zero;
            cellSize = collider.bounds.size;

            return cellSize;
        }
        private void SetupLocalPosition(GameObject cellInstance, Vector3 cellSize, int line, int column)
        {
            float centrCellWidth = cellSize.x * 0.5f;
            float centrCellHeight = cellSize.y * 0.5f;
            cellInstance.transform.localPosition = new Vector3(
                column * (cellSize.x + _levelData.Grid.Space) - (_levelData.Grid.GridSize * (cellSize.x + _levelData.Grid.Space)) * 0.5f + centrCellWidth, 0,
                line * (cellSize.y + _levelData.Grid.Space) - (_levelData.Grid.GridSize * (cellSize.y + _levelData.Grid.Space)) * 0.5f + centrCellHeight
                 
            );
        }  
    } 
}
