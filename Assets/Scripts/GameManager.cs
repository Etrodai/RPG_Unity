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

    private static GameManager instance;
    [SerializeField] private List<Building> allBuildings = new List<Building>();
    [SerializeField] private PriorityListItem[] priorityListItems;
    [SerializeField] private Stack<DisabledBuilding> disabledBuildings = new Stack<DisabledBuilding>();
    [SerializeField] private Building nullBuilding;


    #endregion

    #region Properties

    public static GameManager Instance
    {
        get => instance;
        set => instance = value;
    }

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
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
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
    public BuildingTypes GetBuildingTypeOnPriority(int priority)
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
            foreach (Building item in allBuildings)
            {
                if (item != nullBuilding)
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

    public void DisableBuildings(float neededResourceValue, ResourceTypes type)
    {
        for (int i = priorityListItems.Length - 1; i > 0; i--)
        {
            int surplus = 0;
            BuildingTypes priorityBuildingType = GetBuildingTypeOnPriority(i);
            List<Building> priorityList = GetAllBuildingsOfType(priorityBuildingType);

            if (priorityList.Count > 0)
            {
                foreach (Resource item in priorityList[0].BuildingResources.Consumption)
                {
                    if (item.resource == type)
                    {
                        surplus += item.value;
                    }
                }

                foreach (Resource item in priorityList[0].BuildingResources.Production)
                {
                    if (item.resource == type)
                    {
                        surplus -= item.value;
                        return;
                    }
                }
                
                if (surplus <= 0)
                {
                    Debug.Log("Überprüfe deine PrioListe, die unterste Priorität ändert den Zustand nicht!");
                }
                else
                {
                    foreach (Building item in priorityList)
                    {
                        if (!item.IsDisabled)
                        {
                            item.IsDisabled = true;
                            
                            
                            disabledBuildings.Push(new DisabledBuilding(item, type));
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
            foreach (Resource item in building.BuildingResources.Consumption)
            {
                if (item.resource == type)
                {
                    surplus += item.value;
                }
            }

            foreach (Resource item in building.BuildingResources.Production)
            {
                if (item.resource == type)
                {
                    surplus -= item.value;
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