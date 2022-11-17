using System.Collections.Generic;
using Buildings;
using Manager;
using ResourceManagement;
using ResourceManagement.Manager;
using SaveSystem;
using Sound;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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
        private readonly List<Toggle> itemToggles = new();
        private MaterialManager materialManager;
        private EnergyManager energyManager;
        private FoodManager foodManager;
        private WaterManager waterManager;
        private CitizenManager citizenManager;
        private GameManager gameManager;
        private List<ResourceManager> managers;
        [SerializeField] private Button sideMenuMileStoneButton;
        private EventTrigger sideMenuMileStoneButtonTrigger;
        [SerializeField] private Button sideMenuPriorityButton;
        private EventTrigger sideMenuPriorityButtonTrigger;

        private float timer;
        [SerializeField] private float maxTimer = 0.5f;

        private bool menuWasBuild;
        private bool showText;

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
            sideMenuPriorityButtonTrigger = sideMenuPriorityButton.GetComponent<EventTrigger>();
            sideMenuMileStoneButtonTrigger = sideMenuMileStoneButton.GetComponent<EventTrigger>();
            BuildPreMainText();

            timer = maxTimer;
        }

        /// <summary>
        /// checks if the Milestone is achieved
        /// if it is, builds PostMainText of this Milestone
        /// </summary>
        private void Update()
        {
            timer -= Time.deltaTime;
            
            if (!(timer < 0)) return;
            timer = maxTimer;
            if (Time.timeScale == 0) return;
            if (!CheckIfAchieved()) return;
            
            isDone = true;
            
            foreach (GameObject item in allItems) Destroy(item);
            foreach (GameObject item in allLabels) Destroy(item);
            
            allItems.Clear();
            itemToggles.Clear();
            allLabels.Clear();
            
            if (textIndex == 0) BuildPostMainText();
        }

        private void OnDestroy()
        {
            Save.OnSaveButtonClick.RemoveListener(SaveData);
            Save.OnSaveAsButtonClick.RemoveListener(SaveDataAs);
            Load.OnLoadButtonClick.RemoveListener(LoadData);
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

        private void SaveData(SaveLoadInvoker invoker)
        {
            MileStoneSystemSave data;
            data.isDone = isDone;
            data.mileStonesDone = mileStonesDone;

            invoker.GameSave.mileStoneData = data;
            // Save.AutoSaveData(data, SaveName);
        }
    
        private void SaveDataAs(string savePlace, SaveLoadInvoker invoker)
        {
            MileStoneSystemSave data;
            data.isDone = isDone;
            data.mileStonesDone = mileStonesDone;

            invoker.GameSave.mileStoneData = data;
            // Save.SaveDataAs(savePlace, data, SaveName);
        }
    
        private void LoadData(GameSave gameSave)
        {
            // path = Path.Combine(path, $"{SaveName}.dat");
            // if (!File.Exists(path)) return;
            //
            // MileStoneSystemSave[] data = Load.LoadData(path) as MileStoneSystemSave[];

            MileStoneSystemSave data = gameSave.mileStoneData;
            
            // if (data == null) return;
            
            isDone = data.isDone;
            mileStonesDone = data.mileStonesDone;
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
                sideMenuMileStoneButtonTrigger.enabled = false;
                sideMenuPriorityButton.interactable = false;
                sideMenuPriorityButtonTrigger.enabled = false;

                Time.timeScale = 0;
                mileStoneText.text = mileStones[mileStonesDone].MileStoneText[textIndex];
                textIndex++;
            }
            else
            {
                mainText.SetActive(false);
                sideMenuMileStoneButton.interactable = true;
                sideMenuMileStoneButtonTrigger.enabled = true;
                sideMenuPriorityButton.interactable = true;
                sideMenuPriorityButtonTrigger.enabled = true;
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
                sideMenuMileStoneButtonTrigger.enabled = true;
                sideMenuPriorityButton.interactable = false;
                sideMenuPriorityButtonTrigger.enabled = false;
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
        private void BuildMenu() // TODO: (Robin) Viele getComponents, evtl iwo zwischenspeichern?
        {
            if (mileStones[mileStonesDone].RequiredEvent.Length != 0)
            {
                GameObject item = Instantiate(labelPrefab, mileStonePanel.transform, true);
                allLabels.Add(item);
                item.transform.localScale = Vector3.one;
                TextMeshProUGUI text = item.GetComponent<TextMeshProUGUI>();
                text.text = "ToDos";
                showText = false;
                foreach (MileStoneEventNames requiredEvent in mileStones[mileStonesDone].RequiredEvent)
                {
                    foreach (MileStoneEvent mileStoneEvent in events)
                    {
                        if (mileStoneEvent.Name == requiredEvent)
                        {
                            for (int i = 0; i < mileStoneEvent.Events.Length; i++)
                            {
                                item = Instantiate(itemPrefab, mileStonePanel.transform, true);
                                allItems.Add(item);
                                itemToggles.Add(item.GetComponentInChildren<Toggle>());
                                item.transform.localScale = Vector3.one;
                                text = item.GetComponentInChildren<TextMeshProUGUI>();
                                text.text = mileStoneEvent.Events[i].text;
                                if (!string.IsNullOrEmpty(text.text))
                                {
                                    showText = true;
                                }
                            }

                            mileStoneEvent.enabled = true;
                            mileStoneEvent.ResetAll();
                        }
                    }
                }

                if (!showText)
                {
                    foreach (GameObject allLabel in allLabels)
                    {
                        Destroy(allLabel);
                    }
                    allLabels.Clear();
                    
                    foreach (GameObject allItem in allItems)
                    {
                        Destroy(allItem);
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
                text.text = "Benötigte Ressourcen";
                foreach (Resource requiredResource in mileStones[mileStonesDone].RequiredResources)
                {
                    item = Instantiate(itemPrefab, mileStonePanel.transform, true);
                    allItems.Add(item);
                    itemToggles.Add(item.GetComponentInChildren<Toggle>());
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
                text.text = "Benötigte Module";
                foreach (MileStoneModules requiredModule in mileStones[mileStonesDone].RequiredModules)
                {
                    item = Instantiate(itemPrefab, mileStonePanel.transform, true);
                    allItems.Add(item);
                    itemToggles.Add(item.GetComponentInChildren<Toggle>());
                    item.transform.localScale = Vector3.one;
                    text = item.GetComponentInChildren<TextMeshProUGUI>();

                    switch (requiredModule.buildingTypes)
                    {
                        case BuildingTypes.StartModule:
                            text.text = "";
                            break;
                        case BuildingTypes.EnergyGain:
                            text.text += $"{requiredModule.value} Generator";
                            if (requiredModule.value <= 1) break;
                            text.text += "en";
                            break;
                        case BuildingTypes.LifeSupportGain:
                            text.text += $"{requiredModule.value} Biokuppel";
                            if (requiredModule.value <= 1) break;
                            text.text += "en";
                            break;
                        case BuildingTypes.MaterialGain:
                            text.text += $"{requiredModule.value} Raffinerie";
                            if (requiredModule.value <= 1) break;
                            text.text += "n";
                            break;
                        case BuildingTypes.EnergySave:
                            text.text += $"{requiredModule.value} Batterie";
                            if (requiredModule.value <= 1) break;
                            text.text += "n";
                            break;
                        case BuildingTypes.LifeSupportSave:
                            text.text += $"{requiredModule.value} Wassertank";
                            if (requiredModule.value <= 1) break;
                            text.text += "s";
                            break;
                        case BuildingTypes.MaterialSave:
                            text.text += $"{requiredModule.value} Erzlager";
                            if (requiredModule.value <= 1) break;
                            text.text += "er";
                            break;
                        case BuildingTypes.CitizenSave:
                            text.text += $"{requiredModule.value} Wohneinheit";
                            
                            if (requiredModule.value <= 1) break;
                            text.text += "en";
                            break;
                        case BuildingTypes.All:
                            text.text += $"Insgesamt {requiredModule.value} Modul";
                            if (requiredModule.value <= 1) break;
                            text.text += "e";
                            break;
                    }
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
            if (!menuWasBuild) return false;

            bool hasAllRequiredStuff = true;
            int index = 0;

            if (mileStones[mileStonesDone].RequiredEvent.Length != 0)
            {
                foreach (MileStoneEventNames requiredEvent in mileStones[mileStonesDone].RequiredEvent)
                {
                    foreach (MileStoneEvent mileStoneEvent in events)
                    {
                        if (mileStoneEvent.Name != requiredEvent) continue;

                        for (int i = 0; i < mileStoneEvent.Events.Length; i++)
                        {
                            if (!showText)
                            {
                                if (!mileStoneEvent.CheckAchieved(index))
                                {
                                    hasAllRequiredStuff = false;
                                    index++;
                                    continue;
                                }
                            }
                            else
                            {
                                if (itemToggles[index].isOn)
                                {
                                    index++;
                                    continue;
                                }

                                if (!mileStoneEvent.CheckAchieved(index))
                                {
                                    itemToggles[index].isOn = false;
                                    hasAllRequiredStuff = false;
                                }
                                else
                                {
                                    itemToggles[index].isOn = true;
                                    SoundManager.PlaySound(SoundManager.Sound.AchievedMilestone);
                                }
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
                    if (requiredResource.resource != manager.ResourceType) continue;
                    
                    if (manager.SavedResourceValue < requiredResource.value)
                    {
                        itemToggles[index].isOn = false;
                        hasAllRequiredStuff = false;
                    }
                    else
                    {
                        if (itemToggles[index].isOn)
                        {
                            index++;
                            continue;
                        }
                        itemToggles[index].isOn = true;
                        SoundManager.PlaySound(SoundManager.Sound.AchievedMilestone); 
                    }

                    index++;
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
                if (itemToggles[index].isOn)
                {
                    index++;
                    continue;
                }

                if (gameManager.GetBuildingCount(requiredModule.buildingTypes) < requiredModule.value)
                {
                    itemToggles[index].isOn = false;
                    hasAllRequiredStuff = false;
                }
                else
                {
                    itemToggles[index].isOn = true;
                    SoundManager.PlaySound(SoundManager.Sound.AchievedMilestone);
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

            return true;
        }

        #endregion

        #endregion
    }
}
