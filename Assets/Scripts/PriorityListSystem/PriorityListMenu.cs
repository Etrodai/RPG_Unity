using System;
using System.Collections.Generic;
using System.IO;
using Buildings;
using Manager;
using SaveSystem;
using TMPro;
using UnityEngine;

namespace PriorityListSystem
{
    [System.Serializable]
    public struct PriorityListItemSave
    {
        public int priority;
        public BuildingTypes type;
    }
    
    public class PriorityListMenu : MonoBehaviour
    {
        [SerializeField] private GameObject labelPrefab;
        [SerializeField] private GameObject itemPrefab;
        private GameManager gameManager;
        private List<PriorityListItem> items = new();

        public void Instantiate()
        {
            gameManager = MainManagerSingleton.Instance.GameManager;
            
            int priority = 0;
            for (int i = 0; i < Enum.GetNames(typeof(BuildingTypes)).Length; i++)
            {
                BuildingTypes type = (BuildingTypes) i;
                if (!(type == BuildingTypes.All || type == BuildingTypes.StartModule || type == BuildingTypes.EnergySave))
                {
                    GameObject priorityLabel = Instantiate (labelPrefab, gameObject.transform, true);
                    priorityLabel.transform.localScale = Vector3.one;
                    TextMeshProUGUI labelText = priorityLabel.GetComponent<TextMeshProUGUI>();
                    labelText.text = $"Priority {priority}";

                    GameObject itemObject = Instantiate(itemPrefab, gameObject.transform, true);
                    itemObject.transform.localScale = Vector3.one;
                    PriorityListItem item = itemObject.GetComponent<PriorityListItem>();
                    item.Priority = priority;
                    item.Type = type;

                    gameManager.PriorityListItems.Add(item);
                    items.Add(item);
                    priority++;
                }
            }

            foreach (PriorityListItem item in gameManager.PriorityListItems)
            {
                item.Instantiate();
                item.ChangePriority(item.Priority);
            }
            
            Save.OnSaveButtonClick.AddListener(SaveData);
            Save.OnSaveAsButtonClick.AddListener(SaveDataAs);
            Load.OnLoadButtonClick.AddListener(LoadData);
        }
        
        #region Save Load

        private void SaveData()
        {
            PriorityListItemSave[] data = new PriorityListItemSave[items.Count];

            for (int i = 0; i < items.Count; i++)
            {
                data[i].priority = items[i].Priority;
                data[i].type = items[i].Type;
            }
        
            Save.AutoSaveData(data, "PriorityListMenu");
        }
    
        private void SaveDataAs(string savePlace)
        {
            PriorityListItemSave[] data = new PriorityListItemSave[items.Count];

            for (int i = 0; i < items.Count; i++)
            {
                data[i].priority = items[i].Priority;
                data[i].type = items[i].Type;
            }
        
            Save.SaveDataAs(savePlace, data, "PriorityListMenu");
        }
    
        private void LoadData(string path)
        {
            path = Path.Combine(path, "PriorityListMenu");

            //TODO: (Robin) PriorityListItemSave[] data = Load.LoadData(path);
            PriorityListItemSave[] data = new PriorityListItemSave[1];

            for (int i = 0; i < data.Length; i++)
            {
                items[i].Type = data[i].type;
                
                while (items[i].Priority < data[i].priority) //TODO: (Robin) Attention!!! whileLoop
                {
                    items[i].OnClickPlusButton();
                }

                while (items[i].Priority > data[i].priority) //TODO: (Robin) Attention!!! whileLoop
                {
                    items[i].OnClickMinusButton();
                }
            }
        }

        #endregion

    }
}
