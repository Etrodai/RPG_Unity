using System.Collections.Generic;
using Buildings;
using PriorityListSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    private static GameManager instance;
    [SerializeField] private List<Building> allBuildings = new List<Building>();
    [SerializeField] private PriorityListMenu[] priorityListItems;
    private List<Building> disabledBuildings = new List<Building>();
    
    #endregion

    #region Properties

    public static GameManager Instance
    {
        get => instance;
        set => instance = value;
    }

    public List<Building> AllBuildings => allBuildings;
    
    public PriorityListMenu[] PriorityListItems => priorityListItems;

    public List<Building> DisabledBuildings
    {
        get => disabledBuildings;
        set => disabledBuildings = value;
    }

    #endregion

    #region UnityEvents

    /// <summary>
    /// singleton
    /// </summary>
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Searches for an empty slot in allBuildingsList
    /// </summary>
    /// <returns>Index of first empty in allBuildings</returns>
    public int GetIndexOfFirstEmpty()
    {
        return allBuildings.IndexOf(null);
    }
    
    /// <summary>
    /// Searches the Priority of the given BuildingType
    /// </summary>
    /// <param name="type">BuildingType</param>
    /// <returns>Priority of the given BuildingType</returns>
    public int GetPriority(BuildingTypes type)
    {
        int priority = 0;
        foreach (PriorityListMenu item in priorityListItems)
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
    public BuildingTypes GetBuildingTypeOnPriority(int priority)
    {
        BuildingTypes type = BuildingTypes.All;
        foreach (PriorityListMenu item in priorityListItems)
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
            foreach (Building item in allBuildings)
            {
                if (item != null)
                {
                    count++;
                }
            }
    
            return count;
        }
    
        foreach (Building item in allBuildings)
        {
            if (item.BuildingType == type) count++;
        }
    
        return count;
    }

    public List<Building> GetAllBuildingsOfType(BuildingTypes type)
    {
        List<Building> buildings = new List<Building>();
        foreach (Building item in allBuildings)
        {
            if (item.BuildingType == type)
            {
                buildings.Add(item);
            }
        }

        return buildings;
    }

    #endregion
}