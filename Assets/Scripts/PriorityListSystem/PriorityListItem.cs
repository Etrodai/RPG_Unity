using Buildings;
using TMPro;
using UnityEngine;

namespace PriorityListSystem
{
    public class PriorityListItem : MonoBehaviour
    {
        #region Variables
        
        [SerializeField] private BuildingTypes type;
        [SerializeField] private TextMeshProUGUI buildingGroup;
        [SerializeField] private TextMeshProUGUI workingBuildings;
        [SerializeField] private GameObject upButton;
        [SerializeField] private GameObject downButton;
        private GameManager gameManager;
        private int priority;
        
        #endregion
    
        #region Properties
        
        public int Priority => priority;
        public BuildingTypes Type => type;
        
        #endregion
    
        #region UnityEvents
        
        private void Start()
        {
            gameManager = MainManagerSingleton.Instance.GameManager;
            priority = transform.GetSiblingIndex();
            buildingGroup.text = type.ToString();
            workingBuildings.text = $"{gameManager.GetBuildingCount(type)}";
            ChangePriority(priority);
        }
        
        #endregion
    
        #region OnClickEvents
        
        public void OnClickPlusButton()
        {
            if (priority == 0) return;
            foreach (PriorityListItem item in gameManager.PriorityListItems)
            {
                if (item.transform.GetSiblingIndex() == priority - 1)
                {
                    item.transform.SetSiblingIndex(priority);
                    item.ChangePriority(priority);
                }
            }

            priority--;
            transform.SetSiblingIndex(priority);
            ChangePriority(priority);
            gameManager.OnChangePriority();
        }

        public void OnClickMinusButton()
        {
            if (priority != gameManager.PriorityListItems.Length - 1)
            {
                foreach (PriorityListItem item in gameManager.PriorityListItems)
                {
                    if (item.transform.GetSiblingIndex() == priority + 1)
                    {
                        item.transform.SetSiblingIndex(priority);
                        item.ChangePriority(priority);
                    }
                }

                priority++;
                transform.SetSiblingIndex(priority);
                ChangePriority(priority);
                gameManager.OnChangePriority();
            }
        }
        
        #endregion

        #region Methods

        private void ChangePriority(int index)
        {
            priority = index;
            if (priority == 0)
            {
                upButton.SetActive(false);
            }
            else if (priority == gameManager.PriorityListItems.Length - 1)
            {
                downButton.SetActive(false);
            }
            else
            {
                upButton.SetActive(true);
                downButton.SetActive(true);
            }
        }
        
        #endregion
    }
}
