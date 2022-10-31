using System.Collections;
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

        public void WaitForEndOfClip(AudioSource changeAudioSource, SoundManager.Sound sound)
        {
            StartCoroutine(ChangeClip(changeAudioSource.clip.length, sound));
        }
        
        private IEnumerator ChangeClip(float t, SoundManager.Sound sound)
        {
            yield return new WaitForSeconds(t);
            SoundManager.PlaySound(sound, false);
        }
    }
}
