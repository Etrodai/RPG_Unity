using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ResourceManagement.Manager
{
    public class FoodManager : ResourceManager
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI savedResourceText;
        [SerializeField] private TextMeshProUGUI surplusText;
        [SerializeField] private float repeatRate = 0.5f;
        private float dividendFor10Seconds;
        private float currentResourceSurplus;
        private float savedResourceValue;
        private float saveSpace;

        #endregion

        #region Events

        private UnityEvent onFoodSurplusChanged;
        private UnityEvent onFoodSavedValueChanged;
        private UnityEvent onFoodSaveSpaceChanged;

        #endregion

        #region Properties

        private static FoodManager Instance { get; set; }
        public override float CurrentResourceSurplus
        {        
            get => currentResourceSurplus;
            set
            {
                currentResourceSurplus = value;
                onFoodSurplusChanged?.Invoke();
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
                onFoodSavedValueChanged?.Invoke();
            } 
        }
        public override float SaveSpace
        {        
            get => saveSpace;
            set
            {
                saveSpace = value;
                onFoodSaveSpaceChanged?.Invoke();
            } 
        }
        public override ResourceTypes ResourceType { get; set; } = ResourceTypes.Food;

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
            onFoodSurplusChanged = new UnityEvent();
            onFoodSurplusChanged.AddListener(ChangeUIText);
            onFoodSavedValueChanged = new UnityEvent();
            onFoodSavedValueChanged.AddListener(ChangeUIText);
            onFoodSaveSpaceChanged = new UnityEvent();
            onFoodSaveSpaceChanged.AddListener(ChangeUIText);
            dividendFor10Seconds = 10 / repeatRate;
            InvokeRepeating(nameof(InvokeCalculation), 0f, repeatRate);
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
                SavedResourceValue = 0;
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