using _Project.Scripts.Infrastructure.Services.PersonPool;
using _Project.Scripts.StaticData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
namespace _Project.Scripts.Infrastructure.Services
{
    public class PoolMaterials : IMaterialsPool
    {
        private List<MyMaterial> _poolMaterials = new List<MyMaterial>();
        public const string MATERIAL_COLOR_KEY = "_BaseColor";
        public List<MyMaterial> MyMaterials
        {
            get => _poolMaterials;
        }

        public void AddMaterial(MyMaterial material)
        { 
            _poolMaterials.Add(material);
        }

        public MyMaterial GetMaterial(ColorTag colorTag)
        {
         
          var myMat = _poolMaterials.First(m=>m.MaterialProperty.ColorTag == colorTag); 
            return myMat;
        }
    }
    public struct MyMaterial
    {
        public MyMaterial (Material material , MaterialProperty materialProperty)
        {
            Material = material;
            MaterialProperty = materialProperty;
        }
        public Material Material;
        public MaterialProperty MaterialProperty;
    }
}