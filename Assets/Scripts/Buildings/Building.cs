using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private BuildingResourcesScriptableObject buildingResources;
    [SerializeField] private bool isDisabled;
    private BuilingResourceManager buildingResourceManager;
    private EnergyManager energyManager;
    private FoodManager foodManager;
    private WaterManager waterManager;

    private void Awake()
    {
        buildingResourceManager = BuilingResourceManager.Instance;
        energyManager = EnergyManager.Instance;
        foodManager = FoodManager.Instance;
        waterManager = WaterManager.Instance;
    }

    private void Start()
    {
        foreach (Resource item in buildingResources.Costs)
        {
            switch (item.resource)
            {
                case ResourceTypes.BuildingResource:
                    buildingResourceManager.CurrentResourceValue -= item.value;
                    break;
                case ResourceTypes.Energy:
                    energyManager.CurrentResourceValue -= item.value;
                    break; 
                case ResourceTypes.Inhabitant:
                    break;
                case ResourceTypes.Food:
                    foodManager.CurrentResourceValue -= item.value;
                    break;
                case ResourceTypes.Water:
                    waterManager.CurrentResourceValue -= item.value;
                    break;
            }
        }
        
        EnableModule();
    }

    private void Update() // Can be solved as usual Method via UI
    {
        if (isDisabled)
        {
            DisableModule();
        }
        else
        {
            EnableModule();
        }
    }

    private void EnableModule()
    {
        foreach (Resource item in buildingResources.Production)
        {
            switch (item.resource)
            {
                case ResourceTypes.BuildingResource:
                    buildingResourceManager.CurrentResourceProduction += item.value;
                    break;
                case ResourceTypes.Energy:
                    energyManager.CurrentResourceProduction += item.value;
                    break; 
                case ResourceTypes.Inhabitant:
                    break;
                case ResourceTypes.Food:
                    foodManager.CurrentResourceProduction += item.value;
                    break;
                case ResourceTypes.Water:
                    waterManager.CurrentResourceProduction += item.value;
                    break;
            }
        }
        
        foreach (Resource item in buildingResources.Consumption)
        {
            switch (item.resource)
            {
                case ResourceTypes.BuildingResource:
                    buildingResourceManager.CurrentResourceDemand += item.value;
                    break;
                case ResourceTypes.Energy:
                    energyManager.CurrentResourceDemand += item.value;
                    break; 
                case ResourceTypes.Inhabitant:
                    break;
                case ResourceTypes.Food:
                    foodManager.CurrentResourceDemand += item.value;
                    break;
                case ResourceTypes.Water:
                    waterManager.CurrentResourceDemand += item.value;
                    break;
            }
        }
    }

    private void DisableModule()
    {
        foreach (Resource item in buildingResources.Production)
        {
            switch (item.resource)
            {
                case ResourceTypes.BuildingResource:
                    buildingResourceManager.CurrentResourceProduction -= item.value;
                    break;
                case ResourceTypes.Energy:
                    energyManager.CurrentResourceProduction -= item.value;
                    break; 
                case ResourceTypes.Inhabitant:
                    break;
                case ResourceTypes.Food:
                    foodManager.CurrentResourceProduction -= item.value;
                    break;
                case ResourceTypes.Water:
                    waterManager.CurrentResourceProduction -= item.value;
                    break;
            }
        }
        
        foreach (Resource item in buildingResources.Consumption)
        {
            switch (item.resource)
            {
                case ResourceTypes.BuildingResource:
                    buildingResourceManager.CurrentResourceDemand -= item.value;
                    break;
                case ResourceTypes.Energy:
                    energyManager.CurrentResourceDemand -= item.value;
                    break; 
                case ResourceTypes.Inhabitant:
                    break;
                case ResourceTypes.Food:
                    foodManager.CurrentResourceDemand -= item.value;
                    break;
                case ResourceTypes.Water:
                    waterManager.CurrentResourceDemand -= item.value;
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        DisableModule();
    }
}
