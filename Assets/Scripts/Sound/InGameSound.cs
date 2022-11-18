using UnityEngine;

namespace Sound
{
    public class InGameSound : MonoBehaviour
    {
        public void Start()
        {
            SoundManager.PlaySound(SoundManager.Sound.InGameBackgroundMusic, false);
        }
    }
}
