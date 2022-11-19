using System.Collections.Generic;
using Buildings;
using Manager;
using ResourceManagement;
using ResourceManagement.Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.BuildMode
{
    [System.Serializable]
    public struct Buttons
    {
        public Button button;
        public EventTrigger trigger;
        public BuildingResourcesScriptableObject moduleToBuild;
    }
    
    public class BuildMenuScript : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Buttons[] buttons;
        private MaterialManager materialManager;
        private EnergyManager energyManager;
        private FoodManager foodManager;
        private WaterManager waterManager;
        private CitizenManager citizenManager;
        private List<ResourceManager> managers;
        private bool isInitialized;

        #endregion

        #region UnityEvents

        /// <summary>
        /// builds BuildMenu
        /// checks, if buttons should be interactable
        /// </summary>
        private void OnEnable()
        {
            if (!isInitialized)
            {
                Initialize();
                isInitialized = true;
            }
            
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].trigger = buttons[i].button.GetComponent<EventTrigger>();
            }
            
            InvokeRepeating(nameof(UpdateButtons), 0, 0.5f);
            UpdateButtons();
        }

        #endregion

        #region Methods

        /// <summary>
        /// sets Variables
        /// </summary>
        private void Initialize()
        {
            managers = new();
            materialManager = MainManagerSingleton.Instance.MaterialManager;
            managers.Add(materialManager);
            energyManager = MainManagerSingleton.Instance.EnergyManager;
            managers.Add(energyManager);
            foodManager = MainManagerSingleton.Instance.FoodManager;
            managers.Add(foodManager);
            waterManager = MainManagerSingleton.Instance.WaterManager;
            managers.Add(waterManager);
            citizenManager = MainManagerSingleton.Instance.CitizenManager;
            managers.Add(citizenManager);
        }

        /// <summary>
        /// checks if buttons should be interactable
        /// </summary>
        private void UpdateButtons()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                Buttons button = buttons[i];
                bool activate = true;
                for (int j = 0; j < button.moduleToBuild.Costs.Length; j++)
                {
                    Resource cost = button.moduleToBuild.Costs[j];
                    for (int k = 0; k < managers.Count; k++)
                    {
                        ResourceManager manager = managers[k];
                        if (manager.ResourceType != cost.resource) continue;
                        if (manager.SavedResourceValue < cost.value) activate = false;
                    }
                }

                button.button.interactable = activate;
                button.trigger.enabled = activate;
            }
        }

        #endregion
    }
}