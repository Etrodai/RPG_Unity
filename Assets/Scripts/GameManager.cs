using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get => instance;
        set => instance = value;
    }

    [SerializeField] private List<BuildingTypes> allBuildings = new List<BuildingTypes>();
    public List<BuildingTypes> AllBuildings => allBuildings;

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

    public int GetBuildingCount(BuildingTypes type)
    {
        int count = 0;
        foreach (BuildingTypes item in allBuildings)
        {
            if (item == type)
            {
                count++;
            }
        }

        return count;
    }
}
