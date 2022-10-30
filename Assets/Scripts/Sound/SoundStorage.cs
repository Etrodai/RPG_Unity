using UnityEngine;

namespace Sound
{
    public class SoundStorage : MonoBehaviour
    {
        public static SoundStorage Instance { get; private set; }

        public SoundAudioClip[] soundAudioClips;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            SoundManager.Initialize();
        }
    }
}
