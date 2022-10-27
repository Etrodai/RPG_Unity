using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ResourceManagement.Manager
{
    public class EnergyManager : ResourceManager
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI savedResourceText;
        [SerializeField] private TextMeshProUGUI surplusText;
        [SerializeField] private float repeatRate = 0.5f;
        private float dividendFor10Seconds;
        private GameManager gameManager;
        private float currentResourceSurplus;
        private float savedResourceValue;
        private float saveSpace;
        #endregion

        private UnityEvent onEnergySurplusChanged;
        private UnityEvent onEnergySavedValueChanged;
        private UnityEvent onEnergySaveSpaceChanged;
        
        #region Properties

        public static EnergyManager Instance { get; private set; }
        public override float CurrentResourceSurplus 
        {        
            get => currentResourceSurplus;
            set
            {
                currentResourceSurplus = value;
                onEnergySurplusChanged?.Invoke();
            } 
        }
        public override float CurrentResourceProduction { get; set; }
        public override float CurrentResourceDemand { get; set; }
        public override float SavedResourceValue
        {        
            get => savedResourceValue;
            set
            {
                savedResourceValue = value;
                onEnergySavedValueChanged?.Invoke();
            } 
        }
        public override float SaveSpace        
        {        
            get => saveSpace;
            set
            {
                saveSpace = value;
                onEnergySaveSpaceChanged?.Invoke();
            } 
        }
        public override ResourceTypes ResourceType { get; set; } = ResourceTypes.Energy;


        #endregion

        #region OldCode

        // private float currentEnergyValue; //Calculated Energy production
        // public float CurrentEnergyValue
        // {
        //     get => currentEnergyValue;
        //     set { currentEnergyValue = value; }
        // }

        // private float currentEnergyDemand; //Energy consumption of every module combined
        // public float CurrentEnergyDemand
        // {
        //     get => currentEnergyDemand;
        //     set { currentEnergyDemand = value; }
        // }
        //
        // private float currentEnergyProduction; //Energy production of every Energy source combined
        // public float CurrentEnergyProduction
        // {
        //     get => currentEnergyProduction;
        //     set { currentEnergyProduction = value; }
        // }
        //
        // private float savedEnergyValue; //No use for now

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
        /// Starts Calculation
        /// </summary>
        private void Start()
        {
            onEnergySurplusChanged = new UnityEvent();
            onEnergySurplusChanged.AddListener(ChangeUIText);
            onEnergySavedValueChanged = new UnityEvent();
            onEnergySavedValueChanged.AddListener(ChangeUIText);
            onEnergySaveSpaceChanged = new UnityEvent();
            onEnergySaveSpaceChanged.AddListener(ChangeUIText);
            gameManager = GameManager.Instance;
            dividendFor10Seconds = 10 / repeatRate;
            InvokeRepeating(nameof(InvokeCalculation), 0, repeatRate); 
        }

        #endregion

        #region Methods

        /// <summary>
        /// calls the Calculations
        /// </summary>
        protected override void InvokeCalculation()
        {
            CalculateCurrentResourceSurplus();
            CalculateSavedResourceValue();
        }

        #region OldSummary

        /// <summary>
        /// Calculation of currentEnergyValue
        /// </summary>
        /// <param name="currentProduction">Combined value of all energy production sources</param>
        /// <param name="savedValue">Combined value of all (when needed) active saved energy sources like batteries</param>
        /// <param name="currentDemand">Combined value of all energy consuming sources like modules</param>

        #endregion

        /// <summary>
        /// Calculation of currentMaterialSurplus
        /// </summary>
        protected override void CalculateCurrentResourceSurplus()
        {
            CurrentResourceSurplus = CurrentResourceProduction - CurrentResourceDemand;
        }

        /// <summary>
        /// Calculation of SavedMaterialValue every 0.5 seconds
        /// </summary>
        protected override void CalculateSavedResourceValue()
        {
            if (SaveSpace > SavedResourceValue + CurrentResourceSurplus / dividendFor10Seconds)
            {
                SavedResourceValue += CurrentResourceSurplus / dividendFor10Seconds;
            }
            else
            {
                SavedResourceValue = SaveSpace;
            }

            if (SavedResourceValue < 0)
            {
                gameManager.DisableBuildings(CurrentResourceSurplus, ResourceType);
                SavedResourceValue = 0;
            }
            else
            {
                // gameManager.EnableBuildings(CurrentResourceSurplus, resourceType);
                gameManager.EnableBuildings(CurrentResourceSurplus, ResourceType);
            }
        }

        private void ChangeUIText()
        {
            surplusText.text = $"{CurrentResourceSurplus}";
            savedResourceText.text = $"{(int) SavedResourceValue}/{SaveSpace}";
        }
            
        #endregion
    }
}
