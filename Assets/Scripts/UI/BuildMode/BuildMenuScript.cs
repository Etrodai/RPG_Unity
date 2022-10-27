using System.Collections.Generic;
using Buildings;
using Manager;
using ResourceManagement;
using ResourceManagement.Manager;
using UnityEngine;

namespace UI.BuildMode
{
    public class BuildMenuScript : MonoBehaviour
    {
        [SerializeField] private Buttons[] buttons;
        private MaterialManager materialManager;
        private EnergyManager energyManager;
        private FoodManager foodManager;
        private WaterManager waterManager;
        private CitizenManager citizenManager;
        private List<ResourceManager> managers;
        private bool isInitialized;

        private void Initialize()
        {
            managers = new List<ResourceManager>();
            materialManager = MainManagerSingleton.Instance.MaterialManager;
            managers.Add(materialManager);
            energyManager = MainManagerSingleton.Instance.EnergyManager;
            managers.Add(energyManager);
            foodManager = MainManagerSingleton.Instance.FoodManager;
            managers.Add(foodManager);
            waterManager = MainManagerSingleton.Instance.WaterManager;
            managers.Add(waterManager);
            citizenManager = MainManagerSingleton.Instance.CitizenManager;
            managers.Add(citizenManager);
        }

        private void OnEnable()
        {
            if (!isInitialized)
            {
                Initialize();
                isInitialized = true;
            }
        
            foreach (Buttons button in buttons)
            {
                bool activate = true;
                foreach (Resource cost in button.ModuleToBuild.Costs)
                {
                    foreach (ResourceManager manager in managers)
                    {
                        if (manager.ResourceType == cost.resource)
                        {
                            if (manager.SavedResourceValue < cost.value)
                            {
                                activate = false;
                            }
                        }
                    }
                }
                button.Button.SetActive(activate);
            }
        }
    }

    [System.Serializable]
    public struct Buttons
    {
        public GameObject Button;
        public BuildingResourcesScriptableObject ModuleToBuild;
    }
}