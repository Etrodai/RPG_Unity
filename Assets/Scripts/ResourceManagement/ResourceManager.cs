using UnityEngine;

namespace ResourceManagement
{
    public abstract class ResourceManager : MonoBehaviour //Made by Robin
    {
        #region Variables/Properties

        public abstract float CurrentResourceSurplus { get; set; }
        public abstract float CurrentResourceProduction { get; set; }
        public abstract float CurrentResourceDemand { get; set; }
        public abstract float SavedResourceValue { get; set; }
        public abstract float SaveSpace { get; set; }
        public abstract ResourceType ResourceType { get; set; }

        #endregion

        #region Methods

        protected abstract void InvokeCalculation();
        
        /// <summary>
        /// Calculation of currentResourceSurplus
        /// </summary>
        protected abstract void CalculateCurrentResourceSurplus();
        
        /// <summary>
        /// Calculation of SavedResourceValue every 0.5 seconds
        /// </summary>
        protected abstract void CalculateSavedResourceValue();

        #endregion
    }
}
