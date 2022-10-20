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
    private bool isInitialized;

    private void Initialize()
    {
        materialManager = MaterialManager.Instance;
        energyManager = EnergyManager.Instance;
        foodManager = FoodManager.Instance;
        waterManager = WaterManager.Instance;
        citizenManager = CitizenManager.Instance;
    }

    private void OnEnable()
    {
        if (!isInitialized)
        {
            Initialize();
            isInitialized = true;
        }
        
        foreach (Buttons jtem in buttons)
        {
            bool activate = true;
            foreach (Resource item in jtem.ModuleToBuild.Costs)
            {
                switch (item.resource)
                {
                    case ResourceTypes.Material:
                        if (materialManager.SavedResourceValue < item.value)
                        {
                            activate = false;
                        }
                        break;
                    case ResourceTypes.Energy:
                        if (energyManager.SavedResourceValue < item.value)
                        {
                            activate = false;
                        }
                        break;
                    case ResourceTypes.Citizen:
                        if (citizenManager.Citizen < item.value)
                        {
                            activate = false;
                        }
                        break;
                    case ResourceTypes.Food:
                        if (foodManager.SavedResourceValue < item.value)
                        {
                            activate = false;
                        }
                        break;
                    case ResourceTypes.Water:
                        if (waterManager.SavedResourceValue < item.value)
                        {
                            activate = false;
                        }
                        break;
                }
            }
            jtem.Button.SetActive(activate);
        }
    }
}

[System.Serializable]
public struct Buttons
{
    public GameObject Button;
    public BuildingResourcesScriptableObject ModuleToBuild;
}
