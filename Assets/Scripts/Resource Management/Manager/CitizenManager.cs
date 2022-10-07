using TMPro;
using UnityEngine;

public class CitizenManager : MonoBehaviour
{
    #region TODOS
    // Citizen werden abgezogen, wenn sie arbeiten
    // JoblessCitizen/Working Citizen fehlt
    // Weniger Citizen, wenn Surplus negativ, oder wenn kein SavedResource mehr??
    #endregion
    
    #region Variables
    private static CitizenManager instance;
    private int citizen;
    [SerializeField] private TextMeshProUGUI savedResourceText;
    [SerializeField] private TextMeshProUGUI surplusText;
    [SerializeField] private int growTime;
    [SerializeField] private float waterConsumptionPerCitizen;
    [SerializeField] private float foodConsumptionPerCitizen;
    private FoodManager foodManager;
    private WaterManager waterManager;
    private int housing;
    
    // private int joblessCitizen;                          TODO
    // public int JoblessCitizen
    // {
    //     get => joblessCitizen;
    //     set => joblessCitizen = value;
    // }
    #endregion
    
    #region Properties
    public static CitizenManager Instance
    {
        get => instance;
        set => instance = value;
    }
    public int Citizen => citizen;
    public int Housing { get => housing ; set => housing = value; }
    #endregion
    
    #region UnityEvents
    /// <summary>
    /// Singleton
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
    
    /// <summary>
    /// sets variables
    /// starts growing
    /// sets food and water demand
    /// </summary>
    void Start()
    {
        foodManager = FoodManager.Instance;
        waterManager = WaterManager.Instance;
        InvokeRepeating(nameof(Growth), 0, growTime);
        foodManager.CurrentResourceDemand = citizen * foodConsumptionPerCitizen;
        waterManager.CurrentResourceDemand = citizen * waterConsumptionPerCitizen;
    }
    #endregion
    
    #region Methods
    /// <summary>
    /// reduces citizen, if there are too few housings, food or water
    /// adds citizen, if there are more than needed housings, food and water
    /// has no surplus, if there are nothing is too few or not all variables are more than needed
    /// </summary>
    private void Growth()
    {
        if (housing < citizen || foodManager.CurrentResourceSurplus < 0 || waterManager.CurrentResourceSurplus < 0)
        {
            ChangeCitizen(-1);
        }
        else if (housing > citizen && foodManager.CurrentResourceSurplus > 0 && waterManager.CurrentResourceSurplus > 0)
        {
            ChangeCitizen(1);
        }
        else
        {
            surplusText.text = $"0";
        }

        savedResourceText.text = $"{citizen}/{Housing}";
    }

    /// <summary>
    /// adds or reduces citizen
    /// changes food and water demand
    /// </summary>
    /// <param name="value">value of citizens to be added or reduced </param>
    private void ChangeCitizen(int value)
    {
        citizen += value;
        foodManager.CurrentResourceDemand += foodConsumptionPerCitizen * value;
        waterManager.CurrentResourceDemand += waterConsumptionPerCitizen * value;
        surplusText.text = $"{value}";
    }
    #endregion
}
