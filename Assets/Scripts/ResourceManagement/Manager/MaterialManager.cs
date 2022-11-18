using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ResourceManagement.Manager
{
    public class MaterialManager : ResourceManager
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

        #region Events

                private readonly UnityEvent onMaterialSurplusChanged = new();
                private readonly UnityEvent onMaterialSavedValueChanged = new();
                private readonly UnityEvent onMaterialSaveSpaceChanged = new();

        #endregion
        
        #region Properties

        private static MaterialManager Instance { get; set; }
        
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
                onMaterialSurplusChanged?.Invoke();
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
                onMaterialSavedValueChanged?.Invoke();
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
                if (saveSpace == value) return;
                saveSpace = value;
                onMaterialSaveSpaceChanged?.Invoke();
            } 
        }
        
        public override ResourceTypes ResourceType { get; set; } = ResourceTypes.Material;

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
        /// adds listener
        /// starts Calculation
        /// </summary>
        private void Start()
        {
            onMaterialSurplusChanged.AddListener(ChangeUIText);
            onMaterialSavedValueChanged.AddListener(ChangeUIText);
            onMaterialSaveSpaceChanged.AddListener(ChangeUIText);
            gameManager = MainManagerSingleton.Instance.GameManager;
            dividendFor10Seconds = 10 / repeatRate;
            InvokeRepeating(nameof(InvokeCalculation), 0f, repeatRate);
        }

        private void OnDestroy()
        {
            onMaterialSurplusChanged.RemoveListener(ChangeUIText);
            onMaterialSavedValueChanged.RemoveListener(ChangeUIText);
            onMaterialSaveSpaceChanged.RemoveListener(ChangeUIText);
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
                gameManager.DisableBuildings(CurrentResourceSurplus, ResourceType, false);
                SavedResourceValue = 0;
            }
            else if (gameManager.DisabledBuildings.Count != 0)
            {
                gameManager.EnableBuildings(CurrentResourceSurplus, ResourceType);
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