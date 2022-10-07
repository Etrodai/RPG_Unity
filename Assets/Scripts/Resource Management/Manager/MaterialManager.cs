using TMPro;
using UnityEngine;

public class MaterialManager : ResourceManager
{
    #region TODOS
    // what happens, when there are no Materials left?
    // bad code in Calculation ( * 20) // Cause of growTime??
    #endregion
    
    #region Variables
    private static MaterialManager instance;
    [SerializeField] private TextMeshProUGUI savedResourceText;
    [SerializeField] private TextMeshProUGUI surplusText;
    #endregion

    #region Properties
    public static MaterialManager Instance
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
    /// Starts Calculation
    /// </summary>
    private void Start()
    {
        InvokeRepeating(nameof(InvokeCalculation), 0f, 0.5f);
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
