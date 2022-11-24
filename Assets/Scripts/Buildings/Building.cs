using System.Collections.Generic;
using Manager;
using PriorityListSystem;
using ResourceManagement;
using ResourceManagement.Manager;
using Sound;
using UnityEngine;
using UnityEngine.Events;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Buildings //Made by Robin
{
    [System.Serializable]
    public struct BuildingData
    {
        public bool isDisabled;
        public float currentProductivity;
        public int buildingType;
    }
    
    public class Building : MonoBehaviour
    {
        #region Variables
        
        [SerializeField] private BuildingResourcesScriptableObject buildingResources; // Which Resources the building needs/produces
        [SerializeField] private BuildingType buildingType; // ENUM
        private int indexOfAllBuildings; // index of the building in allBuildingsList of gameManager
        [SerializeField] private bool isDisabled; // does the Building work?
        private float currentProductivity = 1.0f;
        private MaterialManager materialManager;
        private EnergyManager energyManager;
        private FoodManager foodManager;
        private WaterManager waterManager;
        private CitizenManager citizenManager;
        private GameManager gameManager;
        private List<ResourceManager> managers;
        private Building nullBuilding;

        #endregion

        #region Events
        
        /// <summary>
        /// 0: old productivity
        /// 1: new productivity
        /// </summary>
        private readonly UnityEvent<float, float> onBuildingProductivityChanged = new();

        #endregion
        
        #region Properties

        public BuildingResourcesScriptableObject BuildingResources => buildingResources;
        
        public BuildingType BuildingType => buildingType;
        
        public bool IsDisabled
        {
            get => isDisabled;
            set => isDisabled = value;
        }

        //OnValueChangedEvent
        public float CurrentProductivity
        {
            get => currentProductivity;
            set
            {
                if (currentProductivity == value) return;

                float oldValue = currentProductivity;
                Debug.Log("onBuildingProductivityChanged.Invoke(currentProductivity, value)");
                currentProductivity = value;
                onBuildingProductivityChanged.Invoke(oldValue, value);

            }
        }
        
        public bool IsLoading { get; set; }

        #endregion

        #region UnityEvents
        
        /// <summary>
        /// adds Listener
        /// sets all Variables
        /// reduces needed resources when build the building
        /// adds building to gameManager building count
        /// actualizes PriorityUI
        /// </summary>
        private void Start()
        {
            onBuildingProductivityChanged.AddListener(ChangeProductivity);

            managers = new ();
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
            gameManager = MainManagerSingleton.Instance.GameManager;
            nullBuilding = gameManager.NullBuilding;
            
            if (!IsLoading) BuildModule();

            EnableModule(CurrentProductivity);
            for (int i = 0; i < gameManager.PriorityListItems.Count; i++)
            {
                PriorityListItem item = gameManager.PriorityListItems[i];
                item.onChangePriorityUI.Invoke();
            }
        }

        /// <summary>
        /// disables function of building when it gets deleted
        /// unsubscribes building from gameManagerList
        /// removes Listener
        /// </summary>
        private void OnDestroy()
        {
            if (!IsDisabled)
            {
                DisableModule(CurrentProductivity);
            }
            onBuildingProductivityChanged.RemoveListener(ChangeProductivity);

            gameManager.AllBuildings[indexOfAllBuildings] = nullBuilding;
        }
        
        #endregion

        #region Save Load

        /// <summary>
        /// saves building data
        /// </summary>
        /// <returns></returns>
        public BuildingData SaveBuildingData()
        {
            BuildingData data = new();
            data.isDisabled = isDisabled;
            data.currentProductivity = currentProductivity;
            data.buildingType = (int)buildingType;
            
            return data;
        }

        #endregion
        
        #region Methods
        
        /// <summary>
        /// Changes Productivity by the given values, when onBuildingProductivity event triggers
        /// </summary>
        /// <param name="oldProductivity">old Value</param>
        /// <param name="newProductivity">new Value</param>
        private void ChangeProductivity(float oldProductivity, float newProductivity)
        {
            if (oldProductivity > newProductivity) DisableModule(oldProductivity - newProductivity);
            else EnableModule(newProductivity - oldProductivity);
        }
        
        /// <summary>
        /// takes all Resources of building's costs
        /// adds building to gameManager building count
        /// </summary>
        private void BuildModule()
        {
            for (int i = 0; i < buildingResources.Costs.Length; i++)
            {
                Resource cost = buildingResources.Costs[i];
                for (int j = 0; j < managers.Count; j++)
                {
                    ResourceManager manager = managers[j];
                    if (cost.resource != manager.ResourceType) continue;
                    
                        manager.SavedResourceValue -= cost.value;
                }
            }

            int empty = gameManager.GetIndexOfFirstEmpty();
            if (empty == -1)
            {
                gameManager.AllBuildings.Add(this);
                indexOfAllBuildings = gameManager.AllBuildings.Count - 1;
            }
            else
            {
                gameManager.AllBuildings[empty] = this;
                indexOfAllBuildings = empty;
            }
            
            SoundManager.PlaySound(SoundManager.Sound.BuildModule);
        }
        
        /// <summary>
        /// adds production, consumption and saveSpace
        /// </summary>
        private void EnableModule(float productivity)
        {
            for (int i = 0; i < buildingResources.Production.Length; i++)
            {
                Resource production = buildingResources.Production[i];
                for (var j = 0; j < managers.Count; j++)
                {
                    ResourceManager manager = managers[j];
                    if (production.resource != manager.ResourceType) continue;
                    manager.CurrentResourceProduction += production.value * productivity;
                }
            }

            for (int i = 0; i < buildingResources.Consumption.Length; i++)
            {
                Resource consumption = buildingResources.Consumption[i];
                for (int j = 0; j < managers.Count; j++)
                {
                    ResourceManager manager = managers[j];
                    if (consumption.resource != manager.ResourceType) continue;
                    manager.CurrentResourceDemand += consumption.value * productivity;
                }
            }

            for (int i = 0; i < buildingResources.SaveSpace.Length; i++)
            {
                Resource space = buildingResources.SaveSpace[i];
                for (int j = 0; j < managers.Count; j++)
                {
                    ResourceManager manager = managers[j];
                    if (space.resource != manager.ResourceType) continue;
                    manager.SaveSpace += space.value * productivity;
                }
            }
        }

        /// <summary>
        /// reduces production, consumption and saveSpace
        /// </summary>
        private void DisableModule(float productivity)
        {
            for (var i = 0; i < buildingResources.Production.Length; i++)
            {
                Resource production = buildingResources.Production[i];
                for (int j = 0; j < managers.Count; j++)
                {
                    ResourceManager manager = managers[j];
                    if (production.resource != manager.ResourceType) continue;
                    manager.CurrentResourceProduction -= production.value * productivity;
                }
            }

            for (int i = 0; i < buildingResources.Consumption.Length; i++)
            {
                Resource consumption = buildingResources.Consumption[i];
                for (var j = 0; j < managers.Count; j++)
                {
                    ResourceManager manager = managers[j];
                    if (consumption.resource != manager.ResourceType) continue;
                    manager.CurrentResourceDemand -= consumption.value * productivity;
                }
            }

            for (int i = 0; i < buildingResources.SaveSpace.Length; i++)
            {
                Resource space = buildingResources.SaveSpace[i];
                for (int j = 0; j < managers.Count; j++)
                {
                    ResourceManager manager = managers[j];
                    if (space.resource != manager.ResourceType) continue;
                    manager.SaveSpace -= space.value * productivity;
                }
            }
        }
        
        #endregion
    }
}
