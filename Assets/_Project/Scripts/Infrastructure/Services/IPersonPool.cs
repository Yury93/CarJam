using _Project.Scripts.GameLogic;
using System.Collections.Generic;

namespace _Project.Scripts.Infrastructure.Services.PersonPool
{
    public interface IPersonPool : IService
    {
        public int TotalPersons { get; }
        public List<PersonEntity> PersonEntities { get; }
        List<PersonEntity> CreatePool(List<ICarData> carDatas);
        List<PersonEntity> GetPersonsGroup();
        void RemovePersonEntity( PersonEntity  personColor);
    }
}