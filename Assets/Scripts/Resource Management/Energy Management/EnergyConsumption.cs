using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyConsumption : MonoBehaviour
{
    // private EnergyManager energyManager;
    //
    // [SerializeField]
    // private bool isDisabled = false;
    // [SerializeField]
    // private bool isEnabled = false;
    //
    // [SerializeField]
    // private float moduleDemand; //Is supposed to be positive due to the calculation in EnergyManager.CalculateCurrentResourceValue
    //
    // private void Start()
    // {
    //     energyManager = EnergyManager.Instance;
    //     energyManager.CurrentEnergyDemand += moduleDemand;
    // }
    //
    // private void Update()
    // {
    //     if(isDisabled) //Can both later be solved via UI buttons for the module insight view
    //     {
    //         DisableModule();
    //     }
    //     
    //     if(isEnabled)
    //     {
    //         EnableModule();
    //     }
    // }
    //
    // /// <summary>
    // /// Removes the Energy demand of the disabled module and can contain additional Behaviour for when the module is disabled
    // /// </summary>
    // private void DisableModule()
    // {
    //     energyManager.CurrentEnergyDemand -= moduleDemand;
    //     isDisabled = false;
    //     //more Behaviour, for example disabling vfx lights
    // }
    //
    // /// <summary>
    // /// Adds the Energy demand of the enabled module and can contain additional Behaviour for when the module is enabled
    // /// </summary>
    // private void EnableModule()
    // {
    //     energyManager.CurrentEnergyDemand += moduleDemand;
    //     isEnabled = false;
    //     //more Behaviour, for example enabling vfx lights again
    // }
}
