using _Project.Scripts.GameLogic;
using _Project.Scripts.Helper;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace _Project.Scripts.Infrastructure.Services.PersonPool
{
    public class PersonPool : IPersonPool
    {
        public List<MaterialProperty> MaterialProperties { get; private set; }
        public int TotalPersons => MaterialProperties.Count;

        public List<MaterialProperty> CreatePool(List<ICarData> cars)
        {
            MaterialProperties = new List<MaterialProperty>();
            foreach (ICarData car in cars)
            {
                for (int placePoint = 0; placePoint < car.GetCountFreePlacment(); placePoint++)
                {
                    MaterialProperties.Add(new MaterialProperty(car.MaterialProperty.Color,car.MaterialProperty.ColorTag));
                }
            }
            MiniUIInfo.instance.ShowQueue(TotalPersons);
            return MaterialProperties;
        }

        public void CreatePool(List<ICarData> carsData, List<MaterialProperty> removePersonMaterials)
        {
            CreatePool(carsData);
            foreach (var personMat in removePersonMaterials)
            {
                if (MaterialProperties.Exists(p=>p.ColorTag == personMat.ColorTag))
                {
                   var removeTarget = MaterialProperties.FirstOrDefault(p => p.ColorTag == personMat.ColorTag);
                    MaterialProperties.Remove(removeTarget);
                }
            }
            MiniUIInfo.instance.ShowQueue(TotalPersons);
        }

        public List<MaterialProperty> GetPersonsGroup()
        {
            if (MaterialProperties.Count == 0)
                return new List<MaterialProperty>();
             
            var groupItem = MaterialProperties
                .GroupBy(person => person.Color)
                .OrderByDescending(group => group.Count())
                .FirstOrDefault();
             
            List<MaterialProperty> persons = MaterialProperties
                .Where(person => person.Color == groupItem.Key)
                .ToList();

            var groups = MaterialProperties
                .Where(person => person.Color != groupItem.Key)
                .GroupBy(person => person.Color);



            var otherColors = groups 
                .OrderByDescending(group => group.Count()) 
                .Take(UnityEngine.Random.Range(0, groups.Count()))  
                .SelectMany(group => group.Take(UnityEngine.Random.Range(0,group.Count())) 
                .ToList());
             
            persons.AddRange(otherColors);

            persons.Shuffle();
 
            return persons;
        }

        public void RemovePersonEntity(MaterialProperty personEntity)
        { 
                MaterialProperties.Remove(personEntity);
            MiniUIInfo.instance.ShowQueue(TotalPersons);
        }
    }
}