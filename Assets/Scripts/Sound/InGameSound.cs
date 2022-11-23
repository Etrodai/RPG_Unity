using UnityEngine;

namespace Sound
{
    public class InGameSound : MonoBehaviour //Made by Robin
    {
        public void Start()
        {
            SoundManager.PlaySound(SoundManager.Sound.InGameBackgroundMusic, false);
        }
    }
}
