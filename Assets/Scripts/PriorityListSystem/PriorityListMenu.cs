using Buildings;
using TMPro;
using UnityEngine;

namespace PriorityListSystem
{
    public class PriorityListMenu : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject menu; // SideMenu which always is there and can be mini and maximized
        bool isMinimized; // shows if the SideMenu is mini or maximized

        #endregion
        
        #region OnClickEvents
        public void OnClickMenuButton()
        {
            if (isMinimized) OpenMenu();
            else CloseMenu();
        }
        #endregion

        #region Methods
       
        /// <summary>
        /// maximizes menu by moving it to the left side
        /// </summary>
        private void OpenMenu()
        {
            Vector3 transformPosition = menu.transform.position;
            transformPosition.x -= 300 * canvas.scaleFactor;
            menu.transform.position = transformPosition;
            isMinimized = false;
        }

        /// <summary>
        /// minimizes menu by moving it to the right side
        /// </summary>
        private void CloseMenu()
        {
            var transformPosition = menu.transform.position;
            transformPosition.x += 300 * canvas.scaleFactor;
            menu.transform.position = transformPosition;
            isMinimized = true;
        }
        #endregion
    }
}
