using System.Collections.Generic;
using System.IO;
using Manager;
using ResourceManagement;
using ResourceManagement.Manager;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MilestoneSystem
{
    [System.Serializable]
    public struct MileStoneSystemSave
    {
        public int mileStonesDone;
        public bool isDone;
    }
    
    public class MileStoneSystem : MonoBehaviour
    {
        #region Variables

        private MileStoneEvent[] events;
        [SerializeField] private MileStonesScriptableObject[] mileStones;
        private int mileStonesDone; // Counter of Milestones Done
        [SerializeField] private GameObject mainText; // Full Screen Text what's happening
        private int textIndex; // Counter of Texts Shown
        private bool isDone; // Shows, if a Milestone is done, and the end-text can be shown
        private TextMeshProUGUI mileStoneText; // TextField of MainText
        [SerializeField] private GameObject mileStonePanel; // SideMenu which always is there and can be mini and maximized
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private GameObject labelPrefab;
        private readonly List<GameObject> allLabels = new();
        private readonly List<GameObject> allItems = new();
        private MaterialManager materialManager;
        private EnergyManager energyManager;
        private FoodManager foodManager;
        private WaterManager waterManager;
        private CitizenManager citizenManager;
        private GameManager gameManager;
        private List<ResourceManager> managers;
        [SerializeField] private Button sideMenuMileStoneButton;
        [SerializeField] private Button sideMenuPriorityButton;
        private const string saveName = "MileStoneSystem";

        private bool menuWasBuild;

        #endregion

        #region Events

        public UnityEvent onSideMenuShouldClose;

        #endregion
        
        #region UnityEvents
        
        /// <summary>
        /// sets variables
        /// </summary>
        private void Awake()
        {
            mileStoneText = mainText.GetComponentInChildren<TextMeshProUGUI>();
            events = GetComponents<MileStoneEvent>();
        }

        /// <summary>
        /// Builds PreMainText of first Milestone
        /// </summary>
        private void Start()
        {
            managers = new List<ResourceManager>();
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
            
            gameManager = MainManagerSingleton.Instance.GameManager;
            
            Save.OnSaveButtonClick.AddListener(SaveData);
            Save.OnSaveAsButtonClick.AddListener(SaveDataAs);
            Load.OnLoadButtonClick.AddListener(LoadData);
            
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
                foreach (GameObject item in allItems)
                {
                    Destroy(item);
                }
                allItems.Clear();
                foreach (GameObject item in allLabels)
                {
                    Destroy(item);
                }
                allLabels.Clear();
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
        
        #endregion

        #region Save Load

        private void SaveData()
        {
            MileStoneSystemSave[] data = new MileStoneSystemSave[1];
            data[0].isDone = isDone;
            data[0].mileStonesDone = mileStonesDone;

            Save.AutoSaveData(data, saveName);
        }
    
        private void SaveDataAs(string savePlace)
        {
            MileStoneSystemSave[] data = new MileStoneSystemSave[1];
            data[0].isDone = isDone;
            data[0].mileStonesDone = mileStonesDone;
        
            Save.SaveDataAs(savePlace, data, saveName);
        }
    
        private void LoadData(string path)
        {
            path = Path.Combine(path, saveName);

            MileStoneSystemSave[] data = Load.LoadData(path) as MileStoneSystemSave[];
            
            isDone = data[0].isDone;
            mileStonesDone = data[0].mileStonesDone;
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
                onSideMenuShouldClose.Invoke();
                // Debug.Log("onSideMenuShouldClose.Invoke()");
                mainText.SetActive(true);
                sideMenuMileStoneButton.interactable = false;
                sideMenuPriorityButton.interactable = false;
                Time.timeScale = 0;
                mileStoneText.text = mileStones[mileStonesDone].MileStoneText[textIndex];
                textIndex++;
            }
            else
            {
                mainText.SetActive(false);
                sideMenuMileStoneButton.interactable = true;
                sideMenuPriorityButton.interactable = true;
                BuildMenu();
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
                onSideMenuShouldClose.Invoke();
                // Debug.Log("onSideMenuShouldClose.Invoke()");
                mainText.SetActive(true);
                sideMenuMileStoneButton.interactable = false;
                sideMenuPriorityButton.interactable = false;
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
                GameObject item = Instantiate(labelPrefab, mileStonePanel.transform, true);
                allLabels.Add(item);
                item.transform.localScale = Vector3.one;
                TextMeshProUGUI text = item.GetComponent<TextMeshProUGUI>();
                text.text = "ToDos";
                foreach (MileStoneEventNames requiredEvent in mileStones[mileStonesDone].RequiredEvent)
                {
                    foreach (MileStoneEvent mileStoneEvent in events)
                    {
                        if (mileStoneEvent.Name == requiredEvent)
                        {
                            foreach (MileStoneEventItems eventItems in mileStoneEvent.Events)
                            {
                                item = Instantiate(itemPrefab, mileStonePanel.transform, true);
                                allItems.Add(item);
                                item.transform.localScale = Vector3.one;
                                text = item.GetComponentInChildren<TextMeshProUGUI>();
                                text.text = eventItems.text;
                            }
                            mileStoneEvent.enabled = true;
                            mileStoneEvent.ResetAll();
                        }
                    }
                }

                menuWasBuild = true;
            }

            if (mileStones[mileStonesDone].RequiredResources.Length != 0)
            {
                GameObject item = Instantiate(labelPrefab, mileStonePanel.transform, true);
                allLabels.Add(item);
                item.transform.localScale = Vector3.one;
                TextMeshProUGUI text = item.GetComponent<TextMeshProUGUI>();
                text.text = "Required resources";
                foreach (Resource requiredResource in mileStones[mileStonesDone].RequiredResources)
                {
                    item = Instantiate(itemPrefab, mileStonePanel.transform, true);
                    allItems.Add(item);
                    item.transform.localScale = Vector3.one;
                    text = item.GetComponentInChildren<TextMeshProUGUI>();
                    text.text += $"{requiredResource.value} {requiredResource.resource}";
                }
            }

            if (mileStones[mileStonesDone].RequiredModules.Length != 0)
            {
                GameObject item = Instantiate(labelPrefab, mileStonePanel.transform, true);
                allLabels.Add(item);
                item.transform.localScale = Vector3.one;
                TextMeshProUGUI text = item.GetComponent<TextMeshProUGUI>();
                text.text = "Required modules";
                foreach (MileStoneModules requiredModule in mileStones[mileStonesDone].RequiredModules)
                {
                    item = Instantiate(itemPrefab, mileStonePanel.transform, true);
                    allItems.Add(item);
                    item.transform.localScale = Vector3.one;
                    text = item.GetComponentInChildren<TextMeshProUGUI>();
                    text.text += $"{requiredModule.value} {requiredModule.buildingTypes}";
                }
            }
        }

        #endregion

        #region CheckIfAchieved
        
        /// <summary>
        /// checks if all required events, resources and buildings are achieved
        /// </summary>
        /// <returns>if all required things are achieved</returns>
        private bool CheckIfAchieved()
        {
            if (!menuWasBuild)
            {
                return false;
            }
            bool hasAllRequiredStuff = true;
            int index = 0;

            if (mileStones[mileStonesDone].RequiredEvent.Length != 0)
            {
                foreach (MileStoneEventNames requiredEvent in mileStones[mileStonesDone].RequiredEvent)
                {
                    foreach (MileStoneEvent mileStoneEvent in events)
                    {
                        if (mileStoneEvent.Name == requiredEvent)
                        {
                            if (!mileStoneEvent.CheckAchieved(index))
                            {
                                hasAllRequiredStuff = false;
                            }
                            else
                            {
                                Toggle toggle = allItems[index].GetComponentInChildren<Toggle>();
                                toggle.isOn = true;
                            }

                            index++;
                        }
                    }
                }
            }

            foreach (Resource requiredResource in mileStones[mileStonesDone].RequiredResources)
            {
                foreach (ResourceManager manager in managers)
                {
                    if (requiredResource.resource == manager.ResourceType)
                    {
                        if (manager.SavedResourceValue < requiredResource.value)
                        {
                            hasAllRequiredStuff = false;
                        }
                        else
                        {
                            Toggle toggle = allItems[index].GetComponentInChildren<Toggle>();
                            toggle.isOn = true;
                        }

                        index++;
                    }
                }
                
                // switch (requiredResource.resource)
                // {
                //     case ResourceTypes.Material:
                //         if (MaterialManager.Instance.SavedResourceValue < requiredResource.value) hasAllRequiredStuff = false;
                //         break;
                //     case ResourceTypes.Energy:
                //         if (EnergyManager.Instance.SavedResourceValue < requiredResource.value) hasAllRequiredStuff = false;
                //         break;
                //     case ResourceTypes.Citizen:
                //         if (CitizenManager.Instance.CurrentResourceProduction < requiredResource.value) hasAllRequiredStuff = false;
                //         break;
                //     case ResourceTypes.Food:
                //         if (FoodManager.Instance.SavedResourceValue < requiredResource.value) hasAllRequiredStuff = false;
                //         break;
                //     case ResourceTypes.Water:
                //         if (WaterManager.Instance.SavedResourceValue < requiredResource.value) hasAllRequiredStuff = false;
                //         break;
                // }
            }

            foreach (MileStoneModules requiredModule in mileStones[mileStonesDone].RequiredModules)
            {
                if (gameManager.GetBuildingCount(requiredModule.buildingTypes) < requiredModule.value)
                {
                    hasAllRequiredStuff = false;
                }
                else
                {
                    Toggle toggle = allItems[index].gameObject.GetComponentInChildren<Toggle>();
                    toggle.isOn = true;
                }

                index++;

                // switch (requiredModule.buildingTypes)
                // {
                //     case BuildingTypes.All:
                //         if (GameManager.Instance.GetBuildingCount(requiredModule.buildingTypes) < requiredModule.value) 
                //             hasAllRequiredStuff = false;
                //         break;
                //     case BuildingTypes.CitizenSave:
                //         if (GameManager.Instance.GetBuildingCount(requiredModule.buildingTypes) < requiredModule.value)
                //             hasAllRequiredStuff = false;
                //         break;
                //     case BuildingTypes.EnergyGain:
                //         if (GameManager.Instance.GetBuildingCount(requiredModule.buildingTypes) < requiredModule.value)
                //             hasAllRequiredStuff = false;
                //         break;
                //     case BuildingTypes.EnergySave:
                //         if (GameManager.Instance.GetBuildingCount(requiredModule.buildingTypes) < requiredModule.value)
                //             hasAllRequiredStuff = false;
                //         break;
                //     case BuildingTypes.MaterialGain:
                //         if (GameManager.Instance.GetBuildingCount(requiredModule.buildingTypes) < requiredModule.value)
                //             hasAllRequiredStuff = false;
                //         break;
                //     case BuildingTypes.MaterialSave:
                //         if (GameManager.Instance.GetBuildingCount(requiredModule.buildingTypes) < requiredModule.value)
                //             hasAllRequiredStuff = false;
                //         break;
                //     case BuildingTypes.LifeSupportGain:
                //         if (GameManager.Instance.GetBuildingCount(requiredModule.buildingTypes) < requiredModule.value)
                //             hasAllRequiredStuff = false;
                //         break;
                //     case BuildingTypes.LifeSupportSave:
                //         if (GameManager.Instance.GetBuildingCount(requiredModule.buildingTypes) < requiredModule.value)
                //             hasAllRequiredStuff = false;
                //         break;
                // }
            }

            if (!hasAllRequiredStuff) return false;
            {
                foreach (MileStoneEventNames requiredEvent in mileStones[mileStonesDone].RequiredEvent)
                {
                    foreach (MileStoneEvent mileStoneEvent in events)
                    {
                        if (mileStoneEvent.Name == requiredEvent)
                        {
                            mileStoneEvent.enabled = false;
                        }
                    }
                    
                    // switch (requiredEvent)
                    // {
                    //     case MileStoneEventNames.CameraMovement:
                    //         foreach (MileStoneEvent jtem in events)
                    //         {
                    //             if (jtem.Name == MileStoneEventNames.CameraMovement)
                    //             {
                    //                 jtem.enabled = false;
                    //             }
                    //         }
                    //         break;
                    //     case MileStoneEventNames.ShowPrioritySystem:
                    //         foreach (MileStoneEvent jtem in events)
                    //         {
                    //             if (jtem.Name == MileStoneEventNames.ShowPrioritySystem)
                    //             {
                    //                 jtem.enabled = false;
                    //             }
                    //         }
                    //         break;
                    //     case MileStoneEventNames.WaitForSeconds:
                    //         foreach (MileStoneEvent jtem in events)
                    //         {
                    //             if (jtem.Name == MileStoneEventNames.WaitForSeconds)
                    //             {
                    //                 jtem.enabled = false;
                    //             }
                    //         }
                    //         break;
                    // }
                }
            }

            return true;
        }
        
        #endregion

        #endregion
    }
}
