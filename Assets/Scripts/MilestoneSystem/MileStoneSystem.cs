using System.Collections.Generic;
using Buildings;
using ResourceManagement;
using ResourceManagement.Manager;
using TMPro;
using UnityEngine;

namespace MilestoneSystem
{
    public class MileStoneSystem : MonoBehaviour
    {
        #region TODOS
        
        // Text springt weg, ohne auf OK zu drücken
        
        #endregion

        #region Variables

        [SerializeField] private List<MileStoneEvent> events;
        [SerializeField] private MileStonesScriptableObject[] mileStones;
        private int mileStonesDone; // Counter of Milestones Done
        [SerializeField] private GameObject mainText; // Full Screen Text what's happening
        private int textIndex; // Counter of Texts Shown
        private bool isDone; // Shows, if a Milestone is done, and the end-text can be shown
        private TextMeshProUGUI mileStoneText; // TextField of MainText
        [SerializeField] private GameObject menu; // SideMenu which always is there and can be mini and maximized
        private TextMeshProUGUI requiredStuffText; // TextField of Menu
        bool isMinimized; // shows if the SideMenu is mini or maximized
        [SerializeField] private Canvas canvas;
    
        #endregion

        public List<MileStoneEvent> Events
        {
            get => events;
            set => events = value;
        }
        
        #region UnityEvents
        
        /// <summary>
        /// sets variables
        /// </summary>
        private void Awake()
        {
            mileStoneText = mainText.GetComponentInChildren<TextMeshProUGUI>();
            requiredStuffText = menu.GetComponentInChildren<TextMeshProUGUI>();
        }

        /// <summary>
        /// Builds PreMainText of first Milestone
        /// </summary>
        private void Start()
        {
            BuildPreMainText();
        }

        /// <summary>
        /// checks if the Milestone is achieved
        /// if it is, builds PostMainText of this Milestone
        /// </summary>
        private void Update()
        {
            if (CheckIfAchieved())
            {
                isDone = true;
                if (textIndex == 0) BuildPostMainText();
            }
        }
        
        #endregion

        #region ClickEvents
        
        /// <summary>
        /// starts next MainText
        /// </summary>
        public void OnClickOKButton()
        {
            if (isDone) BuildPostMainText();
            else BuildPreMainText();
        }

        /// <summary>
        /// mini- or maximizes MileStoneMenu
        /// </summary>
        public void OnClickMenuButton()
        {
            if (isMinimized) OpenMenu();
            else CloseMenu();
        }
        
        #endregion

        #region Methods
        
        #region MainText
        
        /// <summary>
        /// when new MileStone starts, it builds and shows its PreMainText and stops the game
        /// after all Texts are shown, the game goes on and the MainTextMenu gets closed
        /// </summary>
        private void BuildPreMainText()
        {
            if (textIndex < mileStones[mileStonesDone].MileStoneText.Length)
            {
                if (!isMinimized) CloseMenu();
                mainText.SetActive(true);
                Time.timeScale = 0;
                mileStoneText.text = mileStones[mileStonesDone].MileStoneText[textIndex];
                textIndex++;
            }
            else
            {
                mainText.SetActive(false);
                BuildMenu();
                if (isMinimized) OpenMenu();
                Time.timeScale = 1;
                textIndex = 0;
            }
        }

        /// <summary>
        /// when the curren MileStone is achieved, it builds and shows its PostMainText and stops the game
        /// after all Texts are shown, the game goes on and the MainTextMenu gets closed
        /// </summary>
        private void BuildPostMainText()
        {
            if (textIndex < mileStones[mileStonesDone].MileStoneAchievedText.Length)
            {
                if (!isMinimized) CloseMenu();
                mainText.SetActive(true);
                Time.timeScale = 0;
                mileStoneText.text = mileStones[mileStonesDone].MileStoneAchievedText[textIndex];
                textIndex++;
            }
            else
            {
                textIndex = 0;
                isDone = false;
                mileStonesDone++;
                BuildPreMainText();
            }
        }
        
