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
        public int GetPriority(BuildingTypes type)
        {
            int priority = 0;
            foreach (PriorityListItem item in PriorityListItems)
            {
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
        private BuildingTypes GetBuildingTypeOnPriority(int priority)
        {
            BuildingTypes type = BuildingTypes.All;
            foreach (PriorityListItem item in PriorityListItems)
            {
                if (item.Priority == priority)
                {
                    type = item.Type;
                }
            }
    
            return type;
        }

        /// <summary>
        /// Gets count of one buildingType
        /// </summary>
        /// <param name="type">BuildingType</param>
        /// <returns>value of this buildingType</returns>
        public int GetBuildingCount(BuildingTypes type)
        {
            int count = 0;
            if (type == BuildingTypes.All)
            {
                foreach (Building building in AllBuildings)
                {
                    if (building != nullBuilding)
                    {
                        count++;
                    }
                }
    
                return count;
            }
    
            foreach (Building building in AllBuildings)
            {
                if (building.BuildingType == type) count++;
            }
    
            return count;
        }
        
        /// <summary>
        /// Gets count of all working buildings
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetWorkingBuildingCount(BuildingTypes type)
        {
            int count = GetBuildingCount(type);
            foreach (DisabledBuilding disabledBuilding in DisabledBuildings)
            {
                if (disabledBuilding.building.BuildingType == type)
                {
                    count--;
                }
            }
            
            return count;
        }

        /// <summary>
        /// Gets all buildings of one type
        /// </summary>
        /// <param name="type">BuildingType</param>
        /// <returns>All buildings of given type</returns>
        private List<Building> GetAllBuildingsOfType(BuildingTypes type)
        {
            List<Building> buildings = new List<Building>();
            foreach (Building building in AllBuildings)
            {
                if (building.BuildingType == type)
                {
                    buildings.Add(building);
                }
            }

            return buildings;
        }
        
        /// <summary>
        /// reduces productivity
        /// </summary>
        /// <param name="surplus">value of consumption - production per building (always positive)</param>
        /// <param name="neededResourceValue">amount of needed citizen (always negative)</param>
        /// <param name="priorityList">List of buildings of current priority</param>
        /// <returns>amount of still needed citizen</returns>
        private float ChangeProductivityNegative(float surplus, float neededResourceValue, List<Building> priorityList)
        { //TODO: (Robin) why it doesn't work correctly
            foreach (Building building in priorityList)
            {
                if (building.IsDisabled || building.CurrentProductivity == 0) continue;

                if (neededResourceValue + surplus < 0)
                {
                    building.CurrentProductivity = 0f;
                    DisabledBuildings.Push(new DisabledBuilding(building, ResourceTypes.Citizen));
                }
                else building.CurrentProductivity = (surplus + neededResourceValue) / surplus;
                
                neededResourceValue += surplus;
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
                foreach (Resource consumption in building.BuildingResources.Consumption)
                {
                    if (consumption.resource == ResourceTypes.Citizen)
                    {
                        surplus += consumption.value;
                    }
                }

                foreach (Resource production in building.BuildingResources.Production)
                {
                    if (production.resource == ResourceTypes.Citizen)
                    {
                        surplus -= production.value;
                    }
                }

                if (building.CurrentProductivity == 0f)
                {
                    if (surplus > 0 && surplus <= givenResourceValue)
                    {
                        givenResourceValue -= surplus;
                        ChangedProductivityBuildings.Pop().CurrentProductivity = 1f;

                        List<DisabledBuilding> disabledBuildingsList = new List<DisabledBuilding>();
                        int count = DisabledBuildings.Count;
                        for (int j = 0; j < count; j++)
                        {
                            disabledBuildingsList.Add(DisabledBuildings.Pop());
                        }

                        foreach (DisabledBuilding disabledBuilding in disabledBuildingsList)
                        {
                            if (disabledBuilding.type == ResourceTypes.Citizen)
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
        public void DisableBuildings(float neededResourceValue, ResourceTypes type)
        {
            for (int i = PriorityListItems.Count - 1; i > 0; i--)
            {
                int surplus = 0;
                BuildingTypes priorityBuildingType = GetBuildingTypeOnPriority(i);
                List<Building> priorityList = GetAllBuildingsOfType(priorityBuildingType);

                if (priorityList.Count <= 0) continue;
                
                foreach (Resource consumption in priorityList[0].BuildingResources.Consumption)
                {
                    if (consumption.resource != type) continue;
                    
                    surplus += consumption.value;
                }

                foreach (Resource production in priorityList[0].BuildingResources.Production)
                {
                    if (production.resource != type) continue;
                    
                    surplus -= production.value;
                }
                
                if (surplus <= 0)
                {
                    Debug.Log("Überprüfe deine PrioListe, die unterste Priorität ändert den Zustand nicht!");
                }
                else
                {
                    if (type == ResourceTypes.Citizen)
                    {
                        neededResourceValue = ChangeProductivityNegative(surplus, neededResourceValue, priorityList);
                    }
                    else
                    {
                        foreach (Building building in priorityList)
                        {
                            if (!building.IsDisabled)
                            {
                                building.IsDisabled = true;
                                Debug.Log($"{building.BuildingType} is disabled cause of {type}");
                            
                                DisabledBuildings.Push(new DisabledBuilding(building, type));
                                neededResourceValue += surplus;
                                if (neededResourceValue >= 0)
                                {
                                    foreach (PriorityListItem item in PriorityListItems)
                                    {
                                        item.onChangePriorityUI.Invoke();
                                        Debug.Log("item.onChangePriorityUI.Invoke()");
                                    }
                                    return;
                                }
                            }
                        }
                    }
                }

                if (neededResourceValue >= 0)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Enables building, when there are resources of given type left
        /// </summary>
        /// <param name="givenResourceValue">amount of resource surplus</param>
        /// <param name="type">type of resource</param>
        public void EnableBuildings(float givenResourceValue, ResourceTypes type)
        {
            if (DisabledBuildings.Count == 0) return;

            int surplus = 0;
            bool somethingChanged = false;
            
            for (int i = 0; i < DisabledBuildings.Count; i++)
            {
                if (DisabledBuildings.Peek().type != type) return;

                Building building = DisabledBuildings.Peek().building;
                foreach (Resource consumption in building.BuildingResources.Consumption)
                {
                    if (consumption.resource != type) continue;
                    
                    surplus += consumption.value;
                }

                foreach (Resource production in building.BuildingResources.Production)
                {
                    if (production.resource != type) continue; 
                    
                    surplus -= production.value;
                }
                
                if (surplus > 0 && surplus <= givenResourceValue)
                {
                    givenResourceValue -= surplus;
                    Debug.Log(DisabledBuildings.Peek().building.BuildingType + " is Enabled");
                    DisabledBuildings.Pop().building.IsDisabled = false;
                    somethingChanged = true;
                    
                    if (DisabledBuildings.Count != 0) continue;
                    
                    foreach (PriorityListItem item in PriorityListItems)
                    {
                        item.onChangePriorityUI.Invoke();
                        Debug.Log("item.onChangePriorityUI.Invoke()");
                    }
                }
                else
                {
                    if (!somethingChanged) return;
                    
                    foreach (PriorityListItem item in PriorityListItems)
                    {
                        item.onChangePriorityUI.Invoke();
                        Debug.Log("item.onChangePriorityUI.Invoke()");
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
            for (int i = 0; i < count; i++)
            {
                buildings[i] = DisabledBuildings.Pop();
            }
        
            for (int i = PriorityListItems.Count - 1; i > 0; i--)
            {
                BuildingTypes type = GetBuildingTypeOnPriority(i);
                foreach (DisabledBuilding item in buildings)
                {
                    if (type != item.building.BuildingType) continue;
                    
                    DisabledBuildings.Push(item);
                }
        
                if (DisabledBuildings.Count == buildings.Length) return;
            }
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
                foreach (Resource resource in building.BuildingResources.Consumption)
                {
                    if (resource.resource != ResourceTypes.Citizen) continue;
                    
                    surplus += resource.value - building.CurrentProductivity * resource.value;
                }
            }
            
            DisableBuildings(surplus, ResourceTypes.Citizen);
            
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
        }

        #endregion
    }
}