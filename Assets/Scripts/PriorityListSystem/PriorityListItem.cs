using Buildings;
using Manager;
using TMPro;
using UnityEngine;

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
    
        #region Properties

        public int Priority { get; set; }
        public BuildingTypes Type { get; set; }

        #endregion
    
        #region UnityEvents
        
        public void Instantiate()
        {
            gameManager = MainManagerSingleton.Instance.GameManager;
            buildingGroup.text = Type.ToString();
            workingBuildings.text = $"{gameManager.GetBuildingCount(Type)}";
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