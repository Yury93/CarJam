using _Project.Scripts.Helper;
using _Project.Scripts.Infrastructure.Services.PersonPool;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "Level", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        [field:SerializeField] public CarsStaticData Cars {  get; private set; }
        [field:SerializeField] public MaterialsData MaterialsData { get; private set; }
        [field: SerializeField] public GridStaticData Grid { get; private set; }
        [field: SerializeField] public int Id { get; set; } = 0;
        [field: SerializeField] public string LevelName { get; private set; } = "Level";
        [field:SerializeField] public Direction SetupDirection { get; private set; }

        [Button("RndCars")]
        public void RandomCars()
        {
            int num = 0;
            foreach (var item in Cars.GridItems)
            {
                item.SetRandomDirection();
                item.Number = num;
                num++;
            }
        }
        [Button("SetupDirection")]
        public void SetupSelectedDirection()
        {
            int num = 0;
            foreach (var item in Cars.GridItems)
            {
                item.Direction = SetupDirection;
                item.Number = num;
                num++;
            }
        }
    }
    [Serializable]
    public class MaterialsData
    {
        [field: SerializeField] public MaterialProperty red { get; private set; }
        [field: SerializeField] public MaterialProperty yellow { get; private set; }
        [field: SerializeField] public MaterialProperty black { get; private set; }
        [field: SerializeField] public MaterialProperty blue { get; private set; }
        [field: SerializeField] public MaterialProperty lightBlue { get; private set; }
        [field: SerializeField] public MaterialProperty green { get; private set; }
        public List<MaterialProperty> GetColors()
        {
            return new List<MaterialProperty>(){
            red, yellow, black, blue,lightBlue, green
            };
        } 
        public MaterialProperty GetColor(ColorTag colorTag)
        {
            if (colorTag == ColorTag.red)
            {
                return red;
            }
            if (colorTag == ColorTag.yellow)
            {
                return yellow;
            }
            if (colorTag == ColorTag.black)
            {
                return black;
            }
            if (colorTag == ColorTag.blue)
            {
                return blue;
            }
            if (colorTag == ColorTag.lightBlue)
            {
                return lightBlue;
            }
            if (colorTag == ColorTag.green)
            {
                return green;
            }
            Debug.LogError("user error 200 :)");
            return green;
        }
    }
     
}
