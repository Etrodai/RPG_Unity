using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ResourceManagement.Manager
{
    public class WaterManager : ResourceManager
    {
        #region TODOS

        // use waterScalingFactor

        #endregion

        #region Variables

        [SerializeField] private TextMeshProUGUI savedResourceText;
        [SerializeField] private TextMeshProUGUI surplusText;
        [SerializeField] private float repeatRate = 0.5f;
        private float dividendFor10Seconds;
        private float currentResourceSurplus;
        private float savedResourceValue;
        private float saveSpace;

        // private const float waterScalingFactor = 1.6f; //Factor to multiply the demand based off of the current citizen number (Can later be changed into dynamic field to change scaling over time)

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

                onWaterSurplusChanged?.Invoke();
                // Debug.Log("onWaterSurplusChanged?.Invoke()");
                currentResourceSurplus = value;
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

                onWaterSavedValueChanged?.Invoke();
                // Debug.Log("onWaterSavedValueChanged?.Invoke()");
                savedResourceValue = value;
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

                onWaterSaveSpaceChanged?.Invoke();
                // Debug.Log("onWaterSaveSpaceChanged?.Invoke()");
                saveSpace = value;
            }
        }
        public override ResourceTypes ResourceType { get; set; } = ResourceTypes.Water;

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
            surplusText.text = $"{CurrentResourceSurplus}";
            savedResourceText.text = $"{(int) SavedResourceValue}/{SaveSpace}";
        }
        
        #endregion
    }
}