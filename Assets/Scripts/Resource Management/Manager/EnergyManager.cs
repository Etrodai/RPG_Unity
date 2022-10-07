using TMPro;
using UnityEngine;

public class EnergyManager : ResourceManager
{
    #region TODOS
    // what happens, when there are no Energy left?
    // bad code in Calculation ( * 20) // Cause of growTime??
    #endregion

    #region Variables
    private static EnergyManager instance;
    [SerializeField] private TextMeshProUGUI savedResourceText;
    [SerializeField] private TextMeshProUGUI surplusText;
    #endregion

    #region Properties
    public static EnergyManager Instance
    {
        get => instance;
        set { instance = value; }
    }
    public override float CurrentResourceSurplus { get; set; }
    public override float CurrentResourceProduction { get; set; }
    public override float CurrentResourceDemand { get; set; }
    public override float SavedResourceValue { get; set; }
    public override float SaveSpace { get; set; }
    #endregion
    
    #region OldCode
    // private float currentEnergyValue; //Calculated Energy production
    // public float CurrentEnergyValue
    // {
    //     get => currentEnergyValue;
    //     set { currentEnergyValue = value; }
    // }

    // private float currentEnergyDemand; //Energy consumption of every module combined
    // public float CurrentEnergyDemand
    // {
    //     get => currentEnergyDemand;
    //     set { currentEnergyDemand = value; }
    // }
    //
    // private float currentEnergyProduction; //Energy production of every Energy source combined
    // public float CurrentEnergyProduction
    // {
    //     get => currentEnergyProduction;
    //     set { currentEnergyProduction = value; }
    // }
    //
    // private float savedEnergyValue; //No use for now
    #endregion

    #region UnityEvents
    /// <summary>
    /// Singleton
    /// </summary>
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    /// <summary>
    /// Starts Calculation
    /// </summary>
    private void Start()
    {
        InvokeRepeating(nameof(InvokeCalculation), 0, 0.5f);
    }
    #endregion
    
    #region Methods
    /// <summary>
    /// calls the Calculations
    /// </summary>
    protected override void InvokeCalculation()
    {
        CalculateCurrentResourceSurplus();
        CalculateSavedResourceValue();
    }

    #region OldSummary
    /// <summary>
    /// Calculation of currentEnergyValue
    /// </summary>
    /// <param name="currentProduction">Combined value of all energy production sources</param>
    /// <param name="savedValue">Combined value of all (when needed) active saved energy sources like batteries</param>
    /// <param name="currentDemand">Combined value of all energy consuming sources like modules</param>
    #endregion
    
    /// <summary>
    /// Calculation of currentMaterialSurplus
    /// </summary>
    protected override void CalculateCurrentResourceSurplus()
    {
        CurrentResourceSurplus = CurrentResourceProduction - CurrentResourceDemand;
        surplusText.text = $"{CurrentResourceSurplus}";
    }

    /// <summary>
    /// Calculation of SavedMaterialValue every 0.5 seconds
    /// </summary>
    protected override void CalculateSavedResourceValue()
    {
        if (SaveSpace > SavedResourceValue + CurrentResourceSurplus/20)
        {
            SavedResourceValue += CurrentResourceSurplus/20;
        }
        else
        {
            SavedResourceValue = SaveSpace;
        }
        
        if (SavedResourceValue < 0)
        {
            SavedResourceValue = 0;
            // Disable Modules
        }
        
        savedResourceText.text = $"{(int)SavedResourceValue}/{SaveSpace}";
    }
    #endregion
}
