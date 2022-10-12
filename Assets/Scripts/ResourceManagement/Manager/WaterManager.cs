using TMPro;
using UnityEngine;

namespace ResourceManagement.Manager
{
    public class WaterManager : ResourceManager
    {
        #region TODOS

        // use waterScalingFactor

        #endregion

        #region Variables

        private static WaterManager instance;
        [SerializeField] private TextMeshProUGUI savedResourceText;
        [SerializeField] private TextMeshProUGUI surplusText;
        [SerializeField] private float repeatRate = 0.5f;
        private float dividendFor10Seconds;

        private const float waterScalingFactor = 1.6f; //Factor to multiply the demand based off of the current citizen number (Can later be changed into dynamic field to change scaling over time)

        #endregion

        #region Properties

        public static WaterManager Instance
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

        // private float currentWaterValue; //Calculated water production
        // public float CurrentWaterValue
        // {
        //     get => currentWaterValue;
        //     set { currentWaterValue = value; }
        // }
        //
        // private float currentWaterDemand; //combined water consumption of all Citizens
        // public float CurrentWaterDemand
        // {
        //     get => currentWaterDemand;
        //     set { currentWaterDemand = value; }
        // }
        //
        // private float currentWaterProduction; //Water production of every water source combined
        // public float CurrentWaterProduction
        // {
        //     get => currentWaterProduction;
        //     set { currentWaterProduction = value; }
        // }
        //
        // private float savedWaterValue; //No use for now

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

        #region OldSummary

        /// <summary>
        /// Calculation of currentFoodValue
        /// </summary>
        /// <param name="currentProduction">Combined value of all water production sources</param>
        /// <param name="savedValue">Won't be used since there won't be any saving Options like silos</param>
        /// <param name="currentDemand">Combined value of all water consuming sources like modules</param>

        #endregion

        /// <summary>
        /// Calculation of currentWaterSurplus
        /// </summary>
        protected override void CalculateCurrentResourceSurplus()
        {
            CurrentResourceSurplus = CurrentResourceProduction - CurrentResourceDemand;
            surplusText.text = $"{CurrentResourceSurplus}";
        }

        /// <summary>
        /// Calculation of SavedWaterValue every 0.5 seconds
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
                // Kill People???
            }

            savedResourceText.text = $"{(int) SavedResourceValue}/{SaveSpace}";
        }

        #endregion
    }
}