using _Project.Scripts.Configs;
using _Project.Scripts.GridSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.GridSystem
{
    [System.Serializable]
    public class Grid
    {  
        [SerializeField] private GridItem cellPrefab;
        private LevelData levelData;
        public List<GridItem> GridItems { get; private set; } = new List<GridItem>();
        public List<GridItem>CreateGrid(Transform parent, LevelData levelData)
        {
            this.levelData = levelData;
            GridItems = new List<GridItem>();
            int idItem = 0;
            for (int line = 0; line < levelData. Lines; line++)
            {
                for (int column = 0; column < levelData.Columns; column++)
                { 
                    if (cellPrefab != null && parent != null)
                    {
                        GridItem cellInstance = GameObject.Instantiate(cellPrefab, parent);
                        cellInstance.transform.localScale = new Vector3(levelData.CellSize, levelData.CellSize, levelData.CellSize);
                  
                        Vector3 cellSize = GetSizeCell(cellInstance.gameObject); 
                        SetupLocalPosition(cellInstance.gameObject, cellSize,line, column);

                        GridItems.Add(cellInstance);
                        cellInstance.Init(cellInstance.transform.localPosition,column,line,idItem);
                        idItem++;
                    }
                }
            }
            return GridItems;
        } 
        public bool CanPlace(int gridItemId,Direction direction ,int size)
        {
            GridItem startItem = GridItems.FirstOrDefault(i => i.Id == gridItemId);
            if (startItem == null || !startItem.IsFree)
            {
               
                return false;
            }
             
            int startLine = startItem.Line;
            int startColumn = startItem.Column;
             
            for (int step = 0; step < size; step++)
            {
                int checkLine, checkColumn;
                CanPlaceItemInDirection(direction, startLine, startColumn, step, out checkLine, out checkColumn);

                if (checkLine < 0 || checkLine >= levelData.Lines || checkColumn < 0 || checkColumn >= levelData.Columns)
                {
                    Debug.LogError("Выход за границы сетки");
                    return false;
                }

                GridItem cellToCheck = GridItems.FirstOrDefault(i => i.Line == checkLine && i.Column == checkColumn);
                if (cellToCheck == null || !cellToCheck.IsFree)
                {
                    Debug.LogError("Нет места под эту машинку");
                    return false;
                }
            }

            return true;
        } 
        public void MarkCellsAsOccupied(int gridItemId, CarEntity carEntity, int size)
        {
            GridItem startItem = GridItems.FirstOrDefault(i => i.Id == gridItemId);
             
            int startLine = startItem.Line;
            int startColumn = startItem.Column;

            for (int step = 0; step < size; step++)
            {
                int checkLine, checkColumn;
                CanPlaceItemInDirection(carEntity.Direction , startLine, startColumn, step, out checkLine, out checkColumn);
                 
                GridItem cellToMark = GridItems.FirstOrDefault(i => i.Line == checkLine && i.Column == checkColumn);
                if (cellToMark != null)
                {
                    cellToMark.SetFree(false);
                    cellToMark.CarId = carEntity.Id;
                }
            }
        }

        private void CanPlaceItemInDirection(Direction direction, int startLine, int startColumn, int step, out int checkLine, out int checkColumn)
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
                column * (cellSize.x + levelData.Space) - (levelData.Columns * (cellSize.x + levelData.Space)) * 0.5f + centrCellWidth, 0,
                line * (cellSize.y + levelData.Space) - (levelData.Lines * (cellSize.y + levelData.Space)) * 0.5f + centrCellHeight
                 
            );
        }  
    } 
}
