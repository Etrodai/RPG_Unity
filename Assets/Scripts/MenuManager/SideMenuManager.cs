using UnityEngine;

namespace MenuManager
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

        #endregion
        
        #region UnityEvents

        private void Start()
        {
            CloseMenu();
        }

        #endregion
        
        #region OnClickEvents
    
        public void OnPriorityListMenuButton()
        {
            if (priorityListPanel.isMinimized && mileStoneSystemPanel.isMinimized)
            {
                OpenMenu();
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
            var transform1 = transform;
            var transformPosition = transform1.position;
            transformPosition.x -= 300 * canvas.scaleFactor;
            transform1.position = transformPosition;
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

        private void ShowPanel(ref Panel panel)
        {
            panel.panel.SetActive(true);
            panel.isMinimized = false;
        }

        private void HidePanel(ref Panel panel)
        {
            panel.panel.SetActive(false);
            panel.isMinimized = true;
        }

        #endregion
    }
}