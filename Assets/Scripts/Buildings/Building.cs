using System;
using System.Collections.Generic;
using Manager;
using PriorityListSystem;
using ResourceManagement;
using ResourceManagement.Manager;
using Sound;
using UnityEngine;
using UnityEngine.Events;

namespace Buildings
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
        [SerializeField] private BuildingTypes buildingType; // ENUM
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
        /// 0: shows, if it was disabled
        /// </summary>
        private UnityEvent<bool> onBuildingWasDisabled = new();
        
        /// <summary>
        /// 0: old productivity
        /// 1: new productivity
        /// </summary>
        private UnityEvent<float, float> onBuildingProductivityChanged = new();

        #endregion
        
        #region Properties

        public BuildingResourcesScriptableObject BuildingResources => buildingResources;
        
        public BuildingTypes BuildingType => buildingType;

        //OnValueChangedEvent
        public bool IsDisabled
        {
            get => isDisabled;
            set
            {
                if (isDisabled == value) return;

                isDisabled = value;
                onBuildingWasDisabled?.Invoke(isDisabled);
                Debug.Log("onBuildingWasDisabled?.Invoke(isDisabled)");
            }
        }

        //OnValueChangedEvent
        public float CurrentProductivity
        {
            get => currentProductivity;
            set
            {
                if (currentProductivity == value) return;
                
                onBuildingProductivityChanged.Invoke(currentProductivity, value);
                Debug.Log("onBuildingProductivityChanged.Invoke(currentProductivity, value)");
                currentProductivity = value;
            }
        }
        
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
            onBuildingWasDisabled.AddListener(ChangeIsDisabled);
            onBuildingProductivityChanged.AddListener(ChangeProductivity);
            
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
            gameManager = MainManagerSingleton.Instance.GameManager;
            nullBuilding = gameManager.NullBuilding;
            
            BuildModule();
            
            EnableModule(CurrentProductivity, false);
            foreach (PriorityListItem item in gameManager.PriorityListItems)
            {
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
                DisableModule(CurrentProductivity, false);
            }
            onBuildingWasDisabled.RemoveListener(ChangeIsDisabled);
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
            BuildingData data = new BuildingData();
            data.isDisabled = isDisabled;
            data.currentProductivity = currentProductivity;
            data.buildingType = (int)buildingType;
            
            return data;
        }
        
        
        /// <summary>
        /// loads building data
        /// </summary>
        /// <param name="data">data of loaded building</param>
        public void LoadBuildingData(BuildingData data)
        {
            isDisabled = data.isDisabled;
            currentProductivity = data.currentProductivity;
            buildingType = (BuildingTypes)data.buildingType;
        }
        
        #endregion
        
        #region Methods

        /// <summary>
        /// Disables or Enables Modules, when onBuildingWasDisabled event triggers
        /// </summary>
        /// <param name="disabled">shows, if it gets disabled or enabled</param>
        private void ChangeIsDisabled(bool disabled)
        {
            if (disabled) DisableModule(CurrentProductivity, true);
            else EnableModule(CurrentProductivity, true);
        }

        /// <summary>
        /// Changes Productivity by the given values, when onBuildingProductivity event triggers
        /// </summary>
        /// <param name="oldProductivity">old Value</param>
        /// <param name="newProductivity">new Value</param>
        private void ChangeProductivity(float oldProductivity, float newProductivity)
        {
            if (oldProductivity > newProductivity) DisableModule(oldProductivity - newProductivity, true);
            else EnableModule(newProductivity - oldProductivity, true);
        }
        
        /// <summary>
        /// takes all Resources of building's costs
        /// adds building to gameManager building count
        /// </summary>
        private void BuildModule()
        {
            foreach (Resource cost in buildingResources.Costs)
            {
                foreach (ResourceManager manager in managers)
                {
                    if (cost.resource == manager.ResourceType)
                    {
                        if (manager.SavedResourceValue < cost.value)
                        {
                            Debug.LogError($"Not enough {cost.resource} to build this Module.");
                        }
                        else
                        {
                            manager.SavedResourceValue -= cost.value;
                        }
                    }
                }
                // switch (item.resource)
                // {
                //     case ResourceTypes.Material:
                //         if (materialManager.SavedResourceValue < item.value)
                //             Debug.LogError("Not enough Material to build this Module.");
                //         else materialManager.SavedResourceValue -= item.value;
                //         break;
                //     case ResourceTypes.Energy:
                //         if (energyManager.SavedResourceValue < item.value)
                //             Debug.LogError("Not enough Energy to build this Module.");
                //         else energyManager.SavedResourceValue -= item.value;
                //         break;
                //     case ResourceTypes.Citizen:
                //         if (citizenManager.CurrentResourceProduction < item.value) Debug.LogError("Not enough Citizen to build this Module.");
                //         else citizenManager.CurrentResourceProduction -= item.value;
                //         break;
                //     case ResourceTypes.Food:
                //         if (foodManager.SavedResourceValue < item.value)
                //             Debug.LogError("Not enough Food to build this Module.");
                //         else foodManager.SavedResourceValue -= item.value;
                //         break;
                //     case ResourceTypes.Water:
                //         if (waterManager.SavedResourceValue < item.value)
                //             Debug.LogError("Not enough Water to build this Module.");
                //         else waterManager.SavedResourceValue -= item.value;
                //         break;
                // }
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
        private void EnableModule(float productivity, bool playSound)
        {
            foreach (Resource production in buildingResources.Production)
            {
                foreach (ResourceManager manager in managers)
                {
                    if (production.resource == manager.ResourceType)
                    {
                        manager.CurrentResourceProduction += production.value * productivity;
                    }
                }
                
                // switch (item.resource)
                // {
                //     case ResourceTypes.Material:
                //         materialManager.CurrentResourceProduction += item.value;
                //         break;
                //     case ResourceTypes.Energy:
                //         energyManager.CurrentResourceProduction += item.value;
                //         break;
                //     case ResourceTypes.Citizen:
                //         citizenManager.CurrentResourceDemand -= item.value; // gibt es eine Produktion???
                //         break;
                //     case ResourceTypes.Food:
                //         foodManager.CurrentResourceProduction += item.value;
                //         break;
                //     case ResourceTypes.Water:
                //         waterManager.CurrentResourceProduction += item.value;
                //         break;
                // }
            }

            foreach (Resource consumption in buildingResources.Consumption)
            {
                foreach (ResourceManager manager in managers)
                {
                    if (consumption.resource == manager.ResourceType)
                    {
                        manager.CurrentResourceDemand += consumption.value * productivity;
                    }
                }
                
                // switch (item.resource)
                // {
                //     case ResourceTypes.Material:
                //         materialManager.CurrentResourceDemand += item.value;
                //         break;
                //     case ResourceTypes.Energy:
                //         energyManager.CurrentResourceDemand += item.value;
                //         break;
                //     case ResourceTypes.Citizen:
                //         citizenManager.CurrentResourceDemand += item.value; 
                //         break;
                //     case ResourceTypes.Food:
                //         foodManager.CurrentResourceDemand += item.value;
                //         break;
                //     case ResourceTypes.Water:
                //         waterManager.CurrentResourceDemand += item.value;
                //         break;
                // }
            }

            foreach (Resource space in buildingResources.SaveSpace)
            {
                foreach (ResourceManager manager in managers)
                {
                    if (space.resource == manager.ResourceType)
                    {
                        manager.SaveSpace += space.value * productivity;
                    }
                }
                
                // switch (item.resource)
                // {
                //     case ResourceTypes.Material:
                //         materialManager.SaveSpace += item.value;
                //         break;
                //     case ResourceTypes.Energy:
                //         energyManager.SaveSpace += item.value;
                //         break;
                //     case ResourceTypes.Citizen:
                //         citizenManager.SaveSpace += item.value;
                //         break;
                //     case ResourceTypes.Food:
                //         foodManager.SaveSpace += item.value;
                //         break;
                //     case ResourceTypes.Water:
                //         waterManager.SaveSpace += item.value;
                //         break;
                // }
            }

            if (playSound)
            {
                SoundManager.PlaySound(SoundManager.Sound.EnableModule);
            }
        }

        /// <summary>
        /// reduces production, consumption and saveSpace
        /// </summary>
        private void DisableModule(float productivity, bool playSound)
        {
            foreach (Resource production in buildingResources.Production)
            {
                foreach (ResourceManager manager in managers)
                {
                    if (production.resource == manager.ResourceType)
                    {
                        manager.CurrentResourceProduction -= production.value * productivity;
                    }
                }
                
                // switch (item.resource)
                // {
                //     case ResourceTypes.Material:
                //         materialManager.CurrentResourceProduction -= item.value;
                //         break;
                //     case ResourceTypes.Energy:
                //         energyManager.CurrentResourceProduction -= item.value;
                //         break;
                //     case ResourceTypes.Citizen:
                //         citizenManager.CurrentResourceDemand += item.value;
                //         break;
                //     case ResourceTypes.Food:
                //         foodManager.CurrentResourceProduction -= item.value;
                //         break;
                //     case ResourceTypes.Water:
                //         waterManager.CurrentResourceProduction -= item.value;
                //         break;
                // }
            }

            foreach (Resource consumption in buildingResources.Consumption)
            {
                foreach (ResourceManager manager in managers)
                {
                    if (consumption.resource == manager.ResourceType)
                    {
                        manager.CurrentResourceDemand -= consumption.value * productivity;
                    }
                }
                
                // switch (item.resource)
                // {
                //     case ResourceTypes.Material:
                //         materialManager.CurrentResourceDemand -= item.value;
                //         break;
                //     case ResourceTypes.Energy:
                //         energyManager.CurrentResourceDemand -= item.value;
                //         break;
                //     case ResourceTypes.Citizen:
                //         citizenManager.CurrentResourceDemand -= item.value; 
                //         break;
                //     case ResourceTypes.Food:
                //         foodManager.CurrentResourceDemand -= item.value;
                //         break;
                //     case ResourceTypes.Water:
                //         waterManager.CurrentResourceDemand -= item.value;
                //         break;
                // }
            }

            foreach (Resource space in buildingResources.SaveSpace)
            {
                foreach (ResourceManager manager in managers)
                {
                    if (space.resource == manager.ResourceType)
                    {
                        manager.SaveSpace -= space.value * productivity;
                    }
                }
                
                // switch (item.resource)
                // {
                //     case ResourceTypes.Material:
                //         materialManager.SaveSpace -= item.value;
                //         break;
                //     case ResourceTypes.Energy:
                //         energyManager.SaveSpace -= item.value;
                //         break;
                //     case ResourceTypes.Citizen:
                //         citizenManager.SaveSpace -= item.value;
                //         break;
                //     case ResourceTypes.Food:
                //         foodManager.SaveSpace -= item.value;
                //         break;
                //     case ResourceTypes.Water:
                //         waterManager.SaveSpace -= item.value;
                //         break;
                // }
            }

            if (playSound)
            {
                SoundManager.PlaySound(SoundManager.Sound.DisableModule);
            }
        }
        
        #endregion
    }
}
