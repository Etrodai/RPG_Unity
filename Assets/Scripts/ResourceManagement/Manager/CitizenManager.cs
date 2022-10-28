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

                private UnityEvent onCitizenSurplusChanged;
                private UnityEvent onCitizenProductionChanged;
                private UnityEvent onCitizenSavedValueChanged;
                private UnityEvent onCitizenSaveSpaceChanged;

        #endregion
        
        #region Properties

        private static CitizenManager Instance { get; set; }
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
            Save.onSaveButtonClick.AddListener(SaveData);
            Save.onSaveAsButtonClick.AddListener(SaveDataAs);
            Load.onLoadButtonClick.AddListener(LoadData);
            onCitizenSurplusChanged = new UnityEvent();
            onCitizenSurplusChanged.AddListener(ChangeUIText);
            onCitizenProductionChanged = new UnityEvent();
            onCitizenProductionChanged.AddListener(ChangeUIText);
            onCitizenSavedValueChanged = new UnityEvent();
            onCitizenSavedValueChanged.AddListener(ChangeUIText);
            onCitizenSavedValueChanged.AddListener(CalculateProductivity);
            onCitizenSaveSpaceChanged = new UnityEvent();
            onCitizenSaveSpaceChanged.AddListener(ChangeUIText);
            CurrentResourceProduction = 10;
            foodManager = MainManagerSingleton.Instance.FoodManager;
            waterManager = MainManagerSingleton.Instance.WaterManager;
            gameManager = MainManagerSingleton.Instance.GameManager;
            InvokeCalculation();
        }

        #endregion

        #region Save and Load

        private void SaveData()
        {
            Save.AutoSaveData(SavedResourceValue, "citizenManager");
        }

        private void SaveDataAs(string savePlace)
        {
            Save.SaveDataAs(savePlace, SavedResourceValue, "citizenManager.dat");
        }

        private void LoadData(string path)
        {
            path = Path.Combine(path, "citizenManager.dat");
            SavedResourceValue = Load.LoadFloatData(path);
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
            CalculateProductivity();
        }

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
        
        private void ChangeUIText()
        {
            surplusText.text = $"{CurrentResourceSurplus}";
            currentResourceProductionText.text = $"{CurrentResourceProduction}/{SaveSpace}";
            savedResourceText.text = $"{SavedResourceValue}/{CurrentResourceProduction}";
        }

        #endregion
    }
}
