using System.Collections.Generic;
using Buildings;
using PriorityListSystem;
using ResourceManagement;
using UnityEngine;
using Building = Buildings.Building;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Building nullBuilding;

        #endregion

        #region Properties

        private static GameManager Instance { get; set; }
        public List<Building> AllBuildings { get; } = new();
        public List<PriorityListItem> PriorityListItems { get; } = new();
        public Stack<DisabledBuilding> DisabledBuildings { get; } = new();
        public Stack<Building> ChangedProductivityBuildings { get; } = new();
        public Building NullBuilding => nullBuilding;

        #endregion

        #region UnityEvents

        /// <summary>
        /// singleton
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            Instantiate(nullBuilding);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Searches for an empty slot in allBuildingsList
        /// </summary>
        /// <returns>Index of first empty in allBuildings</returns>
        public int GetIndexOfFirstEmpty()
        {
            return AllBuildings.IndexOf(nullBuilding);
        }
    
        /// <summary>
        /// Searches the Priority of the given BuildingType
        /// </summary>
        /// <param name="type">BuildingType</param>
        /// <returns>Priority of the given BuildingType</returns>
        public int GetPriority(BuildingType type)
        {
            int priority = 0;
            for (int i = 0; i < PriorityListItems.Count; i++)
            {
                var item = PriorityListItems[i];
                if (item.Type == type)
                {
                    priority = item.Priority;
                }
            }

            return priority;
        }

        /// <summary>
        /// Searches Building Type with given Priority
        /// </summary>
        /// <param name="priority">Priority</param>
        /// <returns>type of given Priority, if no Type has given Priority it returns All</returns>
        private BuildingType GetBuildingTypeOnPriority(int priority)
        {
            BuildingType type = BuildingType.All;
            for (int i = 0; i < PriorityListItems.Count; i++)
            {
                PriorityListItem item = PriorityListItems[i];
                if (item.Priority != priority) continue;
                type = item.Type;
            }

            return type;
        }

        /// <summary>
        /// Gets count of one buildingType
        /// </summary>
        /// <param name="type">BuildingType</param>
        /// <returns>value of this buildingType</returns>
        public int GetBuildingCount(BuildingType type)
        {
            int count = 0;
            if (type == BuildingType.All)
            {
                for (int i = 0; i < AllBuildings.Count; i++)
                {
                    Building building = AllBuildings[i];
                    if (building.BuildingType != nullBuilding.BuildingType) count++;
                }

                return count;
            }

            for (int i = 0; i < AllBuildings.Count; i++)
            {
                Building building = AllBuildings[i];
                if (building.BuildingType == type) count++;
            }

            return count;
        }
        
        /// <summary>
        /// Gets count of all working buildings
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetWorkingBuildingCount(BuildingType type)
        {
            int count = GetBuildingCount(type);
            foreach (DisabledBuilding disabledBuilding in DisabledBuildings)
            {
                if (disabledBuilding.building.BuildingType == type) count--;
            }
            
            return count;
        }

        /// <summary>
        /// Gets all buildings of one type
        /// </summary>
        /// <param name="type">BuildingType</param>
        /// <returns>All buildings of given type</returns>
        private List<Building> GetAllBuildingsOfType(BuildingType type)
        {
            List<Building> buildings = new List<Building>();
            for (int i = 0; i < AllBuildings.Count; i++)
            {
                Building building = AllBuildings[i];
                if (building.BuildingType == type) buildings.Add(building);
            }

            return buildings;
        }
        
        /// <summary>
        /// reduces productivity
        /// </summary>
        /// <param name="surplusPerBuilding">value of consumption - production per building (always positive)</param>
        /// <param name="neededResourceValue">amount of needed citizen (always negative)</param>
        /// <param name="priorityList">List of buildings of current priority</param>
        /// <returns>amount of still needed citizen</returns>
        private float ChangeProductivityNegative(float surplusPerBuilding, float neededResourceValue, List<Building> priorityList)
        {
            //TODO: (Robin) why it doesn't work correctly
            for (int i = 0; i < priorityList.Count; i++)
            {
                Building building = priorityList[i];
                if (building.IsDisabled || building.CurrentProductivity == 0) continue;

                if (neededResourceValue + surplusPerBuilding < 0)
                {
                    building.CurrentProductivity = 0f;
                    building.IsDisabled = true;
                    DisabledBuildings.Push(new DisabledBuilding(building, ResourceType.Citizen));
                }
                else
                {
                    building.CurrentProductivity = (surplusPerBuilding + neededResourceValue) / surplusPerBuilding;
                }

                neededResourceValue += surplusPerBuilding;
                ChangedProductivityBuildings.Push(building);
                Debug.Log($"{building}'s new Productivity cause of workers: {building.CurrentProductivity}");
                if (neededResourceValue >= 0) return neededResourceValue;
            }

            return neededResourceValue;
        }
        
        /// <summary>
        /// increases productivity
        /// </summary>
        /// <param name="givenResourceValue">Value of jobless citizen</param>
        public void ChangeProductivityPositive(float givenResourceValue)
        { //TODO: (Robin) why it doesn't work correctly
            int surplus = 0;

            for (int i = 0; i < ChangedProductivityBuildings.Count; i++)
            {
                Building building = ChangedProductivityBuildings.Peek();
                for (int j = 0; j < building.BuildingResources.Consumption.Length; j++)
                {
                    Resource consumption = building.BuildingResources.Consumption[j];
                    if (consumption.resource == ResourceType.Citizen) surplus += consumption.value;
                }

                for (int j = 0; j < building.BuildingResources.Production.Length; j++)
                {
                    Resource production = building.BuildingResources.Production[j];
                    if (production.resource == ResourceType.Citizen) surplus -= production.value;
                }

                if (building.CurrentProductivity == 0f)
                {
                    if (surplus > 0 && surplus <= givenResourceValue)
                    {
                        givenResourceValue -= surplus;
                        building.CurrentProductivity = 1f;
                        ChangedProductivityBuildings.Pop().IsDisabled = false;

                        List<DisabledBuilding> disabledBuildingsList = new List<DisabledBuilding>();
                        int count = DisabledBuildings.Count;
                        for (int j = 0; j < count; j++)
                        {
                            disabledBuildingsList.Add(DisabledBuildings.Pop());
                        }

                        for (int j = 0; j < disabledBuildingsList.Count; j++)
                        {
                            DisabledBuilding disabledBuilding = disabledBuildingsList[j];
                            if (disabledBuilding.type == ResourceType.Citizen)
                            {
                                disabledBuildingsList.Remove(disabledBuilding);
                            }
                            else
                            {
                                DisabledBuildings.Push(disabledBuilding);
                            }
                        }

                        Debug.Log($"{building.BuildingType}'s new Productivity cause of workers: {building.CurrentProductivity}");
                    }
                    else
                    {
                        building.CurrentProductivity = givenResourceValue / surplus;
                        building.IsDisabled = false;
                        Debug.Log($"{building.BuildingType}'s new Productivity cause of workers: {building.CurrentProductivity}");
                        return;
                    }
                }
                else
                {
                    if (surplus > 0 && 1 - building.CurrentProductivity * surplus <= givenResourceValue)
                    {
                        givenResourceValue -= surplus;
                        ChangedProductivityBuildings.Pop().CurrentProductivity = 1f;
                        Debug.Log($"{building.BuildingType}'s new Productivity cause of workers: {building.CurrentProductivity}");
                    }
                    else
                    {
                        building.CurrentProductivity += givenResourceValue / surplus;
                        Debug.Log($"{building.BuildingType}'s new Productivity cause of workers: {building.CurrentProductivity}");
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Disables building, when there are no resources of one type left
        /// if there are no citizen left, it calls ChangeProductivityNegative()
        /// </summary>
        /// <param name="neededResourceValue">amount of needed resource (always negative)</param>
        /// <param name="type">type of needed resource</param>
        /// <param name="wasChanged">was priorityList changed</param>
        public void DisableBuildings(float neededResourceValue, ResourceType type, bool wasChanged)
        {
            for (int i = PriorityListItems.Count - 1; i > 0; i--)
            {
                int surplus = 0;
                BuildingType priorityBuildingType = GetBuildingTypeOnPriority(i);
                List<Building> priorityList = GetAllBuildingsOfType(priorityBuildingType);

                if (priorityList.Count <= 0) continue;

                // calculation of surplus
                for (int j = 0; j < priorityList[0].BuildingResources.Consumption.Length; j++)
                {
                    Resource consumption = priorityList[0].BuildingResources.Consumption[j];
                    if (consumption.resource != type) continue;

                    surplus += consumption.value;
                }

                for (int j = 0; j < priorityList[0].BuildingResources.Production.Length; j++)
                {
                    Resource production = priorityList[0].BuildingResources.Production[j];
                    if (production.resource != type) continue;

                    surplus -= production.value;
                }

                // DisableBuildings or change Productivity
                if (surplus <= 0)
                {
                    // Debug.Log("Überprüfe deine PrioListe, die unterste Priorität ändert den Zustand nicht!");
                }
                else
                {
                    if (type == ResourceType.Citizen)
                    {
                        neededResourceValue = ChangeProductivityNegative(surplus, neededResourceValue, priorityList);
                    }
                    else
                    {
                        for (int j = 0; j < priorityList.Count; j++)
                        {
                            Building building = priorityList[j];
                            if (building.IsDisabled) continue;

                            building.IsDisabled = true;
                            building.CurrentProductivity = 0;
                            if (!wasChanged)
                            {
                                Debug.Log($"{building.BuildingType} is disabled cause of {type}");
                            }

                            DisabledBuildings.Push(new DisabledBuilding(building, type));
                            neededResourceValue += surplus;

                            if (!(neededResourceValue >= 0)) continue;
                            for (int k = 0; k < PriorityListItems.Count; k++)
                            {
                                PriorityListItem item = PriorityListItems[k];
                                item.onChangePriorityUI.Invoke();
                                // Debug.Log("item.onChangePriorityUI.Invoke()");
                            }

                            return;
                        }
                    }
                }

                if (!(neededResourceValue >= 0)) continue;
                for (int j = 0; j < PriorityListItems.Count; j++)
                {
                    PriorityListItem item = PriorityListItems[j];
                    item.onChangePriorityUI.Invoke();
                    // Debug.Log("item.onChangePriorityUI.Invoke()");
                }

                return;

            }
        }

        /// <summary>
        /// Enables building, when there are resources of given type left
        /// </summary>
        /// <param name="givenResourceValue">amount of resource surplus</param>
        /// <param name="type">type of resource</param>
        public void EnableBuildings(float givenResourceValue, ResourceType type)
        {
            if (DisabledBuildings.Count == 0) return;

            int surplus = 0;
            bool somethingChanged = false;
            
            for (int i = 0; i < DisabledBuildings.Count; i++)
            {
                if (DisabledBuildings.Peek().type != type) return;

                Building building = DisabledBuildings.Peek().building;
                
                // Calculation of surplus
                for (int j = 0; j < building.BuildingResources.Consumption.Length; j++)
                {
                    Resource consumption = building.BuildingResources.Consumption[j];
                    if (consumption.resource != type) continue;

                    surplus += consumption.value;
                }

                for (int j = 0; j < building.BuildingResources.Production.Length; j++)
                {
                    Resource production = building.BuildingResources.Production[j];
                    if (production.resource != type) continue;

                    surplus -= production.value;
                }

                // Enables buildings
                if (surplus > 0 && surplus <= givenResourceValue)
                {
                    givenResourceValue -= surplus;
                    Debug.Log(DisabledBuildings.Peek().building.BuildingType + " is Enabled");
                    building.IsDisabled = false;
                    building.CurrentProductivity = 1;
                    DisabledBuildings.Pop();
                    somethingChanged = true;
                    
                    if (DisabledBuildings.Count != 0) continue;

                    for (int j = 0; j < PriorityListItems.Count; j++)
                    {
                        PriorityListItem item = PriorityListItems[j];
                        item.onChangePriorityUI.Invoke();
                        // Debug.Log("item.onChangePriorityUI.Invoke()");
                    }
                }
                else
                {
                    if (!somethingChanged) return;
                    //OnChangePriority();

                    for (int j = 0; j < PriorityListItems.Count; j++)
                    {
                        PriorityListItem item = PriorityListItems[j];
                        item.onChangePriorityUI.Invoke();
                        // Debug.Log("item.onChangePriorityUI.Invoke()");
                    }

                    return;
                }
            }
        }

        /// <summary>
        /// is called, if the priority was changed
        /// calls functions to change order of buildings in list
        /// </summary>
        public void OnChangePriority()
        {
            if (ChangedProductivityBuildings.Count != 0) ChangePriorityOfProductivityBuildings();
            if (DisabledBuildings.Count != 0) ChangePriorityOfDisabledBuildings();
        }

        /// <summary>
        /// changes the order of disabled buildings in list
        /// </summary>
        private void ChangePriorityOfDisabledBuildings()
        {
            DisabledBuilding[] buildings = new DisabledBuilding[DisabledBuildings.Count];
            int count = DisabledBuildings.Count;
            float citizenSurplus = 0;
            float energySurplus = 0;
            float materialSurplus = 0;

            // enable all
            for (int i = 0; i < count; i++)
            {
                DisabledBuilding disabledBuilding = DisabledBuildings.Pop();
                disabledBuilding.building.CurrentProductivity = 1;
                disabledBuilding.building.IsDisabled = false;

                for (int j = 0; j < disabledBuilding.building.BuildingResources.Consumption.Length; j++)
                {
                    Resource resource = disabledBuilding.building.BuildingResources.Consumption[j];
                    if (resource.resource != disabledBuilding.type) continue;

                    switch (resource.resource)
                    {
                        case ResourceType.Material:
                            materialSurplus -= resource.value;
                            break;
                        case ResourceType.Energy:
                            energySurplus -= resource.value;
                            break;
                        case ResourceType.Citizen:
                            citizenSurplus -= resource.value;
                            break;
                    }
                }

                for (int j = 0; j < disabledBuilding.building.BuildingResources.Production.Length; j++)
                {
                    Resource resource = disabledBuilding.building.BuildingResources.Production[j];
                    if (resource.resource != disabledBuilding.type) continue;
                    switch (resource.resource)
                    {
                        case ResourceType.Material:
                            materialSurplus += resource.value;
                            break;
                        case ResourceType.Energy:
                            energySurplus += resource.value;
                            break;
                        case ResourceType.Citizen:
                            citizenSurplus += resource.value;
                            break;
                    }
                }

                buildings[i] = disabledBuilding;
            }
            
            // disable all

            DisableBuildings(materialSurplus, ResourceType.Material, true);
            DisableBuildings(energySurplus, ResourceType.Energy, true);
            DisableBuildings(citizenSurplus, ResourceType.Citizen, true);

            int citizenSaveCount = 0;
            int energyGainCount = 0;
            int lifeSupportGainCount = 0;
            int materialGainCount = 0;
            int energySaveCount = 0;
            int lifeSupportSaveCount = 0;
            int materialSaveCount = 0;
            for (int i = 0; i < buildings.Length; i++)
            {
                DisabledBuilding building = buildings[i];
                switch (building.building.BuildingType)
                {
                    case BuildingType.CitizenSave:
                        citizenSaveCount++;
                        break;
                    case BuildingType.EnergyGain:
                        energyGainCount++;
                        break;
                    case BuildingType.LifeSupportGain:
                        lifeSupportGainCount++;
                        break;
                    case BuildingType.MaterialGain:
                        materialGainCount++;
                        break;
                    case BuildingType.EnergySave:
                        energySaveCount++;
                        break;
                    case BuildingType.LifeSupportSave:
                        lifeSupportSaveCount++;
                        break;
                    case BuildingType.MaterialSave:
                        materialSaveCount++;
                        break;
                }
            }

            foreach (DisabledBuilding disabledBuilding in DisabledBuildings)
            {
                switch (disabledBuilding.building.BuildingType)
                {
                    case BuildingType.CitizenSave:
                        citizenSaveCount--;
                        break;
                    case BuildingType.EnergyGain:
                        energyGainCount--;
                        break;
                    case BuildingType.LifeSupportGain:
                        lifeSupportGainCount--;
                        break;
                    case BuildingType.MaterialGain:
                        materialGainCount--;
                        break;
                    case BuildingType.EnergySave:
                        energySaveCount--;
                        break;
                    case BuildingType.LifeSupportSave:
                        lifeSupportSaveCount--;
                        break;
                    case BuildingType.MaterialSave:
                        materialSaveCount--;
                        break;
                }
            }

            if (citizenSaveCount > 0)
                for (int i = 0; i < citizenSaveCount; i++)
                {
                    Debug.Log(BuildingType.CitizenSave + " is Enabled");
                }
            else if (citizenSaveCount < 0)
                for (int i = citizenSaveCount; i < 0; i++)
                {
                    Debug.Log(BuildingType.CitizenSave + " is Disabled");
                }

            if (energyGainCount > 0)
            {
                for (int i = 0; i < energyGainCount; i++)
                {
                    Debug.Log(BuildingType.EnergyGain + " is Enabled");
                }
            }
            else if (energyGainCount < 0)
            {
                for (int i = energyGainCount; i < 0; i++)
                {
                    Debug.Log(BuildingType.EnergyGain + " is Disabled");
                }
            }
            
            if (lifeSupportGainCount > 0)
            {
                for (int i = 0; i < lifeSupportGainCount; i++)
                {
                    Debug.Log(BuildingType.LifeSupportGain + " is Enabled");
                }
            }
            else if (lifeSupportGainCount < 0)
            {
                for (int i = lifeSupportGainCount; i < 0; i++)
                {
                    Debug.Log(BuildingType.LifeSupportGain + " is Disabled");
                }
            }
            
            if (materialGainCount > 0)
            {
                for (int i = 0; i < materialGainCount; i++)
                {
                    Debug.Log(BuildingType.MaterialGain + " is Enabled");
                }
            }
            else if (materialGainCount < 0)
            {
                for (int i = materialGainCount; i < 0; i++)
                {
                    Debug.Log(BuildingType.MaterialGain + " is Disabled");
                }
            }
            
            if (energySaveCount > 0)
            {
                for (int i = 0; i < energySaveCount; i++)
                {
                    Debug.Log(BuildingType.EnergySave + " is Enabled");
                }
            }
            else if (energySaveCount < 0)
            {
                for (int i = energySaveCount; i < 0; i++)
                {
                    Debug.Log(BuildingType.EnergySave + " is Disabled");
                }
            }
            
            if (lifeSupportSaveCount > 0)
            {
                for (int i = 0; i < lifeSupportSaveCount; i++)
                {
                    Debug.Log(BuildingType.LifeSupportSave + " is Enabled");
                }
            }
            else if (lifeSupportSaveCount < 0)
            {
                for (int i = lifeSupportSaveCount; i < 0; i++)
                {
                    Debug.Log(BuildingType.LifeSupportSave + " is Disabled");
                }
            }
            
            if (materialSaveCount > 0)
            {
                for (int i = 0; i < materialSaveCount; i++)
                {
                    Debug.Log(BuildingType.MaterialSave + " is Enabled");
                }
            }
            else if (materialSaveCount < 0)
            {
                for (int i = materialSaveCount; i < 0; i++)
                {
                    Debug.Log(BuildingType.MaterialSave + " is Disabled");
                }
            }

            //                                                                                              OLD
            // for (int i = 0; i < count; i++)
            // {
            //     buildings[i] = DisabledBuildings.Pop();
            // }
            //
            // for (int i = PriorityListItems.Count - 1; i > 0; i--)
            // {
            //     BuildingTypes type = GetBuildingTypeOnPriority(i);
            
            //     foreach (DisabledBuilding item in buildings)
            //     {
            //         if (type != item.building.BuildingType) continue;
            //     
            //         DisabledBuildings.Push(item);
            //     }
            //
            //     if (DisabledBuildings.Count == buildings.Length) return;
            // }
        }
    
        /// <summary>
        /// changes the order of buildings which has a changed priority in list
        /// </summary>
        private void ChangePriorityOfProductivityBuildings()
        {
            float surplus = 0f;
            int count = ChangedProductivityBuildings.Count;
            for (int i = 0; i < count; i++)
            {
                Building building = ChangedProductivityBuildings.Pop();
                for (int j = 0; j < building.BuildingResources.Consumption.Length; j++)
                {
                    Resource resource = building.BuildingResources.Consumption[j];
                    if (resource.resource != ResourceType.Citizen) continue;

                    surplus += resource.value - building.CurrentProductivity * resource.value;
                }
            }
            
            DisableBuildings(surplus, ResourceType.Citizen, true);
            
            //     Building[] priorityBuildings = new Building[ChangedProductivityBuildings.Count];
            //     int count = ChangedProductivityBuildings.Count;
            //     for (int i = 0; i < count; i++)
            //     {
            //         priorityBuildings[i] = ChangedProductivityBuildings.Pop();
            //     }
            //     for (int i = PriorityListItems.Count - 1; i > 0; i--)
            //     {
            //         BuildingTypes type = GetBuildingTypeOnPriority(i);
            //         foreach (Building item in priorityBuildings)
            //         {
            //             if (type == item.BuildingType)
            //             {
            //                 ChangedProductivityBuildings.Push(item);
            //             }
            //         }
            //     
            //         if (ChangedProductivityBuildings.Count == priorityBuildings.Length)
            //         {
            //             return;
            //         }
            //     }

            for (int i = 0; i < PriorityListItems.Count; i++)
            {
                PriorityListItem item = PriorityListItems[i];
                item.onChangePriorityUI.Invoke();
                Debug.Log("item.onChangePriorityUI.Invoke()");
            }
        }

        #endregion
    }
}