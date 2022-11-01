using UnityEngine;

namespace Sound
{
    [System.Serializable]
    public struct SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip[] clips;
    }
}