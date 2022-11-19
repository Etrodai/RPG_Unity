using UnityEngine;

namespace Sound
{
    public class MainMenuSound : MonoBehaviour
    {
        public void Start()
        {
            SoundManager.PlaySound(SoundManager.Sound.MainMenuBackgroundMusic, true);
        }
    }
}
