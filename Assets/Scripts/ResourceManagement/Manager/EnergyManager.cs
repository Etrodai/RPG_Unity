using TMPro;
using UnityEngine;

namespace ResourceManagement.Manager
{
    public class EnergyManager : ResourceManager
    {
        #region Variables

        private static EnergyManager instance;
        [SerializeField] private TextMeshProUGUI savedResourceText;
        [SerializeField] private TextMeshProUGUI surplusText;
        [SerializeField] private float repeatRate = 0.5f;
        private float dividendFor10Seconds;
        private ResourceTypes resourceType = ResourceTypes.Energy;

        #endregion

        #region Properties

        public static EnergyManager Instance
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
