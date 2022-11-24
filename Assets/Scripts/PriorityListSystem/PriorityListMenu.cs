using System;
using System.Collections.Generic;
using Buildings;
using Manager;
using MilestoneSystem.Events;
using SaveSystem;
using TMPro;
using UnityEngine;

namespace PriorityListSystem
{
    [Serializable]
    public struct PriorityListItemSave
    {
        public int priority;
        public int type;
    }
    
    public class PriorityListMenu : MonoBehaviour //Made by Robin
    {
        #region Variables

        [SerializeField] private GameObject labelPrefab;
        [SerializeField] private GameObject itemPrefab;
        private GameManager gameManager;
        private readonly List<PriorityListItem> items = new();
        [SerializeField] private ShowPrioritySystemMileStoneEvent mileStoneEvent;
        private SaveData saveData;

        #endregion

        #region UnityEvents

        private void OnDestroy()
        {
            Save.OnSaveButtonClick.RemoveListener(SaveData);
            Save.OnSaveAsButtonClick.RemoveListener(SaveDataAs);
            Load.OnLoadButtonClick.RemoveListener(LoadData);
        }

        #endregion
        
        #region Methods
        
        /// <summary>
        /// builds PriorityListMenu:
        /// adds heading and item
        ///
        /// adds Listener
        /// </summary>
        public void Instantiate()
        {
            saveData = SaveSystem.SaveData.Instance;
            gameManager = MainManagerSingleton.Instance.GameManager;
            
            int priority = 0;
            for (int i = 0; i < Enum.GetNames(typeof(BuildingType)).Length; i++)
            {
                BuildingType type = (BuildingType) i;
                if (type is BuildingType.All or BuildingType.StartModule or BuildingType.EnergySave) continue;
                
                GameObject priorityLabel = Instantiate (labelPrefab, gameObject.transform, true);
                priorityLabel.transform.localScale = Vector3.one;
                TextMeshProUGUI labelText = priorityLabel.GetComponent<TextMeshProUGUI>();
                labelText.text = $"Priorität {priority + 1}";

                GameObject itemObject = Instantiate(itemPrefab, gameObject.transform, true);
                itemObject.transform.localScale = Vector3.one;
                PriorityListItem item = itemObject.GetComponent<PriorityListItem>();
                item.Priority = priority;
                item.Type = type;
                item.MileStoneEvent = mileStoneEvent;

                gameManager.PriorityListItems.Add(item);
                items.Add(item);
                priority++;
            }

            for (int i = 0; i < gameManager.PriorityListItems.Count; i++)
            {
                PriorityListItem item = gameManager.PriorityListItems[i];
                item.Instantiate();
                item.ChangePriority(item.Priority);
            }

            Save.OnSaveButtonClick.AddListener(SaveData);
            Save.OnSaveAsButtonClick.AddListener(SaveDataAs);
            Load.OnLoadButtonClick.AddListener(LoadData);
        }
        
        #endregion
        
        #region Save Load

        private void SaveData()
        {
            PriorityListItemSave[] data = new PriorityListItemSave[items.Count];

            for (int i = 0; i < items.Count; i++)
            {
                data[i].priority = items[i].Priority;
                data[i].type = (int)items[i].Type;
            }

            saveData.GameSave.priorityListData = data;
            // Save.AutoSaveData(data, SaveName);
        }
    
        private void SaveDataAs(string savePlace)
        {
            PriorityListItemSave[] data = new PriorityListItemSave[items.Count];

            for (int i = 0; i < items.Count; i++)
            {
                data[i].priority = items[i].Priority;
                data[i].type = (int)items[i].Type;
            }

            saveData.GameSave.priorityListData = data;
            // Save.SaveDataAs(savePlace, data, SaveName);
        }
    
        private void LoadData(GameSave gameSave)
        {
            // path = Path.Combine(path, $"{SaveName}.dat");
            // if (!File.Exists(path)) return;
            //
            // PriorityListItemSave[] data = Load.LoadData(path) as PriorityListItemSave[];

            PriorityListItemSave[] data = gameSave.priorityListData;
            
            // if (data == null) return;

            for (int i = 0; i < data.Length; i++)
            {
                items[i].Type = (BuildingType)data[i].type;
                
                while (items[i].Priority < data[i].priority) 
                {
                    items[i].OnClickPlusButton();
                }

                while (items[i].Priority > data[i].priority)
                {
                    items[i].OnClickMinusButton();
                }
            }
        }

        #endregion
    }
}
