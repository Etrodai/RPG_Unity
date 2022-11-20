using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private ShowPrioritySystemMileStoneEvent showPrioMileStoneEvent;
        [SerializeField] private ShowMileStoneSystem showMileStoneMileStoneEvent;
        private Vector3 openPosition;
        private Vector3 closePosition;
        [SerializeField] private float lerpTime;

        #endregion

        public bool CanOpenMileStonePanel { get; set; }

        public bool CanOpenPriorityListPanel { get; set; }

        #region UnityEvents

        private void Awake()
        {
            openPosition = transform.position;
        }

        /// <summary>
        /// instantiates priorityListMenu
        /// </summary>
        private void Start()
        {
            priorityListMenu = GetComponentInChildren<PriorityListMenu>(true);
            priorityListMenu.Instantiate();
            // CloseMenuInstantly();
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
                showPrioMileStoneEvent.ClickPriorityButton();
                ShowPanel(ref priorityListPanel);
            }
            else if (priorityListPanel.isMinimized &&  !mileStoneSystemPanel.isMinimized)
            {
                showPrioMileStoneEvent.ClickPriorityButton();
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
                showMileStoneMileStoneEvent.OpenMileStoneMenu();
                ShowPanel(ref mileStoneSystemPanel);
            }
            else if (mileStoneSystemPanel.isMinimized &&  !priorityListPanel.isMinimized)
            {
                ShowPanel(ref mileStoneSystemPanel);
                HidePanel(ref priorityListPanel);
            }
            else if (!mileStoneSystemPanel.isMinimized)
            {
                showMileStoneMileStoneEvent.CloseMileStoneMenu();
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
            if (!priorityListPanel.isMinimized && !mileStoneSystemPanel.isMinimized) return;

            StartCoroutine(MoveMenu(true));
            // Transform transform1 = transform;
            // Vector3 transformPosition = transform1.position;
            //
            // transformPosition.x -= 300 * canvas.scaleFactor;
            // transform1.position = transformPosition;
        }

        public void CloseMenuInstantly()
        {
            if (closePosition == Vector3.zero)
            {
                closePosition = new(openPosition.x + 300 * canvas.scaleFactor, openPosition.y, openPosition.z);
            }
            StopAllCoroutines();
            transform.position = closePosition;
            HidePanel(ref mileStoneSystemPanel);
            HidePanel(ref priorityListPanel);
        }
        
        /// <summary>
        /// minimizes menu by moving it to the right side
        /// </summary>
        private void CloseMenu()
        {
            if (priorityListPanel.isMinimized && mileStoneSystemPanel.isMinimized) return;

            StartCoroutine(MoveMenu(false));
            // Transform transform1 = transform;
            // Vector3 transformPosition = transform1.position;
            // transformPosition.x += 300 * canvas.scaleFactor;
            // transform1.position = transformPosition;
            // HidePanel(ref mileStoneSystemPanel);
            // HidePanel(ref priorityListPanel);
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

        private IEnumerator MoveMenu(bool shouldOpen)
        {
            yield return new WaitForEndOfFrame();
            float t = 0;
            while (t < lerpTime)
            {
                yield return new WaitForEndOfFrame();
                transform.position = shouldOpen
                    ? Vector3.Lerp(closePosition, openPosition, t/lerpTime)
                    : Vector3.Lerp(openPosition, closePosition, t/lerpTime);
                t += Time.deltaTime;
            }

            if (shouldOpen) yield break;

            HidePanel(ref mileStoneSystemPanel);
            HidePanel(ref priorityListPanel);
        }

        #endregion
    }
}