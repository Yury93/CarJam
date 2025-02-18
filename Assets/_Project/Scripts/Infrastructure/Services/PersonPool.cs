using _Project.Scripts.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

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
                    PersonEntities.Add(new PersonEntity(car.Color));
                }
            }
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
        }
    }
    [Serializable]
    public struct PersonEntity
    {
        public Color Color;
        public PersonEntity(Color color)
        {
            this.Color = color;
        }
    }
    public interface IPersonPool : IService
    {
        public int TotalPersons { get; }
        public List<PersonEntity> PersonEntities { get; }
        List<PersonEntity> CreatePool(List<ICarData> carDatas);
        List<PersonEntity> GetPersonsGroup();
        void RemovePersonEntity( PersonEntity  personColor);
    }
}