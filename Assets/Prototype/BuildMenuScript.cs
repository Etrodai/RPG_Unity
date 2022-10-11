using System;
using Buildings;
using ResourceManagement;
using ResourceManagement.Manager;
using UnityEngine;

public class BuildMenuScript : MonoBehaviour
{
    [SerializeField] private Buttons[] buttons;
    private MaterialManager materialManager;
    private EnergyManager energyManager;
    private FoodManager foodManager;
    private WaterManager waterManager;
    private CitizenManager citizenManager;

    private void Start()
    {
        materialManager = MaterialManager.Instance;
        energyManager = EnergyManager.Instance;
        foodManager = FoodManager.Instance;
        waterManager = WaterManager.Instance;
        citizenManager = CitizenManager.Instance;
    }

    private void OnEnable()
    {
        foreach (Buttons jtem in buttons)
        {
            foreach (Resource item in jtem.ModuleToBuild.Costs)
            {
                jtem.Button.SetActive(true);
                
                switch (item.resource)
                {
                    case ResourceTypes.Material:
                        if (materialManager.SavedResourceValue < item.value)
                        {
                            jtem.Button.SetActive(false);
                            return; 
                        }
                        break;
                    case ResourceTypes.Energy:
                        if (energyManager.SavedResourceValue < item.value)
                        {
                            jtem.Button.SetActive(false);
                            return; 
                        }
                        break;
                    case ResourceTypes.Citizen:
                        if (citizenManager.Citizen < item.value)
                        {
                            jtem.Button.SetActive(false);
                            return; 
                        }
                        break;
                    case ResourceTypes.Food:
                        if (foodManager.SavedResourceValue < item.value)
                        {
                            jtem.Button.SetActive(false);
                            return; 
                        }
                        break;
                    case ResourceTypes.Water:
                        if (waterManager.SavedResourceValue < item.value)
                        {
                            jtem.Button.SetActive(false);
                            return; 
                        }
                        break;
                }
            }
        }
    }
}

[System.Serializable]
public struct Buttons
{
    public GameObject Button;
    public BuildingResourcesScriptableObject ModuleToBuild;
}
