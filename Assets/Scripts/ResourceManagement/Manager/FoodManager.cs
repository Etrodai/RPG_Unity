using TMPro;
using UnityEngine;

namespace ResourceManagement.Manager
{
    public class FoodManager : ResourceManager
    {
        #region TODOS

        // use foodScalingFactor

        #endregion

        #region Variables

        private static FoodManager instance;
        [SerializeField] private TextMeshProUGUI savedResourceText;
        [SerializeField] private TextMeshProUGUI surplusText;
        [SerializeField] private float repeatRate = 0.5f;
        private float dividendFor10Seconds;

        private const float foodScalingFactor = 1.25f; //Factor to multiply the demand based off of the current citizen number (Can later be changed into dynamic field to change scaling over time)

        #endregion

        #region Properties

        public static FoodManager Instance
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

        // private float currentFoodValue; //Calculated food production
        // public float CurrentFoodValue
        // {
        //     get => currentFoodValue;
        //     set { currentFoodValue = value; }
        // }
        //
        // private float currentFoodDemand; //combined food consumption of all Citizens
        // public float CurrentFoodDemand
        // {
        //     get => currentFoodDemand;
        //     set { currentFoodDemand = value; }
        // }
        //
        // private float currentFoodProduction; //Food production of every food source combined
        // public float CurrentFoodProduction
        // {
        //     get => currentFoodProduction;
        //     set { currentFoodProduction = value; }
        // }
        //
        // private float savedFoodValue; //No use for now

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
        /// <param name="currentProduction">Combined value of all food production sources</param>
        /// <param name="savedValue">Won't be used since there won't be any saving Options like silos</param>
        /// <param name="currentDemand">Combined value of all food consuming sources like modules</param>

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
                SavedResourceValue = 0;
                // Kill People???
            }

            savedResourceText.text = $"{(int) SavedResourceValue}/{SaveSpace}";
        }

        #endregion
    }
}