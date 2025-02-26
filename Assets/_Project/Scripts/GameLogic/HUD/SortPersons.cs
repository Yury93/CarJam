using _Project.Scripts.GameLogic;
using _Project.Scripts.Infrastructure.Services.PersonPool;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SortPersons : MonoBehaviour
{
    [SerializeField] private PersonSpawner _personSpawner;
    [SerializeField] private Button _sortPersonButton;
    private PathBuilder _pathBuilder;
    public void Construct(PathBuilder pathBuilder)
    {
        this._pathBuilder = pathBuilder;
    }
    private void Start()
    {
        _sortPersonButton.onClick.AddListener(SortPerson);
    }
    
    private void SortPerson()
    {
        List<MaterialProperty> materials = new List<MaterialProperty>();
        foreach (var person in _personSpawner.PersonOnLevel)
        {
            if (person.InCar == false)
                materials.Add(person.MaterialProperty);
        }
        materials = materials.OrderBy(c => c.ColorTag).ToList();
        int refreshColorCount = 0;
        foreach (var person in _personSpawner.PersonOnLevel)
        {
            if (person.InCar == false)
            {
                person.RefreshColor(materials[refreshColorCount]);
                refreshColorCount++;
            }
        }
        foreach (var item in _pathBuilder.Stands)
        {
            if(item.Car)
            _personSpawner.CheckStopCar(item.Car);
        }
        
    }
}