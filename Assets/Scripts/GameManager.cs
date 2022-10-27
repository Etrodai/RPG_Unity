using System.Collections.Generic;
using Buildings;
using PriorityListSystem;
using ResourceManagement;
using UnityEngine;

public struct DisabledBuilding
{
    public Building building;
    public ResourceTypes type;

    public DisabledBuilding(Building building, ResourceTypes type)
    {
        this.building = building;
        this.type = type;
    }
}

public class GameManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private List<Building> allBuildings = new List<Building>();
    [SerializeField] private PriorityListItem[] priorityListItems;
    [SerializeField] private Stack<DisabledBuilding> disabledBuildings = new Stack<DisabledBuilding>();
    [SerializeField] private Building nullBuilding;


    #endregion

    #region Properties

    public List<Building> AllBuildings => allBuildings;
    
    public PriorityListItem[] PriorityListItems => priorityListItems;

    public Stack<DisabledBuilding> DisabledBuildings
    {
        get => disabledBuildings;
        set => disabledBuildings = value;
    }

    public Building NullBuilding => nullBuilding;

    #endregion

    #region UnityEvents

    /// <summary>
    /// singleton
    /// </summary>
    private void Awake()
    {
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
        return allBuildings.IndexOf(nullBuilding);
    }
    
    /// <summary>
    /// Searches the Priority of the given BuildingType
    /// </summary>
    /// <param name="type">BuildingType</param>
    /// <returns>Priority of the given BuildingType</returns>
    public int GetPriority(BuildingTypes type)
    {
        int priority = 0;
        foreach (PriorityListItem item in priorityListItems)
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
        foreach (PriorityListItem item in priorityListItems)
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
            foreach (Building building in allBuildings)
            {
                if (building != nullBuilding)
                {
                    count++;
                }
            }
    
            return count;
        }
    
        foreach (Building building in allBuildings)
        {
            if (building.BuildingType == type) count++;
        }
    
        return count;
    }

    private List<Building> GetAllBuildingsOfType(BuildingTypes type)
    {
        List<Building> buildings = new List<Building>();
        foreach (Building building in allBuildings)
        {
            if (building.BuildingType == type)
            {
                buildings.Add(building);
            }
        }

        return buildings;
    }

    public void DisableBuildings(float neededResourceValue, ResourceTypes type)
    {
        for (int i = priorityListItems.Length - 1; i > 0; i--)
        {
            int surplus = 0;
            BuildingTypes priorityBuildingType = GetBuildingTypeOnPriority(i);
            List<Building> priorityList = GetAllBuildingsOfType(priorityBuildingType);

            if (priorityList.Count > 0)
            {
                foreach (Resource consumption in priorityList[0].BuildingResources.Consumption)
                {
                    if (consumption.resource == type)
                    {
                        surplus += consumption.value;
                    }
                }

                foreach (Resource production in priorityList[0].BuildingResources.Production)
                {
                    if (production.resource == type)
                    {
                        surplus -= production.value;
                        return;
                    }
                }
                
                if (surplus <= 0)
                {
                    Debug.Log("Überprüfe deine PrioListe, die unterste Priorität ändert den Zustand nicht!");
                }
                else
                {
                    foreach (Building building in priorityList)
                    {
                        if (!building.IsDisabled)
                        {
                            building.IsDisabled = true;
                            Debug.Log($"{building.BuildingType} is disabled cause of {type}");
                            
                            disabledBuildings.Push(new DisabledBuilding(building, type));
                            neededResourceValue += surplus;
                            if (neededResourceValue >= 0)
                            {
                                return;
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
    }

    public void EnableBuildings(float givenResourceValue, ResourceTypes type)
    {
        if (disabledBuildings.Count == 0)
        {
            return;
        }

        int surplus = 0;

        for (int i = 0; i < disabledBuildings.Count; i++)
        {
            if (disabledBuildings.Peek().type != type)
            {
                return;
            }
            
            Building building = disabledBuildings.Peek().building;
            foreach (Resource consumption in building.BuildingResources.Consumption)
            {
                if (consumption.resource == type)
                {
                    surplus += consumption.value;
                }
            }

            foreach (Resource production in building.BuildingResources.Production)
            {
                if (production.resource == type)
                {
                    surplus -= production.value;
                }
            }

            if (surplus > 0 && surplus <= givenResourceValue)
            {
                givenResourceValue -= surplus;
                Debug.Log(disabledBuildings.Peek().building.BuildingType + " is Enabled");
                disabledBuildings.Pop().building.IsDisabled = false;
            }
            else
            {
                return;
            }
        }
    }

    public void OnChangePriority()
    {
        if (disabledBuildings.Count == 0)
        {
            return;
        }

        DisabledBuilding[] buildings = new DisabledBuilding[disabledBuildings.Count];
        int count = disabledBuildings.Count;
        for (int i = 0; i < count; i++)
        {
            buildings[i] = disabledBuildings.Pop();
        }
        
        for (int i = priorityListItems.Length - 1; i > 0; i--)
        {
            BuildingTypes type = GetBuildingTypeOnPriority(i);
            foreach (DisabledBuilding item in buildings)
            {
                if (type == item.building.BuildingType)
                {
                    disabledBuildings.Push(item);
                }
            }

            if (disabledBuildings.Count == buildings.Length)
            {
                return;
            }
        }
    }

    #endregion
}