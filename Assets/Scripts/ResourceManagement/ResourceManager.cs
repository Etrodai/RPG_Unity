using UnityEngine;

namespace ResourceManagement
{
    public abstract class ResourceManager : MonoBehaviour
    {
        #region Variables/Properties

        public abstract float CurrentResourceSurplus { get; set; }
        public abstract float CurrentResourceProduction { get; set; }
        public abstract float CurrentResourceDemand { get; set; }
        public abstract float SavedResourceValue { get; set; }
        public abstract float SaveSpace { get; set; }
        public abstract ResourceTypes ResourceType { get; set; }

        #endregion

        #region Methods

        protected abstract void InvokeCalculation();
        protected abstract void CalculateCurrentResourceSurplus();
        protected abstract void CalculateSavedResourceValue();

        #endregion
    }
}
