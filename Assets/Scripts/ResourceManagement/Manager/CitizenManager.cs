using System.IO;
using Manager;
using SaveSystem;
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

        #region Events

                private UnityEvent onCitizenSurplusChanged = new();
                private UnityEvent onCitizenProductionChanged = new();
                private UnityEvent onCitizenSavedValueChanged = new();
                private UnityEvent onCitizenSaveSpaceChanged = new();

        #endregion
        
        #region Properties

        private static CitizenManager Instance { get; set; }
        
        /// <summary>
        /// GrowthValue
        /// OnPropertyChangedEvent
        /// </summary>
        public override float CurrentResourceSurplus
        {        
            get => currentResourceSurplus;
            set
            {
                if (currentResourceSurplus == value) return;

                currentResourceSurplus = value;
                onCitizenSurplusChanged?.Invoke();
                // Debug.Log("onCitizenSurplusChanged?.Invoke()");
            } 
        }
        
        /// <summary>
        /// Citizen
        /// OnPropertyChangedEvent
        /// </summary>
        public override float CurrentResourceProduction
        {        
            get => currentResourceProduction;
            set
            {
                if (currentResourceProduction == value) return;

                currentResourceProduction = value;
                onCitizenProductionChanged?.Invoke();
                // Debug.Log("onCitizenProductionChanged?.Invoke()");
            } 
        }
        
        /// <summary>
        /// NeededCitizen
        /// </summary>
        public override float CurrentResourceDemand { get; set; }
        
        /// <summary>
        /// JoblessCitizen
        /// OnPropertyChangedEvent
        /// </summary>
        public override float SavedResourceValue
        {        
            get => savedResourceValue;
            set
            {
                if (savedResourceValue == value) return;

                savedResourceValue = value;
                onCitizenSavedValueChanged?.Invoke();
                // Debug.Log("onCitizenSavedValueChanged?.Invoke()");
            } 
        }
        
        /// <summary>
        /// Housing
        /// OnPropertyChangedEvent
        /// </summary>
        public override float SaveSpace // Housing
        {        
            get => saveSpace;
            set
            {
                if (saveSpace == value) return;

                saveSpace = value;
                onCitizenSaveSpaceChanged?.Invoke();
                // Debug.Log("onCitizenSaveSpaceChanged?.Invoke()");
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
        /// adds Listener
        /// sets variables
        /// starts growing
        /// sets food and water demand
        /// </summary>
        void Start()
        {
            onCitizenSurplusChanged.AddListener(ChangeUIText);
            onCitizenProductionChanged.AddListener(ChangeUIText);
            onCitizenSavedValueChanged.AddListener(ChangeUIText);
            onCitizenSavedValueChanged.AddListener(CalculateProductivity);
            onCitizenSaveSpaceChanged.AddListener(ChangeUIText);
            CurrentResourceProduction = 10;
            foodManager = MainManagerSingleton.Instance.FoodManager;
            waterManager = MainManagerSingleton.Instance.WaterManager;
            gameManager = MainManagerSingleton.Instance.GameManager;
            InvokeCalculation();
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// calls the Calculations
        /// </summary>
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

        /// <summary>
        /// Calculates JoblessCitizen
        /// </summary>
        protected override void CalculateSavedResourceValue()
        {
            SavedResourceValue = CurrentResourceProduction - CurrentResourceDemand;
            if (SavedResourceValue == 0)
            {
                savedResourceText.text = $"{SavedResourceValue}/{CurrentResourceProduction}";
                return;
            }
            CalculateProductivity();
        }

        /// <summary>
        /// Calculates Productivity of all Buildings in List
        /// </summary>
        private void CalculateProductivity()
        {
            if (SavedResourceValue == 0)
            {
                return;
            }
            if (SavedResourceValue < 0)
            {
                gameManager.DisableBuildings(SavedResourceValue, ResourceType);
            }
            else if (gameManager.ChangedProductivityBuildings.Count != 0)
            {
                gameManager.ChangeProductivityPositive(SavedResourceValue);
            }
        }
        
        /// <summary>
        /// changes UIText (surplus, savedResource and saveSpace)
        /// </summary>
        private void ChangeUIText()
        {
            surplusText.text = $"{CurrentResourceSurplus}";
            currentResourceProductionText.text = $"{CurrentResourceProduction}/{SaveSpace}";
            savedResourceText.text = $"{SavedResourceValue}/{CurrentResourceProduction}";
        }

        #endregion
    }
}
