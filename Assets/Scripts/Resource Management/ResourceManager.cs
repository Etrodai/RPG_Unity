using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceManager : MonoBehaviour
{
    //protected float currentResourceValue;
    //public float CurrentResourceValue
    //{
    //    get => currentResourceValue;
    //    set { currentResourceValue = value; }
    //}

    //protected float currentResourceProduction;
    //protected float savedResourceValue;

    protected abstract void InvokeCalculation();
    protected abstract void CalculateCurrentResourceValue(float currentProduction, float savedValue, float currentDemand);
}
