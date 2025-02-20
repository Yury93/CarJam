using _Project.Scripts.GameLogic;
using _Project.Scripts.Helper;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace _Project.Scripts.Infrastructure.Services.PersonPool
{
    public class PersonPool : IPersonPool
    {
        public List<PersonEntity> PersonEntities { get; private set; }
        public int TotalPersons => PersonEntities.Count;

        public List<PersonEntity> CreatePool(List<ICarData> cars)
        {
            PersonEntities = new List<PersonEntity>();
            foreach (ICarData car in cars)
            {
                for (int placePoint = 0; placePoint < car.Placements.Count; placePoint++)
                {
                    PersonEntities.Add(new PersonEntity(car.Color,car.ColorTag));
                }
            }
            MiniUI.instance.ShowQueue(TotalPersons);
            return PersonEntities;
        }
        public List<PersonEntity> GetPersonsGroup()
        {
            if (PersonEntities.Count == 0)
                return new List<PersonEntity>();
             
            var groupItem = PersonEntities
                .GroupBy(person => person.Color)
                .OrderByDescending(group => group.Count())
                .FirstOrDefault();
             
            List<PersonEntity> persons = PersonEntities
                .Where(person => person.Color == groupItem.Key)
                .ToList();

            var groups = PersonEntities
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

        public void RemovePersonEntity(PersonEntity personEntity)
        { 
                PersonEntities.Remove(personEntity);
            MiniUI.instance.ShowQueue(TotalPersons);
        }
    }
}