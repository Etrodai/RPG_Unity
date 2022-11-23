using UnityEngine;

namespace Sound
{
    public class MainMenuSound : MonoBehaviour //Made by Robin
    {
        public void Start()
        {
            SoundManager.PlaySound(SoundManager.Sound.MainMenuBackgroundMusic, true);
        }
    }
}
