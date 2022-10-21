using System.Collections.Generic;
using ResourceManagement;
using ResourceManagement.Manager;
using UnityEngine;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        #region Variables
        
        [SerializeField] private BuildingResourcesScriptableObject buildingResources; // Which Resources the building needs/produces
        [SerializeField] private BuildingTypes buildingType; // ENUM
        private int indexOfAllBuildings; // index of the building in allBuildingsList of gameManager
        [SerializeField] private bool isDisabled; // does the Building work?
        private MaterialManager materialManager;
        private EnergyManager energyManager;
        private FoodManager foodManager;
        private WaterManager waterManager;
        private CitizenManager citizenManager;
        private GameManager gameManager;
        private List<ResourceManager> managers;
        private Building nullBuilding;

        private bool lastIsDisabled;
        
        #endregion

        #region Properties

        public BuildingResourcesScriptableObject BuildingResources => buildingResources;
        
        public BuildingTypes BuildingType => buildingType;

        public bool IsDisabled
        {
            get => isDisabled;
            set => isDisabled = value;
        }
        
        #endregion

        #region UnityEvents
        
        /// <summary>
        /// sets all Variables
        /// reduces needed resources when build the building
        /// adds building to gameManager building count
        /// </summary>
        private void Start()
        {
            materialManager = MaterialManager.Instance;
            managers.Add(materialManager);
            energyManager = EnergyManager.Instance;
            managers.Add(energyManager);
            foodManager = FoodManager.Instance;
            managers.Add(foodManager);
            waterManager = WaterManager.Instance;
            managers.Add(waterManager);
            citizenManager = CitizenManager.Instance;
            managers.Add(citizenManager);
            gameManager = GameManager.Instance;
            nullBuilding = gameManager.NullBuilding;
            BuildModule();
            EnableModule();
        }
        private void Update() // Can be solved as usual Method via UI                    TODO
        {
            if (isDisabled == lastIsDisabled) return;
            
            if (isDisabled)
            {
                DisableModule();
            }
            else
            {
                EnableModule();
            }

            lastIsDisabled = isDisabled;
        }

        /// <summary>
        /// disables function of building when it gets deleted
        /// unsubscribes building from gameManagerList
        /// </summary>
        private void OnDestroy()
        {
            if (!isDisabled)
            {
                DisableModule();
            }

            gameManager.AllBuildings[indexOfAllBuildings] = nullBuilding;
        }
        
        #endregion

        #region Methods

        private void BuildModule()
        {
            foreach (Resource item in buildingResources.Costs)
            {
                foreach (ResourceManager jtem in managers)
                {
                    if (item.resource == jtem.ResourceType)
                    {
                        if (jtem.SavedResourceValue < item.value)
                        {
                            Debug.LogError($"Not enough {item.resource} to build this Module.");
                        }
                        else
                        {
                            jtem.SavedResourceValue -= item.value;                      // TODO nur kontrollieren, ob genug Citizen vorhanden sind???, außer wert ist negativ wegen startmodul
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
        }
        
        
        /// <summary>
        /// adds production, consumption and saveSpace
        /// </summary>
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
                        citizenManager.CurrentResourceDemand -= item.value;
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
                        citizenManager.CurrentResourceDemand += item.value; 
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
                        citizenManager.SaveSpace += item.value;
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

        /// <summary>
        /// reduces production, consumption and saveSpace
        /// </summary>
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
                        citizenManager.CurrentResourceDemand += item.value;
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
                        citizenManager.CurrentResourceDemand -= item.value; 
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
                        citizenManager.SaveSpace -= item.value;
                        break;
                    case ResourceTypes.Food:
                        foodManager.SaveSpace -= item.value;
                        break;
                    case ResourceTypes.Water:
                        waterManager.SaveSpace -= item.value;
                        break;
                }
            }
            
            Debug.Log(BuildingType + " is Disabled");
        }
        
        #endregion
    }
}
