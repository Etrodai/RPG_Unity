using ResourceManagement.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManagerSingleton : MonoBehaviour
{
    private static MainManagerSingleton instance;
    public static MainManagerSingleton Instance
    {
        get => instance;
    }

    //All other managers
    private CitizenManager citizenManager;
    public CitizenManager CitizenManager { get => citizenManager; }

    private EnergyManager energyManager;
    public EnergyManager EnergyManager { get => energyManager; }

    private FoodManager foodManager;
    public FoodManager FoodManager { get => foodManager; }

    private WaterManager waterManager;
    public WaterManager WaterManager { get => waterManager; }

    private MaterialManager materialManager;
    public MaterialManager MaterialManager { get => materialManager; }

    private GameManager gameManager;
    public GameManager GameManager { get => gameManager; }

    private EventManagerScriptable eventManager;
    public EventManagerScriptable EventManager { get => eventManager; }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        citizenManager = gameObject.GetComponent<CitizenManager>();
        energyManager = gameObject.GetComponent<EnergyManager>();
        foodManager = gameObject.GetComponent<FoodManager>();
        waterManager = gameObject.GetComponent<WaterManager>();
        materialManager = gameObject.GetComponent<MaterialManager>();
        gameManager = gameObject.GetComponent<GameManager>();
        eventManager = gameObject.GetComponent<EventManagerScriptable>();
    }
}
