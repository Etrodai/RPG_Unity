using System.Collections.Generic;
using Buildings;
using PriorityListSystem;
using ResourceManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    private static GameManager instance;
    [SerializeField] private List<Building> allBuildings = new List<Building>();
    [SerializeField] private PriorityListItem[] priorityListItems;
    private Stack<Building> disabledBuildings = new Stack<Building>();
    [SerializeField] private Building NULLBuilding;

    #endregion

    #region Properties

    public static GameManager Instance
    {
        get => instance;
        set => instance = value;
    }

    public List<Building> AllBuildings => allBuildings;
    
    public PriorityListItem[] PriorityListItems => priorityListItems;

    public Stack<Building> DisabledBuildings
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

        Instantiate(NULLBuilding);
    }

    private void Start()
    {
        NULLBuilding = GameObject.FindGameObjectWithTag("NULLBuilding").GetComponent<Building>();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Searches for an empty slot in allBuildingsList
    /// </summary>
    /// <returns>Index of first empty in allBuildings</returns>
    public int GetIndexOfFirstEmpty()
    {
        return allBuildings.IndexOf(NULLBuilding);
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
                if (item != NULLBuilding)
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
        List<Building> priorityList = new List<Building>();
        BuildingTypes priorityBuildingType;

        for (int i = priorityListItems.Length - 1; i > 0; i--)
        {
            int surplus = 0;
            priorityBuildingType = GetBuildingTypeOnPriority(i);
            priorityList = GetAllBuildingsOfType(priorityBuildingType);

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
                            disabledBuildings.Push(item);
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
            Building building = disabledBuildings.Peek();
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

            if (surplus <= givenResourceValue && surplus > 0)
            {
                givenResourceValue -= surplus;
                Debug.Log(disabledBuildings.Peek() + " is Enabled");
                disabledBuildings.Pop().IsDisabled = false;
            }
            else
            {
                return;
            }
        }
    }

    #endregion
}