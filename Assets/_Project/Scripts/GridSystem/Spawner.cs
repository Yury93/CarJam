using _Project.Scripts.Configs;
using _Project.Scripts.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Project.Scripts.GridSystem
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        public List<GridItem> gridItems = new List<GridItem>();
        [Button("Create Grid")] 
        public void CreateGrid()
        {
            StartCoroutine(cORCreate());
        }

        private IEnumerator cORCreate()
        {
            gridItems.RemoveAll(n => n == null);
            foreach (var car in gridItems)
            {
                if(car != null)
                Destroy(car.gameObject);
            }
            gridItems.RemoveAll(n => n == null);
            gridItems.Clear();
            int column = 0;
            int lines = 0;

            int carIndex = 0;
            for (int i = 0; i < levelData.CarCount; i++)
            {
                yield return new WaitForSeconds(0.1f);

                if (levelData.Cars.Count - 1 < carIndex)
                {
                    carIndex = 0;
                }

                Quaternion carRotation = levelData.Cars[carIndex].GetStartRotation();
                GameObject carInstance = Instantiate(levelData.Cars[carIndex].Car, Vector3.zero, carRotation, transform); // Позиция будет установлена позже


                Vector3 carSize = GetCarSize(carInstance);
                float maxSize = Mathf.Max(carSize.x, carSize.z);
                 
                Vector3 newPosition = transform.position;
                if (i != 0)
                {
                    try
                    { 
                        if (lines >= levelData.Lines - 1)
                        {
                            column++;
                            lines = 0;
                        }
                        else
                        {
                            lines++;
                        }

                        if (column >= levelData.Column)
                        {
                            column = 0;
                        }

                        // Рассчитываем новую позицию
                        if (lines > 0)
                        {
                            newPosition = gridItems.Last().transform.position + new Vector3(0, 0, maxSize) * levelData.Space;
                            Debug.Log("lines > 0 // ПОЗИЦИЯ COL   :  " + column + "/ LINE: "+ lines);
                        }
                        else if (column > 0)
                        {
                            if (lines == 0)
                            {
                                Debug.Log("lines = 0 // ПОЗИЦИЯ COL   :  " + column + "/ LINE: " + lines);
                                newPosition = gridItems.First(g=>g.Line == 0 && g.Column == column - 1).transform.position + new Vector3(maxSize, 0, 0) * levelData.Space;
                            } 
                        }

                        carInstance.transform.position = newPosition; 
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error: {ex.Message}, col {column}, lin {lines}, gridItems count {gridItems.Count}");
                    }
                }
                var gridItem = carInstance.GetComponent<GridItem>();
                gridItem.SetPlace(carSize, newPosition, column, lines);
                gridItems.Add(gridItem);
                carIndex++; 
            }
        }

        public Vector3 GetCarSize(GameObject carPrefab)
        {
            Collider collider = carPrefab.GetComponent<Collider>();
            if (collider != null)
            {
                return collider.bounds.size;
            }

            return Vector3.one;
        }
    }
}