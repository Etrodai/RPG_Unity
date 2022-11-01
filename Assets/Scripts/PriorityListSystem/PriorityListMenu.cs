using System;
using Buildings;
using Manager;
using TMPro;
using UnityEngine;

namespace PriorityListSystem
{
    public class PriorityListMenu : MonoBehaviour
    {
        [SerializeField] private GameObject labelPrefab;
        [SerializeField] private GameObject itemPrefab;
        private GameManager gameManager;

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

                    priority++;
                }
            }

            foreach (PriorityListItem item in gameManager.PriorityListItems)
            {
                item.Instantiate();
                item.ChangePriority(item.Priority);
            }
        }
    }
}
