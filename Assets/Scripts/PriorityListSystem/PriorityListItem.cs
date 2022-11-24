using Buildings;
using Manager;
using MilestoneSystem.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PriorityListSystem
{
    public class PriorityListItem : MonoBehaviour //Made by Robin
    {
        #region Variables
        
        [SerializeField] private TextMeshProUGUI buildingGroup;
        [SerializeField] private TextMeshProUGUI workingBuildings;
        [SerializeField] private GameObject upButton;
        [SerializeField] private GameObject downButton;
        private ShowPrioritySystemMileStoneEvent mileStoneEvent;

        private GameManager gameManager;

        #endregion

        #region Events

        public UnityEvent onChangePriorityUI;

        #endregion
        
        #region Properties

        public int Priority { get; set; }
        
        public BuildingType Type { get; set; }
        
        public ShowPrioritySystemMileStoneEvent MileStoneEvent
        {
            set => mileStoneEvent = value;
        }
        
        #endregion
    
        #region UnityEvents
        
        /// <summary>
        /// builds Item with name and count
        /// </summary>
        public void Instantiate()
        {
            onChangePriorityUI.AddListener(ChangeUIText);
            gameManager = MainManagerSingleton.Instance.GameManager;
            switch (Type)
            {
                case BuildingType.EnergyGain:
                    buildingGroup.text += "Generatoren";
                    break;
                case BuildingType.LifeSupportGain:
                    buildingGroup.text += "Biokuppelen";
                    break;
                case BuildingType.MaterialGain:
                    buildingGroup.text += "Raffinerien";
                    break;
                case BuildingType.EnergySave:
                    buildingGroup.text += "Batterien";
                    break;
                case BuildingType.LifeSupportSave:
                    buildingGroup.text += "Wassertanks";
                    break;
                case BuildingType.MaterialSave:
                    buildingGroup.text += "Erzlager";
                    break;
                case BuildingType.CitizenSave:
                    buildingGroup.text += "Wohneinheit";
                    break;
            }

            int allBuildingsCount = gameManager.GetBuildingCount(Type);
            int workingBuildingsCount = gameManager.GetWorkingBuildingCount(Type);
            workingBuildings.text = $"{workingBuildingsCount}/{allBuildingsCount}";
        }

        private void OnDestroy()
        {
            onChangePriorityUI.RemoveListener(ChangeUIText);
        }

        #endregion
    
        #region OnClickEvents
        
        /// <summary>
        /// changes priority and place of item (higher) and its sibling (lower)
        /// </summary>
        public void OnClickPlusButton()
        {
            if (Priority == 0) return;
            for (int i = 0; i < gameManager.PriorityListItems.Count; i++)
            {
                PriorityListItem item = gameManager.PriorityListItems[i];
                if (item.transform.GetSiblingIndex() != (Priority - 1) * 2 + 1) continue;
                item.transform.SetSiblingIndex(Priority * 2 + 1);
                item.ChangePriority(Priority);
            }

            Priority--;
            transform.SetSiblingIndex(Priority * 2 + 1);
            ChangePriority(Priority);
            gameManager.OnChangePriority();
            mileStoneEvent.ClickPlusButton();
        }

        /// <summary>
        /// changes priority and place of item (lower) and its sibling (higher)
        /// </summary>
        public void OnClickMinusButton()
        {
            if (Priority == gameManager.PriorityListItems.Count - 1) return;
            for (int i = 0; i < gameManager.PriorityListItems.Count; i++)
            {
                PriorityListItem item = gameManager.PriorityListItems[i];
                if (item.transform.GetSiblingIndex() != (Priority + 1) * 2 + 1) continue;
                item.transform.SetSiblingIndex(Priority * 2 + 1);
                item.ChangePriority(Priority);
            }

            Priority++;
            transform.SetSiblingIndex(Priority * 2 + 1);
            ChangePriority(Priority);
            gameManager.OnChangePriority();
            mileStoneEvent.ClickMinusButton();
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