        #endregion

        #region Menu
        
        /// <summary>
        /// when a Milestone is achieved it builds a new MilestoneMenu
        /// </summary>
        private void BuildMenu()
        {
            if (mileStones[mileStonesDone].RequiredEvent.Length != 0)
            {
                foreach (MileStoneEventNames item in mileStones[mileStonesDone].RequiredEvent)
                {
                    switch (item)
                    {
                        case MileStoneEventNames.CameraMovement:
                            foreach (MileStoneEvent jtem in events)
                            {
                                if (jtem.Name == MileStoneEventNames.CameraMovement)
                                {
                                    requiredStuffText.text += $"{jtem.MenuText}\n\n";
                                    jtem.enabled = true;
                                    jtem.ResetAll();
                                }
                            }
                            break;
                        case MileStoneEventNames.ShowPrioritySystem:
                            foreach (MileStoneEvent jtem in events)
                            {
                                if (jtem.Name == MileStoneEventNames.ShowPrioritySystem)
                                {
                                    requiredStuffText.text += $"{jtem.MenuText}\n\n";
                                    jtem.enabled = true;
                                    jtem.ResetAll();
                                }
                            }
                            break;
                        case MileStoneEventNames.WaitForSeconds:
                            foreach (MileStoneEvent jtem in events)
                            {
                                if (jtem.Name == MileStoneEventNames.WaitForSeconds)
                                {
                                    requiredStuffText.text += $"{jtem.MenuText}\n\n";
                                    jtem.enabled = true;
                                    jtem.ResetAll();
                                }
                            }
                            break;
                    }
                }
            }

            if (mileStones[mileStonesDone].RequiredResources.Length != 0)
            {
                requiredStuffText.text += "\nRequired resources:";
                foreach (Resource item in mileStones[mileStonesDone].RequiredResources)
                {
                    requiredStuffText.text += $"\n{item.value} {item.resource}\n";
                }
            }

            if (mileStones[mileStonesDone].RequiredModules.Length != 0)
            {
                requiredStuffText.text += "\nRequired modules:";
                foreach (MileStoneModules item in mileStones[mileStonesDone].RequiredModules)
                {
                    requiredStuffText.text += $"\n{item.value} {item.buildingTypes}\n";
                }
            }
        }

        /// <summary>
        /// maximizes menu by moving it to the left side
        /// </summary>
        private void OpenMenu()
        {
            var transformPosition = menu.transform.position;
            transformPosition.x -= 300 * canvas.scaleFactor;
            menu.transform.position = transformPosition;
            isMinimized = false;
        }

        /// <summary>
        /// minimizes menu by moving it to the right side
        /// </summary>
        private void CloseMenu()
        {
            var transformPosition = menu.transform.position;
            transformPosition.x += 300 * canvas.scaleFactor;
            menu.transform.position = transformPosition;
            isMinimized = true;
        }
        
        #endregion

        #region CheckIfAchieved
        
