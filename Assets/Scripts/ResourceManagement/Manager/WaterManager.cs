using TMPro;
using UnityEngine;
using UnityEngine.Events;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace ResourceManagement.Manager
{
    public class WaterManager : ResourceManager //Made by Robin
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

        private readonly UnityEvent onWaterSurplusChanged = new();
        private readonly UnityEvent onWaterSavedValueChanged = new();
        private readonly UnityEvent onWaterSaveSpaceChanged = new();

        #endregion

        #region Properties

        private static WaterManager Instance { get; set; }
        
        /// <summary>
        /// OnValueChangedEvent
        /// </summary>
        public override float CurrentResourceSurplus
        {
            get => currentResourceSurplus;
            set
            {
                if (currentResourceSurplus == value) return;
                currentResourceSurplus = value;
                onWaterSurplusChanged?.Invoke();
            }
        }
        
        public override float CurrentResourceProduction { get; set; }
        
        public override float CurrentResourceDemand { get; set; }
        
        /// <summary>
        /// OnValueChangedEvent
        /// </summary>
        public override float SavedResourceValue
        {
            get => savedResourceValue;
            set
            {
                if (savedResourceValue == value) return;
                savedResourceValue = value;
                onWaterSavedValueChanged?.Invoke();
            }
        }
        
        /// <summary>
        /// OnValueChangedEvent
        /// </summary>
        public override float SaveSpace
        {
            get => saveSpace;
            set
            {
                if (saveSpace == value) return;
                saveSpace = value;
                onWaterSaveSpaceChanged?.Invoke();
            }
        }
        public override ResourceType ResourceType { get; set; } = ResourceType.Water;

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
        /// Starts Calculation
        /// </summary>
        private void Start()
        {
            onWaterSurplusChanged.AddListener(ChangeUIText);
            onWaterSavedValueChanged.AddListener(ChangeUIText);
            onWaterSaveSpaceChanged.AddListener(ChangeUIText);
            dividendFor10Seconds = 10 / repeatRate;
            InvokeRepeating(nameof(InvokeCalculation), 0f, repeatRate);
        }
        
        private void OnDestroy()
        {
            onWaterSurplusChanged.RemoveListener(ChangeUIText);
            onWaterSavedValueChanged.RemoveListener(ChangeUIText);
            onWaterSaveSpaceChanged.RemoveListener(ChangeUIText);
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
        /// Calculation of currentWaterSurplus
        /// </summary>
        protected override void CalculateCurrentResourceSurplus()
        {
            CurrentResourceSurplus = CurrentResourceProduction - CurrentResourceDemand;
        }

        /// <summary>
        /// Calculation of SavedWaterValue
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
            surplusText.color = CurrentResourceSurplus >= 0 ? Color.green : Color.red;
            savedResourceText.text = $"{(int)SavedResourceValue}/{(int)SaveSpace}";
        }
        
        #endregion
    }
}