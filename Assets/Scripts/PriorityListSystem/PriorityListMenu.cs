using Buildings;
using TMPro;
using UnityEngine;

namespace PriorityListSystem
{
    public class PriorityListMenu : MonoBehaviour
    {
        #region TODOS

        // anfügen an Scripte

        #endregion
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
            gameManager = GameManager.Instance;
            priority = transform.GetSiblingIndex();
            buildingGroup.text = type.ToString();
            workingBuildings.text = $"{gameManager.GetBuildingCount(type)}";
            ChangePriority(priority);
        }
        #endregion

        #region OnClickEvents
        public void OnClickPlusButton()
        {
            if (priority != 0)
            {
                foreach (PriorityListMenu item in gameManager.PriorityListItems)
                {
                    if (item.transform.GetSiblingIndex() == priority - 1)
                    {
                        item.transform.SetSiblingIndex(priority);
                        item.ChangePriority(priority);
                    }
                }

                priority--;
                transform.SetSiblingIndex(priority);
            }
        }

        public void OnClickMinusButton()
        {
            if (priority != gameManager.PriorityListItems.Length)
            {
                foreach (PriorityListMenu item in gameManager.PriorityListItems)
                {
                    if (item.transform.GetSiblingIndex() == priority + 1)
                    {
                        item.transform.SetSiblingIndex(priority);
                        item.ChangePriority(priority);
                    }
                }

                priority++;
                transform.SetSiblingIndex(priority);
            }
        }
        #endregion

        #region Methods
        public void ChangePriority(int index)
        {
            priority = index;
            if (priority == 0)
            {
                upButton.SetActive(false);
            }
            else if (priority == gameManager.PriorityListItems.Length)
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
