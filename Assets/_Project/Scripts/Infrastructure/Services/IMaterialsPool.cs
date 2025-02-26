
using _Project.Scripts.Infrastructure.Services.PersonPool;
using _Project.Scripts.StaticData;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Services
{
    public interface IMaterialsPool : IService
    {
         List<MyMaterial> MyMaterials { get;   } 
        public void AddMaterial(MyMaterial material);
        public MyMaterial GetMaterial(ColorTag colorTag);
    }
}