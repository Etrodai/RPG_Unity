using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ResourceManagement.Manager
{
    public class CitizenManager : ResourceManager
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI currentResourceProductionText;
        [SerializeField] private TextMeshProUGUI surplusText;
        [SerializeField] private TextMeshProUGUI savedResourceText;
        [SerializeField] private int growTime;
        [SerializeField] private float waterConsumptionPerCitizen;
        [SerializeField] private float foodConsumptionPerCitizen;
        [SerializeField] private float repeatRate = 0.5f;
        [SerializeField] private int stepsToGrow;
        [SerializeField] private int sizeOfStepsToGrow;
        private FoodManager foodManager;
        private WaterManager waterManager;
        private GameManager gameManager;
        private float currentResourceSurplus;
        private float savedResourceValue;
        private float saveSpace;
        private float currentResourceProduction;

        #endregion

        private UnityEvent onCitizenSurplusChanged;
        private UnityEvent onCitizenProductionChanged;
        private UnityEvent onCitizenSavedValueChanged;
        private UnityEvent onCitizenSaveSpaceChanged;
        
        #region Properties

        public static CitizenManager Instance { get; private set; }
        public override float CurrentResourceSurplus // GrowthValue
        {        
            get => currentResourceSurplus;
            set
            {
                currentResourceSurplus = value;
                onCitizenSurplusChanged?.Invoke();
            } 
        }
        public override float CurrentResourceProduction // Citizen
        {        
            get => currentResourceProduction;
            set
            {
                currentResourceProduction = value;
                onCitizenProductionChanged?.Invoke();
            } 
        }
        public override float CurrentResourceDemand { get; set; } // neededCitizen
        public override float SavedResourceValue // Jobless
        {        
            get => savedResourceValue;
            set
            {
                savedResourceValue = value;
                onCitizenSavedValueChanged?.Invoke();
            } 
        }
        public override float SaveSpace // Housing
        {        
            get => saveSpace;
            set
            {
                saveSpace = value;
                onCitizenSaveSpaceChanged?.Invoke();
            } 
        }
        public override ResourceTypes ResourceType { get; set; } = ResourceTypes.Citizen; // resourceType

        #endregion

        #region UnityEvents

        /// <summary>
        /// Singleton
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// sets variables
        /// starts growing
        /// sets food and water demand
        /// </summary>
        void Start()
        {
            onCitizenSurplusChanged = new UnityEvent();
            onCitizenSurplusChanged.AddListener(ChangeUIText);
            onCitizenProductionChanged = new UnityEvent();
            onCitizenProductionChanged.AddListener(ChangeUIText);
            onCitizenSavedValueChanged = new UnityEvent();
            onCitizenSavedValueChanged.AddListener(ChangeUIText);
            onCitizenSaveSpaceChanged = new UnityEvent();
            onCitizenSaveSpaceChanged.AddListener(ChangeUIText);
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
            InvokeRepeating(nameof(CalculateCurrentResourceSurplus), 0, repeatRate);
            InvokeRepeating(nameof(CalculateSavedResourceValue), 0, repeatRate);
            InvokeRepeating(nameof(ChangeCitizen), 0, growTime);
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
            if ((int) SaveSpace == (int)CurrentResourceProduction || foodManager.CurrentResourceSurplus <= 0 || waterManager.CurrentResourceSurplus <= 0)
            {
                CurrentResourceSurplus = 0;
            } 
            else if (SaveSpace < CurrentResourceProduction)
            {
                CurrentResourceSurplus = -1;
            }
            else if (foodManager.SavedResourceValue <= 0 || waterManager.SavedResourceValue <= 0)
            {
                for (int i = 0; i > -stepsToGrow; i--)
                {
                    if (foodManager.CurrentResourceProduction - foodManager.CurrentResourceDemand < i * sizeOfStepsToGrow)
                    {
                        CurrentResourceSurplus = i - 1;
                    }
                }
            }
            else if (SaveSpace > CurrentResourceProduction && foodManager.CurrentResourceSurplus > 0 && waterManager.CurrentResourceSurplus > 0)
            {
                for (int i = 0; i < stepsToGrow; i++)
                {
                    if (foodManager.CurrentResourceSurplus > i * sizeOfStepsToGrow)
                    {
                        CurrentResourceSurplus = i + 1;
                    }
                }
            }
        }
        
        /// <summary>
        /// adds or reduces citizen
        /// changes food and water demand
        /// </summary>
        /// <param name="value">value of citizens to be added or reduced </param>
        private void ChangeCitizen()
        {
            if (CurrentResourceSurplus == 0)
            {
                return;
            }
            CurrentResourceProduction += CurrentResourceSurplus;
            foodManager.CurrentResourceDemand += foodConsumptionPerCitizen * CurrentResourceSurplus;
            waterManager.CurrentResourceDemand += waterConsumptionPerCitizen * CurrentResourceSurplus;
        }

        protected override void CalculateSavedResourceValue() // CalculateJoblessCitizen
        {
            SavedResourceValue = CurrentResourceProduction - CurrentResourceDemand;
            if (SavedResourceValue == 0)
            {
                savedResourceText.text = $"{SavedResourceValue}/{CurrentResourceProduction}";
                return;
            }
            if (SavedResourceValue < 0)
            {
                gameManager.DisableBuildings(SavedResourceValue, ResourceType);
            }
            else
            {
                gameManager.ChangeProductivityPositive(SavedResourceValue);
            }
        }

        private void ChangeUIText()
        {
            surplusText.text = $"{CurrentResourceSurplus}";
            currentResourceProductionText.text = $"{CurrentResourceProduction}/{SaveSpace}";
            savedResourceText.text = $"{SavedResourceValue}/{CurrentResourceProduction}";
        }

        #endregion
    }
}
