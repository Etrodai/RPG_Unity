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
        public string[] usedEventTitles;
    }
    
    public class EventManagerScriptable : MonoBehaviour
    {
        #region Variables

        //Event determination variables
        [SerializeField] private List<EventScriptableObject> availableEvents = new List<EventScriptableObject>();
        private Queue<EventScriptableObject> usedEvents = new Queue<EventScriptableObject>();
        private EventScriptableObject activeEvent;
        [SerializeField] private EventBehaviourScriptable eventBehaviour;
        private const int resetEvents = 0;
        private Action resetEventTimer;

        //Timer related variables
        private float timer;
        private float totalTime;
        [SerializeField, Tooltip("Minimum Time in seconds a new event can happen")] private float setMinimumTimer;
        [SerializeField, Tooltip("Maximum Time in seconds a new event can happen")] private float setMaximumTimer;
        private const float updateTimerRate = 0.5f;
        private const float endTimer = 0f;
        private const float stopTime = 0f;
        private const float startTime = 1f;
        
        [SerializeField] private Button sideMenuMileStoneButton;
        [SerializeField] private Button sideMenuPriorityButton;

        //Particle System variables
        private List<GameObject> eventParticles = new List<GameObject>();
        public List<GameObject> EventParticles
        {
            get => eventParticles;
            set { eventParticles = value; }
        }

        private void Awake()
        {
            UnityEngine.Random.InitState(DateTime.Now.Minute + DateTime.Now.Millisecond);
            timer = UnityEngine.Random.Range(setMinimumTimer, setMaximumTimer);
        }

        private void Start()
        {
            resetEventTimer += ResetTimerAndParticle;
            resetEventTimer += StartTimer;                                      //Adding those methods to enable calling them at the end of the event in EventBehaviourScriptable
            Save.OnSaveButtonClick.AddListener(SaveData);
            Save.OnSaveAsButtonClick.AddListener(SaveDataAs);
            Load.OnLoadButtonClick.AddListener(LoadData);
        }

        #endregion

        #region Save Load

        private void SaveData()
        {
            EventManagerSave[] data = new EventManagerSave[1];
            data[0] = new EventManagerSave();
            data[0].timer = timer;
 

            Save.AutoSaveData(data, SaveName);
        }
    
        private void SaveDataAs(string savePlace)
        {
            EventManagerSave[] data = new EventManagerSave[1];
            data[0] = new EventManagerSave();
            data[0].timer = timer;
            EventScriptableObject[] usedEventsArray = usedEvents.ToArray();
            for (int i = 0; i < usedEventsArray.Length; i++)
            {
                data[0].usedEventTitles[i] = usedEventsArray[i].EventTitle;
            }

            Save.SaveDataAs(savePlace, data, SaveName);
        }
    
        private void LoadData(string path)
        {
            path = Path.Combine(path, $"{SaveName}.dat");
            if (!File.Exists(path)) return;
            
            EventManagerSave[] data = Load.LoadData(path) as EventManagerSave[];
            
            // if (data == null) return;
            
            timer = data[0].timer;

            for (int i = 0; i < data[0].usedEventTitles.Length; i++)
            {
                for (int j = 0; j < availableEvents.Count; j++)
                {
                    if (availableEvents[j].EventTitle == data[0].usedEventTitles[i])
                    {
                        usedEvents.Enqueue(availableEvents[j]);             // does it work the right way? Or should it be from Length to 0??
                    }
                }
            }
        }
        
        #endregion

        #region Methodes

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

            switch (activeEvent.EventTitle)
            {
                case "Asteroiden Regen":
                    eventParticles[0].SetActive(true);
                    break;
                case "Kreuzende Wege":
                    eventParticles[1].SetActive(true);
                    break;
                case "Piraten Angriff":
                    eventParticles[2].SetActive(true);
                    break;
                default:
                    break;
            }
            eventBehaviour.enabled = true;                                      //Acts as event behaviour start, since EventBehaviourScriptable has starting logic in OnEnable()
            SoundManager.PlaySound(SoundManager.Sound.EventEnters);
        }

        /// <summary>
        /// Resets the timer for the next event
        /// </summary>
        private void ResetTimerAndParticle()
        {
            timer = UnityEngine.Random.Range(setMinimumTimer, setMaximumTimer);
            switch (activeEvent.EventTitle)
            {
                case "Asteroiden Regen":
                    eventParticles[0].SetActive(false);
                    break;
                case "Kreuzende Wege":
                    eventParticles[1].SetActive(false);
                    break;
                case "Piraten Angriff":
                    eventParticles[2].SetActive(false);
                    break;
                default:
                    break;
            }
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

        #endregion
    }
}
