using System.Collections;
using UnityEngine;

namespace Sound
{
    public class SoundStorage : MonoBehaviour
    {
        #region Variables

        public static SoundStorage Instance { get; private set; }
        public SoundAudioClip[] soundAudioClips;

        #endregion

        #region UnityEvents

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
            
            DontDestroyOnLoad(Instance);

            SoundManager.Initialize();
        }

        #endregion

        #region Methods for SoundManager

        public void WaitForEndOfClip(float clipLength, SoundManager.Sound sound)
        {
            StartCoroutine(ChangeClip(clipLength, sound));
        }
        
        private IEnumerator ChangeClip(float t, SoundManager.Sound sound)
        {
            yield return new WaitForSeconds(t);
            SoundManager.PlaySound(sound, false);
        }

        #endregion

    }
}
