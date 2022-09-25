using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : ResourceManager
{
    private static EnergyManager instance; //Singleton
    public static EnergyManager Instance
    {
        get => instance;
        set { instance = value; }
    }

    private float currentEnergyValue; //Calculated Energy production
    public float CurrentEnergyValue
    {
        get => currentEnergyValue;
        set { currentEnergyValue = value; }
    }

    private float currentEnergyDemand; //Energy consumption of every module combined
    public float CurrentEnergyDemand
    {
        get => currentEnergyDemand;
        set { currentEnergyDemand = value; }
    }

    private float currentEnergyProduction; //Energy production of every Energy source combined
    public float CurrentEnergyProduction
    {
        get => currentEnergyProduction;
        set { currentEnergyProduction = value; }
    }

    private float savedEnergyValue; //No use for now

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

    private void Start()
    {
        InvokeRepeating("InvokeCalculation", 0, 0.5f);
    }

    /// <summary>
    /// Class which is called to call the Calculation of currentEnergyValue with parameters
    /// </summary>
    protected override void InvokeCalculation()
    {
        CalculateCurrentResourceValue(currentEnergyProduction, savedEnergyValue, currentEnergyDemand);
    }

    /// <summary>
    /// Calculation of currentEnergyValue
    /// </summary>
    /// <param name="currentProduction">Combined value of all enrgy production sources</param>
    /// <param name="savedValue">Combined value of all (when needed) active saved energy sources like batteries</param>
    /// <param name="currentDemand">Combined value of all energy consuming sources like modules</param>
    protected override void CalculateCurrentResourceValue(float currentProduction, float savedValue, float currentDemand)
    {
        currentEnergyValue = currentProduction + savedValue - currentDemand;
    }

}
