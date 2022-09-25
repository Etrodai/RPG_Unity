using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : ResourceManager
{
    private static FoodManager  instance; //Singleton
    public static FoodManager Instance
    {
        get => instance;
        set { instance = value; }
    }

    private float currentFoodValue; //Calculated food production
    public float CurrentFoodValue
    {
        get => currentFoodValue;
        set { currentFoodValue = value; }
    }

    private float currentFoodDemand; //combined food consumption of all Citizens
    public float CurrentFoodDemand
    {
        get => currentFoodDemand;
        set { currentFoodDemand = value; }
    }

    private float currentFoodProduction; //Food production of every food source combined
    public float CurrentFoodProduction
    {
        get => currentFoodProduction;
        set { currentFoodProduction = value; }
    }

    private float savedFoodValue; //No use for now

    private const float foodScalingFactor = 1.25f; //Factor to multiply the demand based off of the current citizen number (Can later be changed into dynamic field to change scaling over time)

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
        CalculateCurrentResourceValue(currentFoodProduction, savedFoodValue, currentFoodDemand);
    }

    /// <summary>
    /// Calculation of currentFoodValue
    /// </summary>
    /// <param name="currentProduction">Combined value of all food production sources</param>
    /// <param name="savedValue">Won't be used since there won't be any saving Options like silos</param>
    /// <param name="currentDemand">Combined value of all food consuming sources like modules</param>
    protected override void CalculateCurrentResourceValue(float currentProduction, float savedValue, float currentDemand)
    {
        currentFoodValue = currentProduction + savedFoodValue - currentDemand * foodScalingFactor;
    }
}
