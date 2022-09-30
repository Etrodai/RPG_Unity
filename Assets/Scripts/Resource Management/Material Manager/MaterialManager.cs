using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MaterialManager : ResourceManager
{
    private static MaterialManager instance; //Singleton
    public static MaterialManager Instance
    {
        get => instance;
        set { instance = value; }
    }
    
    [SerializeField] private TextMeshProUGUI savedResourceText;
    [SerializeField] private TextMeshProUGUI surplusText;

    public override float CurrentResourceSurplus { get; set; }
    public override float CurrentResourceProduction { get; set; }
    public override float CurrentResourceDemand { get; set; }
    public override float SavedResourceValue { get; set; }
    public override float SaveSpace { get; set; }

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
        CalculateCurrentResourceSurplus(CurrentResourceProduction, CurrentResourceDemand);
        CalculateSavedResourceValue();
    }

    protected override void CalculateCurrentResourceSurplus(float currentProduction, float currentDemand)
    {
        CurrentResourceSurplus = currentProduction - currentDemand;
        surplusText.text = $"{CurrentResourceSurplus}";
    }

    protected override void CalculateSavedResourceValue()
    {
        if (SaveSpace > SavedResourceValue + CurrentResourceSurplus/20)
        {
            SavedResourceValue += CurrentResourceSurplus/20;
        }
        else
        {
            SavedResourceValue = SaveSpace;
        }

        if (SavedResourceValue < 0)
        {
            SavedResourceValue = 0;
            // Disable Modules
        }
        
        savedResourceText.text = $"{(int)SavedResourceValue}/{SaveSpace}";
    }
}
