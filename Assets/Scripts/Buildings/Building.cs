using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private BuildingResourcesScriptableObject buildingResources;
    [SerializeField] private BuildingTypes buildingType;
    private int indexOfAllBuildings;
    [SerializeField] private bool isDisabled;
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
        
        foreach (Resource item in buildingResources.Costs)
        {
            switch (item.resource)
            {
                case ResourceTypes.Material:
                    if (materialManager.SavedResourceValue < item.value)
                    {
                        Debug.Log("Not enough Material to build this Module.");
                    }
                    else
                    {
                        materialManager.SavedResourceValue -= item.value;
                    }
                    break;
                case ResourceTypes.Energy:
                    if (energyManager.SavedResourceValue < item.value)
                    {
                        Debug.Log("Not enough Energy to build this Module.");
                    }
                    else
                    {
                        energyManager.SavedResourceValue -= item.value;
                    }
                    break; 
                case ResourceTypes.Citizen:
                    if (citizenManager.Citizen < item.value)
                    {
                        Debug.Log("Not enough Citizen to build this Module.");
                    }
                    break;
                case ResourceTypes.Food:
                    if (foodManager.SavedResourceValue < item.value)
                    {
                        Debug.Log("Not enough Food to build this Module.");
                    }
                    else
                    {
                        foodManager.SavedResourceValue -= item.value;
                    }
                    break;
                case ResourceTypes.Water:
                    if (waterManager.SavedResourceValue < item.value)
                    {
                        Debug.Log("Not enough Water to build this Module.");
                    }
                    else
                    {
                        waterManager.SavedResourceValue -= item.value;
                    }
                    break;
            }
        }

        GameManager.Instance.AllBuildings.Add(buildingType);
        indexOfAllBuildings = GameManager.Instance.AllBuildings.Count - 1;
        EnableModule();
    }

    // private void Update() // Can be solved as usual Method via UI                    TODO
    // {
    //     if (isDisabled)
    //     {
    //         DisableModule();
    //     }
    //     else
    //     {
    //         EnableModule();
    //     }
    // }

    private void EnableModule()
    {
        foreach (Resource item in buildingResources.Production)
        {
            switch (item.resource)
            {
                case ResourceTypes.Material:
                    materialManager.CurrentResourceProduction += item.value;
                    break;
                case ResourceTypes.Energy:
                    energyManager.CurrentResourceProduction += item.value;
                    break; 
                case ResourceTypes.Citizen:
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
                case ResourceTypes.Material:
                    materialManager.CurrentResourceDemand += item.value;
                    break;
                case ResourceTypes.Energy:
                    energyManager.CurrentResourceDemand += item.value;
                    break; 
                case ResourceTypes.Citizen:
                    break;
                case ResourceTypes.Food:
                    foodManager.CurrentResourceDemand += item.value;
                    break;
                case ResourceTypes.Water:
                    waterManager.CurrentResourceDemand += item.value;
                    break;
            }
        }

        foreach (Resource item in buildingResources.SaveSpace)
        {
            switch (item.resource)
            {
                case ResourceTypes.Material:
                    materialManager.SaveSpace += item.value;
                    break;
                case ResourceTypes.Energy:
                    energyManager.SaveSpace += item.value;
                    break;
                case ResourceTypes.Citizen:
                    citizenManager.Housing += item.value;
                    break;
                case ResourceTypes.Food:
                    foodManager.SaveSpace += item.value;
                    break;
                case ResourceTypes.Water:
                    waterManager.SaveSpace += item.value;
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
                case ResourceTypes.Material:
                    materialManager.CurrentResourceProduction -= item.value;
                    break;
                case ResourceTypes.Energy:
                    energyManager.CurrentResourceProduction -= item.value;
                    break; 
                case ResourceTypes.Citizen:
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
                case ResourceTypes.Material:
                    materialManager.CurrentResourceDemand -= item.value;
                    break;
                case ResourceTypes.Energy:
                    energyManager.CurrentResourceDemand -= item.value;
                    break; 
                case ResourceTypes.Citizen:
                    break;
                case ResourceTypes.Food:
                    foodManager.CurrentResourceDemand -= item.value;
                    break;
                case ResourceTypes.Water:
                    waterManager.CurrentResourceDemand -= item.value;
                    break;
            }
        }
        
        foreach (Resource item in buildingResources.SaveSpace)
        {
            switch (item.resource)
            {
                case ResourceTypes.Material:
                    materialManager.SaveSpace -= item.value;
                    break;
                case ResourceTypes.Energy:
                    energyManager.SaveSpace -= item.value;
                    break;
                case ResourceTypes.Citizen:
                    citizenManager.Housing -= item.value;
                    break;
                case ResourceTypes.Food:
                    foodManager.SaveSpace -= item.value;
                    break;
                case ResourceTypes.Water:
                    waterManager.SaveSpace -= item.value;
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        if (!isDisabled)
        {
            DisableModule();
        }

        GameManager.Instance.AllBuildings[indexOfAllBuildings] = BuildingTypes.Empty;
    }
}