        /// <summary>
        /// checks if all required events, resources and buildings are achieved
        /// </summary>
        /// <returns>if all required things are achieved</returns>
        private bool CheckIfAchieved()
        {
            bool hasAllRequiredStuff = true;

            if (mileStones[mileStonesDone].RequiredEvent.Length != 0)
            {
                foreach (MileStoneEventNames item in mileStones[mileStonesDone].RequiredEvent)
                {
                    switch (item)
                    {
                        case MileStoneEventNames.CameraMovement:
                            foreach (MileStoneEvent jtem in events)
                            {
                                if (jtem.Name == MileStoneEventNames.CameraMovement)
                                {
                                    if (!jtem.CheckAchieved())
                                    {
                                        hasAllRequiredStuff = false;
                                    }
                                }
                            }
                            break;
                        case MileStoneEventNames.ShowPrioritySystem:
                            foreach (MileStoneEvent jtem in events)
                            {
                                if (jtem.Name == MileStoneEventNames.ShowPrioritySystem)
                                {
                                    if (!jtem.CheckAchieved())
                                    {
                                        hasAllRequiredStuff = false;
                                    }
                                }
                            }
                            break;
                        case MileStoneEventNames.WaitForSeconds:
                            foreach (MileStoneEvent jtem in events)
                            {
                                if (jtem.Name == MileStoneEventNames.WaitForSeconds)
                                {
                                    if (!jtem.CheckAchieved())
                                    {
                                        hasAllRequiredStuff = false;
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            foreach (Resource item in mileStones[mileStonesDone].RequiredResources)
            {
                switch (item.resource)
                {
                    case ResourceTypes.Material:
                        if (MaterialManager.Instance.SavedResourceValue < item.value) hasAllRequiredStuff = false;
                        break;
                    case ResourceTypes.Energy:
                        if (EnergyManager.Instance.SavedResourceValue < item.value) hasAllRequiredStuff = false;
                        break;
                    case ResourceTypes.Citizen:
                        if (CitizenManager.Instance.Citizen < item.value) hasAllRequiredStuff = false;
                        break;
                    case ResourceTypes.Food:
                        if (FoodManager.Instance.SavedResourceValue < item.value) hasAllRequiredStuff = false;
                        break;
                    case ResourceTypes.Water:
                        if (WaterManager.Instance.SavedResourceValue < item.value) hasAllRequiredStuff = false;
                        break;
                }
            }

            foreach (MileStoneModules item in mileStones[mileStonesDone].RequiredModules)
            {
                switch (item.buildingTypes)
                {
                    case BuildingTypes.All:
                        if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value) 
                            hasAllRequiredStuff = false;
                        break;
                    case BuildingTypes.CitizenSave:
                        if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value)
                            hasAllRequiredStuff = false;
                        break;
                    case BuildingTypes.EnergyGain:
                        if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value)
                            hasAllRequiredStuff = false;
                        break;
                    case BuildingTypes.EnergySave:
                        if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value)
                            hasAllRequiredStuff = false;
                        break;
                    case BuildingTypes.MaterialGain:
                        if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value)
                            hasAllRequiredStuff = false;
                        break;
                    case BuildingTypes.MaterialSave:
                        if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value)
                            hasAllRequiredStuff = false;
                        break;
                    case BuildingTypes.LifeSupportGain:
                        if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value)
                            hasAllRequiredStuff = false;
                        break;
                    case BuildingTypes.LifeSupportSave:
                        if (GameManager.Instance.GetBuildingCount(item.buildingTypes) < item.value)
                            hasAllRequiredStuff = false;
                        break;
                }
            }

            if (hasAllRequiredStuff)
            {
                foreach (MileStoneEventNames item in mileStones[mileStonesDone].RequiredEvent)
                {
                    switch (item)
                    {
                        case MileStoneEventNames.CameraMovement:
                            foreach (MileStoneEvent jtem in events)
                            {
                                if (jtem.Name == MileStoneEventNames.CameraMovement)
                                {
                                    jtem.enabled = false;
                                }
                            }
                            break;
                        case MileStoneEventNames.ShowPrioritySystem:
                            foreach (MileStoneEvent jtem in events)
                            {
                                if (jtem.Name == MileStoneEventNames.ShowPrioritySystem)
                                {
                                    jtem.enabled = false;
                                }
                            }
                            break;
                        case MileStoneEventNames.WaitForSeconds:
                            foreach (MileStoneEvent jtem in events)
                            {
                                if (jtem.Name == MileStoneEventNames.WaitForSeconds)
                                {
                                    jtem.enabled = false;
                                }
                            }
                            break;
                    }
                }
            }

            return hasAllRequiredStuff;
        }
        
        #endregion

        #endregion
    }
}
