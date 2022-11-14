using MilestoneSystem.Events;
using PriorityListSystem;
using UnityEngine;

namespace Manager.Menu
{
    [System.Serializable]
    public struct Panel
    {
        public GameObject panel;
        public bool isMinimized;
    }
    
    public class SideMenuManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Canvas canvas;
        [SerializeField] private Panel priorityListPanel;
        [SerializeField] private Panel mileStoneSystemPanel;
        private PriorityListMenu priorityListMenu;
        [SerializeField] private ShowPrioritySystemMileStoneEvent mileStoneEvent;

        #endregion

        #region UnityEvents
        
        /// <summary>
        /// instantiates priorityListMenu
        /// </summary>
        private void Start()
        {
            priorityListMenu = GetComponentInChildren<PriorityListMenu>(true);
            priorityListMenu.Instantiate();
            CloseMenu();
        }
        
        #endregion
        
        #region OnClickEvents
    
        /// <summary>
        /// opens menu with priorityPanel, if both Panels were minimized
        /// opens priorityPanel, if mileStonePanel was opened
        /// closes menu, if priorityPanel was opened
        /// </summary>
        public void OnPriorityListMenuButton()
        {
            if (priorityListPanel.isMinimized && mileStoneSystemPanel.isMinimized)
            {
                OpenMenu();
                mileStoneEvent.ClickPriorityButton();
                ShowPanel(ref priorityListPanel);
            }
            else if (priorityListPanel.isMinimized &&  !mileStoneSystemPanel.isMinimized)
            {
                ShowPanel(ref priorityListPanel);
                HidePanel(ref mileStoneSystemPanel);
            }
            else if (!priorityListPanel.isMinimized)
            {
                CloseMenu();
            }
        } 
    
        /// <summary>
        /// opens menu with mileStonePanel, if both Panels were minimized
        /// opens mileStonePanel, if priorityPanel was opened
        /// closes menu, if mileStonePanel was opened
        /// </summary>
        public void OnMileStoneSystemMenuButton()
        {
            if (mileStoneSystemPanel.isMinimized && priorityListPanel.isMinimized)
            {
                OpenMenu();
                ShowPanel(ref mileStoneSystemPanel);
            }
            else if (mileStoneSystemPanel.isMinimized &&  !priorityListPanel.isMinimized)
            {
                ShowPanel(ref mileStoneSystemPanel);
                HidePanel(ref priorityListPanel);
            }
            else if (!mileStoneSystemPanel.isMinimized)
            {
                CloseMenu();
            }
        }
    
        #endregion
    
        #region Methods
       
        /// <summary>
        /// maximizes menu by moving it to the left side
        /// </summary>
        private void OpenMenu()
        {
            Vector3 transformPosition = transform.position;
            transformPosition.x -= 300 * canvas.scaleFactor;
            transform.position = transformPosition;
        }

        /// <summary>
        /// minimizes menu by moving it to the right side
        /// </summary>
        public void CloseMenu()
        {
            if (priorityListPanel.isMinimized && mileStoneSystemPanel.isMinimized)
            {
                return;
            }
            
            var transform1 = transform;
            var transformPosition = transform1.position;
            transformPosition.x += 300 * canvas.scaleFactor;
            transform1.position = transformPosition;
            HidePanel(ref mileStoneSystemPanel);
            HidePanel(ref priorityListPanel);
        }

        /// <summary>
        /// sets given Panel Active and maximized
        /// </summary>
        /// <param name="panel">Panel to show</param>
        private void ShowPanel(ref Panel panel)
        {
            panel.panel.SetActive(true);
            panel.isMinimized = false;
        }

        /// <summary>
        /// sets given Panel inactive and minimized
        /// </summary>
        /// <param name="panel">Panel to hide</param>
        private void HidePanel(ref Panel panel)
        {
            panel.panel.SetActive(false);
            panel.isMinimized = true;
        }

        #endregion
    }
}