using UnityEngine;

namespace Sound
{
    public class InGameSound : MonoBehaviour
    {
        void Start()
        {
            SoundManager.PlaySound(SoundManager.Sound.InGameBackgroundMusic, false);
        }
    }
}
