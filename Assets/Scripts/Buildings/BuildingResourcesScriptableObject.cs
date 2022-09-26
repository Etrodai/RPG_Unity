using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buildings", menuName = "Buildings")]
public class BuildingResourcesScriptableObject : ScriptableObject
{
    // [Header("Costs")] 
    // public int resourceCosts;
    //
    // [Header("Consumption")] 
    // public int energyConsumption;
    // public int inhabitantConsumption;
    // public int foodConsumption;
    // public int waterConsumption;
    //
    // [Header("Production")]
    // public int energyProduction;
    // public int inhabitantProduction;
    // public int foodProduction;
    // public int waterProduction;

    [SerializeField] private BuildingResources[] Costs;
    [SerializeField] private Vector3 RequiredSpace;
    [SerializeField] private BuildingResources[] Consumption;
    [SerializeField] private BuildingResources[] Production;
}

public enum Resources
{
    BuildingResource,
    Energy,
    Inhabitant,
    Food,
    Water
}

[Serializable]
public class BuildingResources
{
    public Resources resource;
    public int value;
}
