using System;
using System.Collections;
using System.Collections.Generic;
using SaveSystem;
using Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Eventsystem
{
    [Serializable]
    public struct EventManagerSave
    {
        public float timer;
        public string[] usedEventTitles;
    }
    
    public class EventManagerScriptable : MonoBehaviour
    {
        #region Variables

        //Event determination variables
        [SerializeField] private List<EventScriptableObject> availableEvents = new ();
        private readonly Queue<EventScriptableObject> usedEvents = new ();
        private EventScriptableObject activeEvent;
        [SerializeField] private EventBehaviourScriptable eventBehaviour;
        private const int ResetEvents = 0;
        private Action resetEventTimer;

        //Timer related variables
        private float timer;
        private float totalTime;
        [SerializeField, Tooltip("Minimum Time in seconds a new event can happen")] private float setMinimumTimer;
        [SerializeField, Tooltip("Maximum Time in seconds a new event can happen")] private float setMaximumTimer;
        private const float UpdateTimerRate = 0.5f;
        private const float EndTimer = 0f;
        private const float StopTime = 0f;
        private const float StartTime = 1f;
        
        [SerializeField] private Button sideMenuMileStoneButton;
        [SerializeField] private Button sideMenuPriorityButton;

        private SaveData saveData;
        [SerializeField] private List<GameObject> eventParticles = new();

        //Particle System variables

        #endregion
        
        #region Properties

        public List<GameObject> EventParticles => eventParticles;

        public Action ResetEventTimer
        {
            get => resetEventTimer;
            set => resetEventTimer = value;
        }

        public EventScriptableObject ActiveEvent => activeEvent;

        #endregion

        #region UnityEvents

        private void Awake()
        {
            UnityEngine.Random.InitState(DateTime.Now.Minute + DateTime.Now.Millisecond);
            timer = UnityEngine.Random.Range(setMinimumTimer, setMaximumTimer);
        }

        private void Start()
        {
            saveData = SaveSystem.SaveData.Instance;
            eventBehaviour.Initialize();
            resetEventTimer += StartTimer;                                      //Adding those methods to enable calling them at the end of the event in EventBehaviourScriptable
            Save.OnSaveButtonClick.AddListener(SaveData);
            Save.OnSaveAsButtonClick.AddListener(SaveDataAs);
            Load.OnLoadButtonClick.AddListener(LoadData);
            timer = UnityEngine.Random.Range(setMinimumTimer, setMaximumTimer);
            resetEventTimer.Invoke();
            resetEventTimer += ResetTimerAndParticle;
        }

        private void OnDestroy()
        {
            resetEventTimer -= ResetTimerAndParticle;
            resetEventTimer -= StartTimer;
        }

        #endregion

        #region Save Load

        private void SaveData()
        {
            EventManagerSave data = new();
            data.timer = timer;
            saveData.GameSave.eventData = data;
            // Save.AutoSaveData(data, SaveName);
        }
    
        private void SaveDataAs(string savePlace)
        {
            EventManagerSave data = new();
            data.timer = timer;
            EventScriptableObject[] usedEventsArray = usedEvents.ToArray();
            for (int i = 0; i < usedEventsArray.Length; i++)
            {
                data.usedEventTitles[i] = usedEventsArray[i].EventTitle;
            }

            saveData.GameSave.eventData = data;
        }
    
        private void LoadData(GameSave gameSave)
        {
            EventManagerSave data = gameSave.eventData;

            timer = data.timer;

            if (data.usedEventTitles == null) return;
            
            for (int i = 0; i < data.usedEventTitles.Length; i++)
            {
                for (int j = 0; j < availableEvents.Count; j++)
                {
                    if (availableEvents[j].EventTitle == data.usedEventTitles[i])
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
          // ReSharper disable once FunctionRecursiveOnAllPaths
          private IEnumerator NextEventTimer()
        {
            timer -= UpdateTimerRate;
            if (timer <= EndTimer)
            {
                NextEvent();                                                    //calls NextEvent() to choose the next event to be played
                StopAllCoroutines();                                            //Stops the timer
            }
            yield return new WaitForSeconds(UpdateTimerRate);
            StartCoroutine(NextEventTimer());
        }

        /// <summary>
        /// Randomly determines which event will be played next and empties and refills a list and a queue to ensure that every event will be played at least once
        /// </summary>
        private void NextEvent()
        {
            if (availableEvents.Count == ResetEvents)                           //Refills the availableEvents list when empty
            {
                for (int i = usedEvents.Count; i > ResetEvents; i--)
                {
                    availableEvents.Add(usedEvents.Peek());
                    usedEvents.Dequeue();
                }
            }

            sideMenuMileStoneButton.interactable = false;
            sideMenuPriorityButton.interactable = false;
            Time.timeScale = StopTime;
            System.Random random = new System.Random();
            int nextEventIndex = random.Next(0, availableEvents.Count - 1);

            activeEvent = availableEvents[nextEventIndex];                      //Acts as placeholder to give necessary information to EventBehaviourScriptable

            usedEvents.Enqueue(availableEvents[nextEventIndex]);
            availableEvents.RemoveAt(nextEventIndex);

            // switch (activeEvent.EventTitle)
            // {
            //     case "Asteroiden Regen":
            //         eventParticles[0].SetActive(true);
            //         break;
            //     case "Kreuzende Wege":
            //         eventParticles[1].SetActive(true);
            //         break;
            //     case "Piraten Angriff":
            //         eventParticles[2].SetActive(true);
            //         break;
            //     default:
            //         break;
            // }
            eventBehaviour.enabled = true;                                      //Acts as event behaviour start, since EventBehaviourScriptable has starting logic in OnEnable()
            SoundManager.PlaySound(SoundManager.Sound.EventEnters);
        }

        /// <summary>
        /// Resets the timer for the next event
        /// </summary>
        private void ResetTimerAndParticle()
        {
            timer = UnityEngine.Random.Range(setMinimumTimer, setMaximumTimer);
            // switch (activeEvent.EventTitle)
            // {
            //     case "Asteroiden Regen":
            //         eventParticles[0].SetActive(false);
            //         break;
            //     case "Kreuzende Wege":
            //         eventParticles[1].SetActive(false);
            //         break;
            //     case "Piraten Angriff":
            //         eventParticles[2].SetActive(false);
            //         break;
            //     default:
            //         break;
            // }
            sideMenuMileStoneButton.interactable = true;
            sideMenuPriorityButton.interactable = true;
            Time.timeScale = StartTime;
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
