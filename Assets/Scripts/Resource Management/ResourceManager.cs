using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceManager : MonoBehaviour
{
    // protected float currentResourceValue;
    public abstract float CurrentResourceValue { get; set; }

    // protected float currentResourceProduction;
    public abstract float CurrentResourceProduction { get; set; }

    // protected float currentResourceDemand;
    public abstract float CurrentResourceDemand { get; set; }
    
    // protected float savedResourceValue;
    public abstract float SavedResourceValue { get; set; }

    protected abstract void InvokeCalculation();
    protected abstract void CalculateCurrentResourceValue(float currentProduction, float savedValue, float currentDemand);
}
