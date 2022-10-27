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

        private UnityEvent onMaterialSurplusChanged;
        private UnityEvent onMaterialSavedValueChanged;
        private UnityEvent onMaterialSaveSpaceChanged;

        #region Properties

        public static MaterialManager Instance { get; private set; }
        public override float CurrentResourceSurplus 
        {        
            get => currentResourceSurplus;
            set
            {
                currentResourceSurplus = value;
                onMaterialSurplusChanged?.Invoke();
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
                onMaterialSavedValueChanged?.Invoke();
            } 
        }
        public override float SaveSpace
        {        
            get => saveSpace;
            set
            {
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
        /// Starts Calculation
        /// </summary>
        private void Start()
        {
            onMaterialSurplusChanged = new UnityEvent();
            onMaterialSurplusChanged.AddListener(ChangeUIText);
            onMaterialSavedValueChanged = new UnityEvent();
            onMaterialSavedValueChanged.AddListener(ChangeUIText);
            onMaterialSaveSpaceChanged = new UnityEvent();
            onMaterialSaveSpaceChanged.AddListener(ChangeUIText);
            gameManager = GameManager.Instance;
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