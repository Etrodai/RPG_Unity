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
        
        public void OnClickPlusButton()
        {
            if (Priority == 0) return;
            foreach (PriorityListItem item in gameManager.PriorityListItems)
            {
                if (item.transform.GetSiblingIndex() == (Priority - 1) * 2 + 1)
                {
                    item.transform.SetSiblingIndex(Priority * 2 + 1);
                    item.ChangePriority(Priority);
                }
            }

            Priority--;
            transform.SetSiblingIndex(Priority * 2 + 1);
            ChangePriority(Priority);
            gameManager.OnChangePriority();
        }

        public void OnClickMinusButton()
        {
            if (Priority != gameManager.PriorityListItems.Count - 1)
            {
                foreach (PriorityListItem item in gameManager.PriorityListItems)
                {
                    if (item.transform.GetSiblingIndex() == (Priority + 1) * 2 + 1)
                    {
                        item.transform.SetSiblingIndex(Priority * 2 + 1);
                        item.ChangePriority(Priority);
                    }
                }

                Priority++;
                transform.SetSiblingIndex(Priority * 2 + 1);
                ChangePriority(Priority);
                gameManager.OnChangePriority();
            }
        }
        
        #endregion
        
        #region Methods

        private void ChangeUIText() //always, when + building or enabled/disabled as Event
        {
            int allBuildingsCount = gameManager.GetBuildingCount(Type);
            int workingBuildingsCount = gameManager.GetWorkingBuildingCount(Type);
            workingBuildings.text = $"{workingBuildingsCount}/{allBuildingsCount}";
        }
        
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