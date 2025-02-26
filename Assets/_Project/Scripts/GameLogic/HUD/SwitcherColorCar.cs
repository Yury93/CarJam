using _Project.Scripts.GameLogic;
using _Project.Scripts.Helper;
using _Project.Scripts.Infrastructure.Services;
using _Project.Scripts.Infrastructure.Services.PersonPool;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwitcherColorCar : MonoBehaviour
{
    [SerializeField] private PersonSpawner _personSpawner;
    [SerializeField] private Transform carContent;
    [SerializeField] private Button switchCarColor;
    private IMaterialsPool _poolMaterials;
    private IPersonPool _personPool;
    public List<Car> Cars = new List<Car>();
    bool _switchColorProcess;
    public void Construct(IMaterialsPool poolMaterials, IPersonPool personPool)
    {
        _poolMaterials = poolMaterials; 
        _personPool = personPool;
    }
    public void Init()
    {
        var carTransforms = carContent.GetChilds(carContent).Where(c => c.GetComponent<Car>()).ToList();
        foreach (var carTransform in carTransforms)
        {
           var car = carTransform.GetComponent<Car>();
            Cars.Add(car);
            var mover = carTransform.GetComponent<CarMover>();
            mover.onExit += OnCarExit;
        }
        switchCarColor.onClick.AddListener(SwitchColor);
    }
    private void OnCarExit(CarMover mover)
    {
        mover.onExit -= OnCarExit;
        Cars.Remove(mover.GetComponent<Car>());
    }
    private void SwitchColor()
    {
        if (_switchColorProcess) return;
        StartCoroutine(CorSwitch()); 
    }
    IEnumerator CorSwitch()
    {
        var groupsByTag = Cars.GroupBy(c => c.MaterialProperty.ColorTag);
        List<ICarData> carsData = new List<ICarData>();

        foreach (var item in Cars)
        {
            if(item.IsFull == false)
            carsData.Add(item);
        }
        int trials = 0;
        while (trials < 3)
        {
            List<MaterialProperty> materials = new List<MaterialProperty>();
            foreach (var group in groupsByTag)
            {
                materials.Add(group.First().MaterialProperty);
            }
            materials.Shuffle();
            foreach (var group in groupsByTag)
            {
                var rndMaterial = UnityEngine.Random.Range(0, materials.Count);
                foreach (var item in group)
                {
                    if(item.GetComponent<CarMover>().IsStand == false)
                    item.SwitchColorType(_poolMaterials.GetMaterial(materials[rndMaterial].ColorTag));
                }
                materials.Remove(materials[rndMaterial]);
            }
            yield return new WaitForSeconds(0.2f);
            trials++;
        }
        List<MaterialProperty> personMaterials = new List<MaterialProperty>();
        foreach (var item in _personSpawner.PersonOnLevel)
        {
            personMaterials.Add(item.MaterialProperty);
        }


        _personPool.CreatePool(carsData,personMaterials);
        _switchColorProcess = false;
    }
    private void OnDestroy()
    {
        switchCarColor.onClick.RemoveAllListeners();
    }
}
