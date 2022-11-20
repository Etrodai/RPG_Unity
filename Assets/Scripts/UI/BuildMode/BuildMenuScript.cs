using System.Collections.Generic;
using Buildings;
using Manager;
using ResourceManagement;
using ResourceManagement.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.BuildMode
{
    [System.Serializable]
    public struct Buttons
    {
        public Button button;
        private EventTrigger trigger;
        public BuildingResourcesScriptableObject moduleToBuild;
        public GameObject hoverGameObject;
        private TextMeshProUGUI hoverText;
        
        public EventTrigger Trigger
        { 
            get => trigger;
            set => trigger = value;
        }    
        
        public TextMeshProUGUI HoverText
        { 
            get => hoverText;
            set => hoverText = value;
        }
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

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Trigger = buttons[i].button.GetComponent<EventTrigger>();
                buttons[i].HoverText = buttons[i].hoverGameObject.GetComponentInChildren<TextMeshProUGUI>();
                buttons[i].hoverGameObject.SetActive(false);

                string text = "";
                string resourceText = "";
                if (buttons[i].moduleToBuild.Costs.Length != 0)
                {
                    text += "Kosten:\n";
                    for (int j = 0; j < buttons[i].moduleToBuild.Costs.Length; j++)
                    {
                        Resource resourcesCost = buttons[i].moduleToBuild.Costs[j];
                        switch (resourcesCost.resource)
                        {
                            case ResourceType.Material:
                                resourceText = "Mineralien";
                                break;
                            case ResourceType.Energy:
                                resourceText = "Strom";
                                break;
                            case ResourceType.Citizen:
                                resourceText = "Arbeiter";
                                break;
                            case ResourceType.Food:
                                resourceText = "Nahrung";
                                break;
                            case ResourceType.Water:
                                resourceText = "Wasser";
                                break;
                        }
                        text += $"{resourcesCost.value} {resourceText}\n";
                    }

                    text += "\n";
                }
                
                if (buttons[i].moduleToBuild.Consumption.Length != 0)
                {
                    text += "Benötigt:\n";
                    string workerText = "";
                    for (int j = 0; j < buttons[i].moduleToBuild.Consumption.Length; j++)
                    {
                        Resource resourcesConsumption = buttons[i].moduleToBuild.Consumption[j];
                        switch (resourcesConsumption.resource)
                        {
                            case ResourceType.Material:
                                resourceText = "Mineralien";
                                break;
                            case ResourceType.Energy:
                                resourceText = "Strom";
                                break;
                            case ResourceType.Citizen:
                                resourceText = "Arbeiter";
                                break;
                            case ResourceType.Food:
                                resourceText = "Nahrung";
                                break;
                            case ResourceType.Water:
                                resourceText = "Wasser";
                                break;
                        }
                        
                        if (resourcesConsumption.resource == ResourceType.Citizen)
                        {
                            workerText +=$"{resourcesConsumption.value} {resourceText}\n";
                        }
                        else
                        {
                            text += $"{resourcesConsumption.value} {resourceText}/10 Sek\n";
                        }
                    }

                    text += workerText;
                    text += "\n";
                }
                
                if (buttons[i].moduleToBuild.Production.Length != 0)
                {
                    text += "Produziert:\n";
                    for (int j = 0; j < buttons[i].moduleToBuild.Production.Length; j++)
                    {
                        Resource resourcesProduction = buttons[i].moduleToBuild.Production[j];
                        switch (resourcesProduction.resource)
                        {
                            case ResourceType.Material:
                                resourceText = "Mineralien";
                                break;
                            case ResourceType.Energy:
                                resourceText = "Strom";
                                break;
                            case ResourceType.Citizen:
                                resourceText = "Arbeiter";
                                break;
                            case ResourceType.Food:
                                resourceText = "Nahrung";
                                break;
                            case ResourceType.Water:
                                resourceText = "Wasser";
                                break;
                        }
                        text += $"{resourcesProduction.value} {resourceText}/10 Sek\n";
                    }

                    text += "\n";
                }
                
                if (buttons[i].moduleToBuild.SaveSpace.Length != 0)
                {
                    text += "Lagerplatz:\n";
                    for (int j = 0; j < buttons[i].moduleToBuild.SaveSpace.Length; j++)
                    {
                        Resource resourcesSaveSpace = buttons[i].moduleToBuild.SaveSpace[j];
                        switch (resourcesSaveSpace.resource)
                        {
                            case ResourceType.Material:
                                resourceText = "Mineralien";
                                break;
                            case ResourceType.Energy:
                                resourceText = "Strom";
                                break;
                            case ResourceType.Citizen:
                                resourceText = "Arbeiter";
                                break;
                            case ResourceType.Food:
                                resourceText = "Nahrung";
                                break;
                            case ResourceType.Water:
                                resourceText = "Wasser";
                                break;
                        }
                        text += $"{resourcesSaveSpace.value} {resourceText}\n";
                    }

                    text += "\n";
                }
                
                buttons[i].HoverText.text = text;
                // buttons[i].trigger.AddListener(EventTriggerType.PointerEnter, _ => ShowHoverText(buttons[i].hoverGameObject));
                // buttons[i].trigger.AddListener(EventTriggerType.PointerExit, _ => HideHoverText(buttons[i].hoverGameObject));
            }
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
                // button.trigger.enabled = activate;
            }
        }

        public void ShowHoverText(GameObject hoverGameObject)
        {
            hoverGameObject.SetActive(true);
        }

        public void HideHoverText(GameObject hoverGameObject)
        {
            hoverGameObject.SetActive(false);
        }

        #endregion
    }
}