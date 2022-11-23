using System.Globalization;
using TMPro;
using UnityEngine;

namespace DebugMenus
{
    public class TimeDebug : MonoBehaviour //Made by Robin
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI timeText;
        private float time;

        #endregion

        #region UnityEvents

        /// <summary>
        /// shows time for Debugging
        /// </summary>
        void Update()
        {
            time += Time.deltaTime;
            timeText.text = time.ToString(CultureInfo.CurrentCulture);
        }

        #endregion
    }
}