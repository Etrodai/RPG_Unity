using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodeLogic
{
    public void EnergyPlus100()
    {
        EnergyManager.Instance.SavedResourceValue += 100;
    }
    public void FoodPlus100()
    {
        FoodManager.Instance.SavedResourceValue += 100;
    }
    public void WaterPlus100()
    {
        WaterManager.Instance.SavedResourceValue += 100;
    }
    public void MaterialPlus100()
    {
        MaterialManager.Instance.SavedResourceValue += 100;
    }
}
