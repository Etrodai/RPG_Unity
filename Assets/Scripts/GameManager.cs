using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region TODOS

    // GetAllBuildingsCount in GetBuildingsCount integrieren
    // GetEmptyBuildingsIndex hinzufügen

    #endregion
    
    #region Varables
    private static GameManager instance;
    [SerializeField] private List<BuildingTypes> allBuildings = new List<BuildingTypes>();
    #endregion

    #region Properties
    public static GameManager Instance
    {
        get => instance;
        set => instance = value;
    }
    public List<BuildingTypes> AllBuildings => allBuildings;
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
    /// Gets count of all buildings
    /// </summary>
    /// <returns>value of all buildings</returns>
    public int GetAllBuildingsCount()
    {
        int count = 0;
        foreach (BuildingTypes item in allBuildings)
        {
            if (item != BuildingTypes.Empty)
            {
                count++;
            }
        }
    
        return count;
    }
    
    /// <summary>
    /// Gets count of one buildingType
    /// </summary>
    /// <param name="type">BuildingType</param>
    /// <returns>value of this buildingType</returns>
    public int GetBuildingCount(BuildingTypes type)
    {
        int count = 0;
        foreach (BuildingTypes item in allBuildings)
        {
            if (item == type) count++;
        }

        return count;
    }
    #endregion
}
