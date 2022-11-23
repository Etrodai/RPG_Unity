using UnityEngine;
using UnityEngine.Audio;

namespace Sound
{
    [System.Serializable]
    public struct SoundAudioClip //Made by Robin
    {
        public SoundManager.Sound sound;
        public AudioMixerGroup group;
        public AudioClip[] clips;
    }
}