using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sound
{
    [System.Serializable]
    public struct SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip[] clips;
    }

    public static class SoundManager
    {
        public enum Sound
        {
            BuildModule,
            ButtonOver,
            ButtonClick
        }

        private static Dictionary<Sound, float> soundTimerDictionary;
        private static GameObject oneShotGameObject;
        private static AudioSource oneShotAudioSource;

        public static void Initialize()
        {
            soundTimerDictionary = new Dictionary<Sound, float>
            {
                [Sound.BuildModule] = 0f
            };
        }
        
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
            
            //TODO use ObjectPool
            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
        
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
            
            //TODO use ObjectPool
            Object.Destroy(soundGameObject, audioSource.clip.length);
        }

        private static bool CanPlaySound(Sound sound)
        {
            switch (sound)
            {
                case Sound.BuildModule:
                    if (soundTimerDictionary.ContainsKey(sound))
                    {
                        float lastTimePlayed = soundTimerDictionary[sound];
                        float buildTimerMax = 1f;
                        if (lastTimePlayed + buildTimerMax < Time.time)
                        {
                            soundTimerDictionary[sound] = Time.time;
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                default:
                    return true;
            }
        }
        
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

        public static void AddButtonSounds(this Button button)
        {
            button.onClick.AddListener(() => PlaySound(Sound.ButtonClick));
            if (button.TryGetComponent(out EventTrigger trigger))
            {
                trigger.AddListener(EventTriggerType.PointerEnter, _ => PlaySound(Sound.ButtonOver));
            }
        }

        private static void AddListener(this EventTrigger trigger, EventTriggerType eventType,
            System.Action<PointerEventData> listener)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(data => listener.Invoke((PointerEventData)data));
            trigger.triggers.Add(entry);
        }
    }
}
