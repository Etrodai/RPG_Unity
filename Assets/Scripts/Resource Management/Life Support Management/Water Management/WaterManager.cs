using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : ResourceManager
{
    private static WaterManager instance; //Singleton
    public static WaterManager Instance
    {
        get => instance;
        set { instance = value; }
    }

    private float currentWaterValue; //Calculated water production
    public float CurrentWaterValue
    {
        get => currentWaterValue;
        set { currentWaterValue = value; }
    }

    private float currentWaterDemand; //combined water consumption of all Citizens
    public float CurrentWaterDemand
    {
        get => currentWaterDemand;
        set { currentWaterDemand = value; }
    }

    private float currentWaterProduction; //Water production of every water source combined
    public float CurrentWaterProduction
    {
        get => currentWaterProduction;
        set { currentWaterProduction = value; }
    }

    private float savedWaterValue; //No use for now

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
        InvokeRepeating("InvokeCalculation", 0f, 0.5f);
    }

    /// <summary>
    /// Class which is called to call the Calculation of currentFoodValue with parameters
    /// </summary>
    protected override void InvokeCalculation()
    {
        CalculateCurrentResourceValue(currentWaterProduction, savedWaterValue, currentWaterDemand);
    }

    /// <summary>
    /// Calculation of currentFoodValue
    /// </summary>
    /// <param name="currentProduction">Combined value of all water production sources</param>
    /// <param name="savedValue">Won't be used since there won't be any saving Options like silos</param>
    /// <param name="currentDemand">Combined value of all water consuming sources like modules</param>
    protected override void CalculateCurrentResourceValue(float currentProduction, float savedValue, float currentDemand)
    {
        currentWaterValue = currentProduction + savedWaterValue - currentDemand * waterScalingFactor;
    }

}
