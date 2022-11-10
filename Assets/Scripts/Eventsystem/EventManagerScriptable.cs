using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SaveSystem;
using Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Eventsystem
{
    [System.Serializable]
    public struct EventManagerSave
    {
        public float timer;
        public EventScriptableObject[] availableEvents;
        public EventScriptableObject[] usedEvents;
    }
    
    public class EventManagerScriptable : MonoBehaviour
    {
        //Event determination variables
        [SerializeField] private List<EventScriptableObject> availableEvents = new List<EventScriptableObject>();
        private Queue<EventScriptableObject> usedEvents = new Queue<EventScriptableObject>();
        private EventScriptableObject activeEvent;
        public EventScriptableObject ActiveEvent
        {
            get => activeEvent;
        }
        [SerializeField] private EventBehaviourScriptable eventBehaviour;
        private const int resetEvents = 0;
        private Action resetEventTimer;
        public Action ResetEventTimer
        {
            get => resetEventTimer;
            private set { resetEventTimer = value; }
        }

        //Timer related variables
        private float timer;
        private float totalTime;
        [SerializeField, Tooltip("Timer in seconds")] private float setTimer;
        private const float updateTimerRate = 0.5f;
        private const float endTimer = 0f;
        private const float stopTime = 0f;
        private const float startTime = 1f;
        
        [SerializeField] private Button sideMenuMileStoneButton;
        [SerializeField] private Button sideMenuPriorityButton;

        private const string saveName = "EventManager";
        
        private void Awake()
        {
            timer = setTimer;
        }

        private void Start()
        {
            resetEventTimer += ResetTimer;
            resetEventTimer += StartTimer;                                      //Adding those methods to enable calling them at the end of the event in EventBehaviourScriptable
            Save.OnSaveButtonClick.AddListener(SaveData);
            Save.OnSaveAsButtonClick.AddListener(SaveDataAs);
            Load.OnLoadButtonClick.AddListener(LoadData);
        }
        
        #region Save Load

        private void SaveData()
        {
            EventManagerSave[] data = new EventManagerSave[1];
            data[0] = new EventManagerSave
            {
                timer = this.timer,
                availableEvents = this.availableEvents.ToArray(),
                usedEvents = this.usedEvents.ToArray()
            };

            Save.AutoSaveData(data, saveName);
        }
    
        private void SaveDataAs(string savePlace)
        {
            EventManagerSave[] data = new EventManagerSave[1];
            data[0] = new EventManagerSave
            {
                timer = this.timer,
                availableEvents = this.availableEvents.ToArray(),
                usedEvents = this.usedEvents.ToArray()
            };

            Save.SaveDataAs(savePlace, data, saveName);
        }
    
        private void LoadData(string path)
        {
            path = Path.Combine(path, saveName);

            EventManagerSave[] data = Load.LoadData(path) as EventManagerSave[];
            timer = data[0].timer;

            for (int i = 0; i < data[0].availableEvents.Length; i++)
            {
                availableEvents.Add(data[0].availableEvents[i]);
            }

            for (int i = 0; i < data[0].usedEvents.Length; i++)
            {
                usedEvents.Enqueue(data[0].usedEvents[i]);      // does it work the right way? Or should it be from Length to 0??
            }
        }
        
        #endregion

        /// <summary>
        /// Works as timer to start the next event when the time runs out
        /// </summary>
        /// <returns>Restarts the coroutine to continue reducing the timer if it didn't run out</returns>
        private IEnumerator NextEventTimer()
        {
            timer -= updateTimerRate;
            if (timer <= endTimer)
            {
                NextEvent();                                                    //calls NextEvent() to choose the next event to be played
                StopAllCoroutines();                                            //Stops the timer
            }
            yield return new WaitForSeconds(updateTimerRate);
            StartCoroutine(NextEventTimer());
        }

        /// <summary>
        /// Randomly determines which event will be played next and empties and refills a list and a queue to ensure that every event will be played at least once
        /// </summary>
        private void NextEvent()
        {
            if (availableEvents.Count == resetEvents)                           //Refills the availableEvents list when empty
            {
                for (int i = usedEvents.Count; i > resetEvents; i--)
                {
                    availableEvents.Add(usedEvents.Peek());
                    usedEvents.Dequeue();
                }
            }

            sideMenuMileStoneButton.interactable = false;
            sideMenuPriorityButton.interactable = false;
            Time.timeScale = stopTime;
            System.Random random = new System.Random();
            int nextEventIndex = random.Next(0, availableEvents.Count - 1);

            activeEvent = availableEvents[nextEventIndex];                      //Acts as placeholder to give necessary information to EventBehaviourScriptable

            usedEvents.Enqueue(availableEvents[nextEventIndex]);
            availableEvents.RemoveAt(nextEventIndex);

            eventBehaviour.enabled = true;                                      //Acts as event behaviour start, since EventBehaviourScriptable has starting logic in OnEnable()
            SoundManager.PlaySound(SoundManager.Sound.EventEnters);
        }

        /// <summary>
        /// Resets the timer for the next event
        /// </summary>
        private void ResetTimer()
        {
            timer = setTimer;
            sideMenuMileStoneButton.interactable = true;
            sideMenuPriorityButton.interactable = true;
            Time.timeScale = startTime;
        }

        /// <summary>
        /// Starts the timer for the next event
        /// </summary>
        private void StartTimer()
        {
            StartCoroutine(NextEventTimer());
        }
    }
}
