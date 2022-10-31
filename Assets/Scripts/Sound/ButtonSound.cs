using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sound
{
    [RequireComponent(typeof(EventTrigger))]
    public class ButtonSound : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Button>().AddButtonSounds();
        }
    }
}
