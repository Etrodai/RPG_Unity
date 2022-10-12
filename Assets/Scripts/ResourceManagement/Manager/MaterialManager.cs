using TMPro;
using UnityEngine;

namespace ResourceManagement.Manager
{
    public class MaterialManager : ResourceManager
    {
        #region Variables

        private static MaterialManager instance;
        [SerializeField] private TextMeshProUGUI savedResourceText;
        [SerializeField] private TextMeshProUGUI surplusText;
        [SerializeField] private float repeatRate = 0.5f;
        private float dividendFor10Seconds;
        private ResourceTypes resourceType = ResourceTypes.Material;


        #endregion

        #region Properties

        public static MaterialManager Instance
        {
            get => instance;
            set { instance = value; }
        }

        public override float CurrentResourceSurplus { get; set; }
        public override float CurrentResourceProduction { get; set; }
        public override float CurrentResourceDemand { get; set; }
        public override float SavedResourceValue { get; set; }
        public override float SaveSpace { get; set; }

        #endregion

        #region UnityEvents

        /// <summary>
        /// Singleton
        /// </summary>
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        /// <summary>
        /// Starts Calculation
        /// </summary>
        private void Start()
        {
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
            surplusText.text = $"{CurrentResourceSurplus}";
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
                GameManager.Instance.DisableBuildings(SavedResourceValue, resourceType);
                SavedResourceValue = 0;
            }
            else
            {
                GameManager.Instance.EnableBuildings(SavedResourceValue, resourceType);
            }

            savedResourceText.text = $"{(int) SavedResourceValue}/{SaveSpace}";
        }

        #endregion
    }
}