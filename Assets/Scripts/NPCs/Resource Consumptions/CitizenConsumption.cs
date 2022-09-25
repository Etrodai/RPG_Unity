using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenConsumption : MonoBehaviour
{
    private FoodManager foodManager;
    private WaterManager waterManager;

    [SerializeField] private float foodDemand = 1f; //Food demand of the current citizen
    [SerializeField] private float waterDemand = 1f; //Water demand of the current citizen

    private void Start()
    {
        foodManager = FoodManager.Instance;
        foodManager.CurrentFoodDemand += foodDemand; //Adds own food demand to combined food demand

        waterManager = WaterManager.Instance;
        waterManager.CurrentWaterDemand += waterDemand; //Adds own water demand to combined water demand
    }
}
