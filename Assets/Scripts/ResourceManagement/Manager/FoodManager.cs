using TMPro;
using UnityEngine;
using UnityEngine.Events;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace ResourceManagement.Manager
{
    public class FoodManager : ResourceManager //Made by Robin
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

        private readonly UnityEvent onFoodSurplusChanged = new();
        private readonly UnityEvent onFoodSavedValueChanged = new();
        private readonly UnityEvent onFoodSaveSpaceChanged = new();

        #endregion

        #region Properties

        private static FoodManager Instance { get; set; }
        
        /// <summary>
        /// OnPropertyChangedEvent
        /// </summary>
        public override float CurrentResourceSurplus
        {        
            get => currentResourceSurplus;
            set
            {
                if (currentResourceSurplus == value) return;
                currentResourceSurplus = value;
                onFoodSurplusChanged?.Invoke();
            } 
        }
        
        public override float CurrentResourceProduction { get; set; }
        
        public override float CurrentResourceDemand { get; set; }
        
        /// <summary>
        /// OnPropertyChangedEvent
        /// </summary>
        public override float SavedResourceValue
        {        
            get => savedResourceValue;
            set
            {
                if (savedResourceValue == value) return;
                savedResourceValue = value;
                onFoodSavedValueChanged?.Invoke();
            } 
        }
        
        /// <summary>
        /// OnPropertyChangedEvent
        /// </summary>
        public override float SaveSpace
        {        
            get => saveSpace;
            set
            {
                if (SaveSpace == value) return;
                saveSpace = value;
                onFoodSaveSpaceChanged?.Invoke();
            } 
        }
        
        public override ResourceType ResourceType { get; set; } = ResourceType.Food;

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
        /// starts Calculation
        /// </summary>
        private void Start()
        {
            onFoodSurplusChanged.AddListener(ChangeUIText);
            onFoodSavedValueChanged.AddListener(ChangeUIText);
            onFoodSaveSpaceChanged.AddListener(ChangeUIText);
            dividendFor10Seconds = 10 / repeatRate;
            InvokeRepeating(nameof(InvokeCalculation), 0f, repeatRate);
        }
        
        private void OnDestroy()
        {        
            onFoodSurplusChanged.RemoveListener(ChangeUIText);
            onFoodSavedValueChanged.RemoveListener(ChangeUIText);
            onFoodSaveSpaceChanged.RemoveListener(ChangeUIText);
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
        /// Calculation of SavedMaterialValue
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

        /// <summary>
        /// changes UIText (surplus, savedResource and saveSpace)
        /// </summary>
        private void ChangeUIText()
        {
            surplusText.text = CurrentResourceSurplus > 0 ? $"+{(int)CurrentResourceSurplus}" : $"{(int)CurrentResourceSurplus}";
            savedResourceText.text = $"{(int)SavedResourceValue}/{(int)SaveSpace}";
        }
        
        #endregion
    }
}