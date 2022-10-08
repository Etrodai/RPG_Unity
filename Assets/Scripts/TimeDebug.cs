using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeDebug : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    private float time;


    void Update()
    {
        time += Time.deltaTime;
        timeText.text = time.ToString();
    }
}
