using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ResourceManagement.Manager
{
    public class CitizenManager : ResourceManager
    {
        #region TODOS
        
        // Weniger Citizen, wenn Surplus negativ, oder wenn kein SavedResource mehr??

        #endregion

        #region Variables

        private static CitizenManager instance;
        [SerializeField] private TextMeshProUGUI savedResourceText;
        [SerializeField] private TextMeshProUGUI surplusText;
        [SerializeField] private int growTime;
        [SerializeField] private float waterConsumptionPerCitizen;
        [SerializeField] private float foodConsumptionPerCitizen;
        [SerializeField] private float repeatRate = 0.5f;
        private FoodManager foodManager;
        private WaterManager waterManager;
        private GameManager gameManager;
        private ResourceTypes resourceType = ResourceTypes.Citizen;

        #endregion

        #region Properties

        public static CitizenManager Instance => instance;
        public override float CurrentResourceSurplus { get; set; } // GrowthValue
        public override float CurrentResourceProduction { get; set; } // Citizen
        public override float CurrentResourceDemand { get; set; } // neededCitizen
        public override float SavedResourceValue { get; set; } // Jobless
        public override float SaveSpace { get; set; } // Housing
        public override ResourceTypes ResourceType { get => resourceType; set => resourceType = value; } // resourceType

        #endregion

        #region UnityEvents

        /// <summary>
        /// Singleton
        /// </summary>
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

        /// <summary>
        /// sets variables
        /// starts growing
        /// sets food and water demand
        /// </summary>
        void Start()
        {
            CurrentResourceProduction = 10;
            foodManager = FoodManager.Instance;
            waterManager = WaterManager.Instance;
            gameManager = GameManager.Instance;
            InvokeCalculation();
        }

        #endregion

        #region Methods
        
        protected override void InvokeCalculation()
        {
            InvokeRepeating(nameof(CalculateCurrentResourceSurplus), 0, growTime);
            InvokeRepeating(nameof(CalculateSavedResourceValue), 0, repeatRate);
            foodManager.CurrentResourceDemand = CurrentResourceProduction * foodConsumptionPerCitizen;
            waterManager.CurrentResourceDemand = CurrentResourceProduction * waterConsumptionPerCitizen;
        }

        /// <summary>
        /// reduces citizen, if there are too few housings, food or water
        /// adds citizen, if there are more than needed housings, food and water
        /// has no surplus, if there are nothing is too few or not all variables are more than needed
        /// </summary>
        protected override void CalculateCurrentResourceSurplus() // Growth
        {
            if (SaveSpace < CurrentResourceProduction || foodManager.CurrentResourceSurplus < 0 || waterManager.CurrentResourceSurplus < 0)
            {
                CurrentResourceSurplus = -1;
            }
            else if (SaveSpace > CurrentResourceProduction && foodManager.CurrentResourceSurplus > 0 &&
                     waterManager.CurrentResourceSurplus > 0)
            {
                CurrentResourceSurplus = 1;
            }
            else
            {
                CurrentResourceSurplus = 0;
            }
            
            ChangeCitizen(CurrentResourceSurplus);
            savedResourceText.text = $"{CurrentResourceProduction}/{SaveSpace} \nJobless: {SavedResourceValue}/{CurrentResourceProduction}";
        }
        
        /// <summary>
        /// adds or reduces citizen
        /// changes food and water demand
        /// </summary>
        /// <param name="value">value of citizens to be added or reduced </param>
        private void ChangeCitizen(float value)
        {
            surplusText.text = $"{value}";
            if (value == 0)
            {
                return;
            }
            CurrentResourceProduction += value;
            foodManager.CurrentResourceDemand += foodConsumptionPerCitizen * value;
            waterManager.CurrentResourceDemand += waterConsumptionPerCitizen * value;
        }

        protected override void CalculateSavedResourceValue() // CalculateJoblessCitizen
        {
            SavedResourceValue = CurrentResourceProduction - CurrentResourceDemand;
            if (SavedResourceValue < 0)
            {
                gameManager.DisableBuildings(SavedResourceValue, resourceType);
            }
            else
            {
                gameManager.EnableBuildings(SavedResourceValue, resourceType);
            }

            // Debug.Log("Jobless " + joblessCitizen + "\nNeeded " + neededCitizen);
        }

        #endregion
    }
}
