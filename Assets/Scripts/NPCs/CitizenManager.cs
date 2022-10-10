using TMPro;
using UnityEngine;

public class CitizenManager : MonoBehaviour
{
    private static CitizenManager instance;
    public static CitizenManager Instance
    {
        get => instance;
        set => instance = value;
    }
    
    private int citizen;
    //public int Citizen => citizen;
    public int Citizen
    {
        get => citizen;
        set { citizen = value; }
    }

    // private int joblessCitizen;                          TODO
    // public int JoblessCitizen
    // {
    //     get => joblessCitizen;
    //     set => joblessCitizen = value;
    // }

    [SerializeField] private TextMeshProUGUI savedResourceText;
    [SerializeField] private TextMeshProUGUI surplusText;
    [SerializeField] private int growTime;
    [SerializeField] private float waterConsumptionPerCitizen;
    [SerializeField] private float foodConsumptionPerCitizen;
    private FoodManager foodManager;
    private WaterManager waterManager;
    private int housing;
    public int Housing { get => housing ; set => housing = value; }
    
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
    
    void Start()
    {
        foodManager = FoodManager.Instance;
        waterManager = WaterManager.Instance;
        InvokeRepeating(nameof(Growth), 0, growTime);
        foodManager.CurrentResourceDemand = citizen * foodConsumptionPerCitizen;
        waterManager.CurrentResourceDemand = citizen * waterConsumptionPerCitizen;
    }

    private void Growth()
    {
        if (housing < citizen)
        {
            ChangeCitizen(-1);
        }
        else if (housing == citizen && (foodManager.CurrentResourceSurplus < 0 || waterManager.CurrentResourceSurplus < 0))
        {
            ChangeCitizen(-1);
        }
        else if (housing > citizen)
        {
            if (foodManager.CurrentResourceSurplus > 0 && waterManager.CurrentResourceSurplus > 0)
            {
                ChangeCitizen(1);
            }
            else if (foodManager.CurrentResourceSurplus < 0 || waterManager.CurrentResourceSurplus < 0)
            {
                ChangeCitizen(-1);
            }
        }
        else
        {
            surplusText.text = $"0";
        }

        savedResourceText.text = $"{citizen}/{Housing}";
    }

    private void ChangeCitizen(int value)
    {
        citizen += value;
        foodManager.CurrentResourceDemand += foodConsumptionPerCitizen * value;
        waterManager.CurrentResourceDemand += waterConsumptionPerCitizen * value;
        surplusText.text = $"{value}";
    }
}
