using Buildings;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PriorityListSystem
{
    public class PriorityListItem : MonoBehaviour
    {
        #region Variables
        
        [SerializeField] private TextMeshProUGUI buildingGroup;
        [SerializeField] private TextMeshProUGUI workingBuildings;
        [SerializeField] private GameObject upButton;
        [SerializeField] private GameObject downButton;
        private GameManager gameManager;

        #endregion

        #region Events

        public UnityEvent onChangePriorityUI;

        #endregion
        
        #region Properties

        public int Priority { get; set; }
        public BuildingTypes Type { get; set; }

        #endregion
    
        #region UnityEvents
        
        /// <summary>
        /// builds Item with name and count
        /// </summary>
        public void Instantiate()
        {
            onChangePriorityUI.AddListener(ChangeUIText);
            gameManager = MainManagerSingleton.Instance.GameManager;
            buildingGroup.text = Type.ToString();
            int allBuildingsCount = gameManager.GetBuildingCount(Type);
            int workingBuildingsCount = gameManager.GetWorkingBuildingCount(Type);
            workingBuildings.text = $"{workingBuildingsCount}/{allBuildingsCount}";
        }
        
        #endregion
    
        #region OnClickEvents
        
        /// <summary>
        /// changes priority and place of item (higher) and its sibling (lower)
        /// </summary>
        public void OnClickPlusButton()
        {
            if (Priority == 0) return;
            foreach (PriorityListItem item in gameManager.PriorityListItems)
            {
                if (item.transform.GetSiblingIndex() != (Priority - 1) * 2 + 1) continue;
                item.transform.SetSiblingIndex(Priority * 2 + 1);
                item.ChangePriority(Priority);
            }

            Priority--;
            transform.SetSiblingIndex(Priority * 2 + 1);
            ChangePriority(Priority);
            gameManager.OnChangePriority();
        }

        /// <summary>
        /// changes priority and place of item (lower) and its sibling (higher)
        /// </summary>
        public void OnClickMinusButton()
        {
            if (Priority == gameManager.PriorityListItems.Count - 1) return;
            foreach (PriorityListItem item in gameManager.PriorityListItems)
            {
                if (item.transform.GetSiblingIndex() != (Priority + 1) * 2 + 1) continue;
                item.transform.SetSiblingIndex(Priority * 2 + 1);
                item.ChangePriority(Priority);
            }

            Priority++;
            transform.SetSiblingIndex(Priority * 2 + 1);
            ChangePriority(Priority);
            gameManager.OnChangePriority();
        }
        
        #endregion
        
        #region Methods

        /// <summary>
        /// changes uiText of Item
        /// </summary>
        private void ChangeUIText()
        {
            int allBuildingsCount = gameManager.GetBuildingCount(Type);
            int workingBuildingsCount = gameManager.GetWorkingBuildingCount(Type);
            workingBuildings.text = $"{workingBuildingsCount}/{allBuildingsCount}";
        }
        
        /// <summary>
        /// if it's first or last Priority only one button is needed
        /// </summary>
        /// <param name="index">priority of changed item</param>
        public void ChangePriority(int index)
        {
            Priority = index;
            if (Priority == 0)
            {
                upButton.SetActive(false);
            }
            else if (Priority == gameManager.PriorityListItems.Count - 1)
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