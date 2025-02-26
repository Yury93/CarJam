using _Project.Scripts.GameLogic;
using System.Collections.Generic;

namespace _Project.Scripts.Infrastructure.Services.PersonPool
{
    public interface IPersonPool : IService
    {
        public int TotalPersons { get; }
        public List<MaterialProperty> MaterialProperties { get; }
        List<MaterialProperty> CreatePool(List<ICarData> carDatas);
 
        void CreatePool(List<ICarData> carsData, List<MaterialProperty> personMaterials);
        List<MaterialProperty> GetPersonsGroup();
        void RemovePersonEntity( MaterialProperty  personColor);
    }
}