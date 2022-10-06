using UnityEngine;


public abstract class ResourceManager : MonoBehaviour
{
    // protected float currentResourceValue;
    public abstract float CurrentResourceSurplus { get; set; }

    // protected float currentResourceProduction;
    public abstract float CurrentResourceProduction { get; set; }

    // protected float currentResourceDemand;
    public abstract float CurrentResourceDemand { get; set; }
    
    // protected float savedResourceValue;
    public abstract float SavedResourceValue { get; set; }

    public abstract float SaveSpace { get; set; }
    
    protected abstract void InvokeCalculation();
    protected abstract void CalculateCurrentResourceSurplus(float currentProduction, float currentDemand);
    protected abstract void CalculateSavedResourceValue();
}
