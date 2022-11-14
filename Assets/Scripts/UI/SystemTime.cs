using TMPro;
using UnityEngine;

//Maybe integrate in Game Manager, simple Script
//TODO: (Ben) Integrate in GameManager, Comments
namespace UI
{
    public class SystemTime : MonoBehaviour
    {
        private TextMeshProUGUI timeText;

        private void Awake()
        {
            timeText = this.GetComponent<TextMeshProUGUI>();
        }

        private void LateUpdate()
        {
            timeText.text = GetTime();
        }
    
        private string GetTime()
        {
            int[] times = new int[2] {System.DateTime.Now.Hour, System.DateTime.Now.Minute};
            string minText = times[1].ToString();
            if (times[1] < 10)
            {
                minText = "0" + times[1];
            }
            return $"{times[0]}:{minText}";
        }
    }
}
