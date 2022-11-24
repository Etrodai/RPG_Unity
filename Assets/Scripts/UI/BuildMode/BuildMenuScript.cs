using System.Collections;
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
    public class Buttons
    {
        public Button button;
        public BuildingResourcesScriptableObject moduleToBuild;
        public BuildingType type;
        public GameObject hoverGameObject;
        private TextMeshProUGUI hoverText;
        private RawImage image;
        private EventTrigger trigger;

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

        public RawImage Image
        {
            get => image;
            set => image = value;
        }
    }
    
    public class BuildMenuScript : MonoBehaviour //Made by Robin
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
        private float currentAlpha;
        [SerializeField] private float lerpTime;
        private Buttons lerpButton;

        #endregion
        
        public Buttons[] Buttons => buttons;

        #region UnityEvents

        /// <summary>
        /// builds BuildMenu
        /// checks, if buttons should be interactable
        /// </summary>
        private void Start()
        {
            if (!isInitialized)
            {
                Initialize();
            }

            InvokeRepeating(nameof(UpdateButtons), 0, 0.5f);
            UpdateButtons();
        }

        #endregion

        #region Methods

        /// <summary>
        /// sets Variables
        /// </summary>
        public void Initialize()
        {
            if (isInitialized) return;
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
                buttons[i].Image = buttons[i].hoverGameObject.GetComponent<RawImage>();
                // buttons[i].HoverText.color = new(buttons[i].HoverText.color.r, buttons[i].HoverText.color.g, buttons[i].HoverText.color.b, 0);
                // buttons[i].Image.color = new(buttons[i].Image.color.r, buttons[i].Image.color.g, buttons[i].Image.color.b, 0);

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
                
                buttons[i].button.gameObject.SetActive(false);
                buttons[i].hoverGameObject.SetActive(false);
            }
            isInitialized = true;
        }

        /// <summary>
        /// checks if buttons should be interactable
        /// </summary>
        private void UpdateButtons()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                Buttons button = buttons[i];

                if (!button.button.gameObject.activeSelf) continue;
                
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
            }
        }

        public void ShowHoverText(GameObject hoverGameObject)
        {
            // foreach (Buttons button in buttons)
            // {
            //     if (button.hoverGameObject == hoverGameObject) lerpButton = button;
            // }
            // StartCoroutine(LerpInHoverText());
            
            hoverGameObject.SetActive(true);
        }

        public void HideHoverText(GameObject hoverGameObject)
        {
            // foreach (Buttons button in buttons)
            // {
            //     if (button.hoverGameObject != hoverGameObject) continue;
            //     StopCoroutine(LerpInHoverText());
            //     StartCoroutine(LerpOutHoverText());
            // }
            
            hoverGameObject.SetActive(false);
        }

        private IEnumerator LerpInHoverText()
        {
            const float startValue = 0;
            var lerpTimeLocal = lerpTime;
            const float endValue = 1;
            Buttons lerpButtonLocal = lerpButton;
            float t = 0;

            yield return new WaitForSeconds(2);
            
            while (t < lerpTime)
            {
                yield return new WaitForEndOfFrame();
                currentAlpha = Mathf.Lerp(startValue, endValue, t / lerpTimeLocal);
                lerpButtonLocal.Image.color = new(lerpButtonLocal.Image.color.r, lerpButtonLocal.Image.color.g, lerpButtonLocal.Image.color.b, currentAlpha);
                lerpButtonLocal.HoverText.color = new(lerpButtonLocal.HoverText.color.r, lerpButtonLocal.HoverText.color.g, lerpButtonLocal.HoverText.color.b, currentAlpha);
                t += Time.deltaTime;
            }
        }

        private IEnumerator LerpOutHoverText()
        {
            
            float t = 0;
            float startValue = currentAlpha;
            float lerpTimeLocal = currentAlpha * lerpTime;
            float endValue = 0;
            Buttons lerpButtonLocal = lerpButton;
            
            while (t < lerpTime)
            {
                yield return new WaitForEndOfFrame();
                currentAlpha = Mathf.Lerp(startValue, endValue, t / lerpTimeLocal);
                lerpButtonLocal.Image.color = new(lerpButtonLocal.Image.color.r, lerpButtonLocal.Image.color.g, lerpButtonLocal.Image.color.b, currentAlpha);
                lerpButtonLocal.HoverText.color = new(lerpButtonLocal.HoverText.color.r, lerpButtonLocal.HoverText.color.g, lerpButtonLocal.HoverText.color.b, currentAlpha);
                t += Time.deltaTime;
            }
        }
        

        #endregion
    }
}