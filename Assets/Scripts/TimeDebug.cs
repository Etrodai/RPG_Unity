using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeDebug : MonoBehaviour
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
        timeText.text = time.ToString();
    }
    #endregion
}
