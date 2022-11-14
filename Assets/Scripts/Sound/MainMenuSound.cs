using UnityEngine;

namespace Sound
{
    public class MainMenuSound : MonoBehaviour
    {
        void Start()
        {
            SoundManager.PlaySound(SoundManager.Sound.MainMenuBackgroundMusic, true);
        }
    }
}
