using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaterManager : ResourceManager
{
    private static WaterManager instance; //Singleton
    public static WaterManager Instance
    {
        get => instance;
        set { instance = value; }
    }

    [SerializeField] private TextMeshProUGUI savedResourceText;
    [SerializeField] private TextMeshProUGUI surplusText;
    
    public override float CurrentResourceSurplus { get; set; }
    public override float CurrentResourceProduction { get; set; }
    public override float CurrentResourceDemand { get; set; }
    public override float SavedResourceValue { get; set; }
    public override float SaveSpace { get; set; }

    // private float currentWaterValue; //Calculated water production
    // public float CurrentWaterValue
    // {
    //     get => currentWaterValue;
    //     set { currentWaterValue = value; }
    // }
    //
    // private float currentWaterDemand; //combined water consumption of all Citizens
    // public float CurrentWaterDemand
    // {
    //     get => currentWaterDemand;
    //     set { currentWaterDemand = value; }
    // }
    //
    // private float currentWaterProduction; //Water production of every water source combined
    // public float CurrentWaterProduction
    // {
    //     get => currentWaterProduction;
    //     set { currentWaterProduction = value; }
    // }
    //
    // private float savedWaterValue; //No use for now

    private const float waterScalingFactor = 1.6f; //Factor to multiply the demand based off of the current citizen number (Can later be changed into dynamic field to change scaling over time)

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

    private void Start()
    {
        InvokeRepeating(nameof(InvokeCalculation), 0f, 0.5f);
    }



    /// <summary>
    /// Class which is called to call the Calculation of currentFoodValue with parameters
    /// </summary>
    protected override void InvokeCalculation()
    {
        CalculateCurrentResourceSurplus(CurrentResourceProduction, CurrentResourceDemand);
        CalculateSavedResourceValue();
    }

    /// <summary>
    /// Calculation of currentFoodValue
    /// </summary>
    /// <param name="currentProduction">Combined value of all water production sources</param>
    /// <param name="savedValue">Won't be used since there won't be any saving Options like silos</param>
    /// <param name="currentDemand">Combined value of all water consuming sources like modules</param>
    protected override void CalculateCurrentResourceSurplus(float currentProduction, float currentDemand)
    {
        CurrentResourceSurplus = currentProduction - currentDemand;
        surplusText.text = $"{CurrentResourceSurplus}";
    }

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
            // Kill People???
        }
        
        savedResourceText.text = $"{(int)SavedResourceValue}/{SaveSpace}";
    }
}
