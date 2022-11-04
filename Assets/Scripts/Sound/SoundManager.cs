using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sound
{
    /// <summary>
    /// You have to add every sound-group onto the SoundEnum and in the SoundStorage
    /// For starting the sound you have to activate it by starting PlaySound() at the point to play
    /// If a sound gets called in the Update() you can say, how often it will get played by adding it in the Initialize
    /// Button sounds will be added automatically, when the ButtonSound script is on the GameObject of the button
    /// </summary>
    public static class SoundManager
    {
        #region Variables
        
        /// <summary>
        /// All existing sound-groups
        /// </summary>
        public enum Sound
        {
            BuildModule,
            DisableModule,
            EnableModule,
            AchievedMilestone,
            EventEnters,
            ButtonOver,
            ButtonClick,
            InGameBackgroundMusic,
            MainMenuBackgroundMusic
        }
        private static Dictionary<Sound, float> soundTimerDictionary;
        private static GameObject oneShotGameObject;
        private static AudioSource oneShotAudioSource;
        private static GameObject loopGameObject;
        private static AudioSource loopAudioSource;
        private static GameObject changeGameObject;
        private static AudioSource changeAudioSource;
        
        #endregion

        #region Methods

        #region Initialize
        
        /// <summary>
        /// sets timer of sounds, which only should be played every t seconds (when called in Update)
        /// you also have to add it in CanPlaySound()
        /// </summary>
        public static void Initialize()
        {
            soundTimerDictionary = new Dictionary<Sound, float>
            {
                // [Sound.BuildModule] = 0f
            };
        }

        #endregion

        #region 2D Sounds

        /// <summary>
        /// plays a sound only one time in 2D
        /// </summary>
        /// <param name="sound">sound-group</param>
        public static void PlaySound(Sound sound)
        {
            if (!CanPlaySound(sound)) return;
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("One Shot Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
        
        /// <summary>
        /// plays a sound the whole time in current Scene. Should be called only once in a Scene, when it starts.
        /// </summary>
        /// <param name="sound">sound-group</param>
        /// <param name="isLooping">if it's looping it only plays one clip, if it isn't looping it gets a different clip after the last one is over</param>
        public static void PlaySound(Sound sound, bool isLooping)
        {
            if (!CanPlaySound(sound)) return;
            if (isLooping)
            {
                if (loopGameObject == null)
                {
                    loopGameObject = new GameObject("Loop Sound");
                    loopAudioSource = loopGameObject.AddComponent<AudioSource>();
                }

                loopAudioSource.clip = GetAudioClip(sound);
                loopAudioSource.loop = true;
                loopAudioSource.Play();
            }
            else
            {
                if (changeGameObject == null)
                {
                    changeGameObject = new GameObject("Chang Sound");
                    changeAudioSource = changeGameObject.AddComponent<AudioSource>();
                }
                changeAudioSource.PlayOneShot(GetAudioClip(sound));
                SoundStorage.Instance.WaitForEndOfClip(changeAudioSource, sound);
            }
        }

        #endregion

        #region 3D Sounds

        /// <summary>
        /// plays a sound one time at a given position in 3D. It doesn't move.
        /// </summary>
        /// <param name="sound">sound-group</param>
        /// <param name="position">position, where the 3D clip is running</param>
        public static void PlaySound(Sound sound, Vector3 position)
        {
            if (!CanPlaySound(sound)) return;
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.maxDistance = 100f;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;
            audioSource.Play();
            
            //TODO: (Robin) use ObjectPool
            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
        
        /// <summary>
        /// plays a sound one time at its parent's position in 3D and moves along with his parent. 
        /// </summary>
        /// <param name="sound">sound-group</param>
        /// <param name="parent">parent, where the 3D clip is moving along with</param>
        public static void PlaySound(Sound sound, GameObject parent)
        {
            if (!CanPlaySound(sound)) return;
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.SetParent(parent.transform);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.maxDistance = 100f;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;
            audioSource.Play();
            
            //TODO: (Robin) use ObjectPool
            Object.Destroy(soundGameObject, audioSource.clip.length);
        }

        #endregion

        #region Help Methods

        /// <summary>
        /// checks, if a sound could play again after its cooling time
        /// </summary>
        /// <param name="sound">sound-group</param>
        /// <returns>if it can be played or not</returns>
        private static bool CanPlaySound(Sound sound)
        {
            switch (sound)
            {
                // case Sound.BuildModule:
                //     if (soundTimerDictionary.ContainsKey(sound))
                //     {
                //         float lastTimePlayed = soundTimerDictionary[sound];
                //         float buildTimerMax = 1f;
                //         if (lastTimePlayed + buildTimerMax < Time.time)
                //         {
                //             soundTimerDictionary[sound] = Time.time;
                //             return true;
                //         }
                //         return false;
                //     }
                //     else
                //     {
                //         return true;
                //     }
                default:
                    return true;
            }
        }
        
        /// <summary>
        /// gets a random sound from sound-group
        /// </summary>
        /// <param name="sound">sound-group</param>
        /// <returns>random clip to play</returns>
        private static AudioClip GetAudioClip(Sound sound)
        {
            foreach (SoundAudioClip clip in SoundStorage.Instance.soundAudioClips)
            {
                if (clip.sound == sound)
                {
                    return clip.clips[Random.Range(0, clip.clips.Length)];
                }
            }

            Debug.LogError($"Sound {sound} not found!");
            return null;
        }

        #endregion

        #endregion
        
        #region Extension Methods
       
        /// <summary>
        /// adds sounds of every button in scene by adding it as listener to the events
        /// </summary>
        /// <param name="button"></param>
        public static void AddButtonSounds(this Button button)
        {
            button.onClick.AddListener(() => PlaySound(Sound.ButtonClick));
            if (button.TryGetComponent(out EventTrigger trigger))
            {
                trigger.AddListener(EventTriggerType.PointerEnter, _ => PlaySound(Sound.ButtonOver));
                // if more events of the eventTrigger are needed, add it here
            }
        }

        /// <summary>
        /// adds listener to the trigger events
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="eventType">eventType listener has listen to</param>
        /// <param name="listener">method, which is played when event gets triggered</param>
        private static void AddListener(this EventTrigger trigger, EventTriggerType eventType,
            System.Action<PointerEventData> listener)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(data => listener.Invoke((PointerEventData)data));
            trigger.triggers.Add(entry);
        }
       
        #endregion
    }


}
