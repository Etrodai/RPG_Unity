using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilingResourceManager : ResourceManager
{
    private static BuilingResourceManager instance; //Singleton
    public static BuilingResourceManager Instance
    {
        get => instance;
        set { instance = value; }
    }
    
    public override float CurrentResourceValue { get; set; }
    public override float CurrentResourceProduction { get; set; }
    public override float CurrentResourceDemand { get; set; }
    public override float SavedResourceValue { get; set; }
    
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
    
    protected override void InvokeCalculation()
    {
        CalculateCurrentResourceValue(CurrentResourceProduction, SavedResourceValue, CurrentResourceDemand);
    }

    protected override void CalculateCurrentResourceValue(float currentProduction, float savedValue, float currentDemand)
    {
        CurrentResourceValue = currentProduction + savedValue - currentDemand;
    }
}
