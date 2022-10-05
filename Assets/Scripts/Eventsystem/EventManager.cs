using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    public static EventManager Instance
    {
        get => instance;
    }

    private float timer;                                            //Time until next event will start / Should be randomized in order to make the events appear a bit more dynamic
    private float totalTime;                                        //Total time elapsed since starting the game
    private const float setTimer = 5f;

    private List<MonoBehaviour> allEvents = new List<MonoBehaviour>();
    [SerializeField]
    private List<BaseEvent> availableEvents = new List<BaseEvent>(); //Should probably all be Monobehaviour or BaseEvent CLass since int will only be used to determine index
    public List<BaseEvent> AvailableEvents
    {
        get => availableEvents;
        set { availableEvents = value; }
    }
    private Queue<BaseEvent> playedEvents = new Queue<BaseEvent>(); //Not necessary if we don't add them after a period of time (one at a time or all), since we could also use allEvents list
    private Action action;
    public Action Action
    {
        get => action;
    }

    private bool eventIsPlaying = false;
    private const int resetEvents = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        action += ResetTimer;
        timer = setTimer; //Must later be changed, so the timer will be saved, to continue at the right time after loading 
    }

    private void Update()
    {
        if (eventIsPlaying == false) //Kann wahrscheinlich mit Coroutine gelöst werden!
        {
            timer -= Time.deltaTime;
        }
        
        if(timer <= 0f)
        {
            TriggerNextEvent();
            timer = setTimer;
        }
    }

    private void TriggerNextEvent()
    {
        /*freeze timer by setting bool eventIsPlaying to true
         * new int for size of available Events
         * add behaviour to action
         * invoke action
         * remove behaviour from action
         * add used event to played events
         * remove event from available events
         * reset timer & contine timer condition (eventIsPlaying = false)
         * 
         * Theoretisch möglich hier die Instanz von EventManager an die Events zu übergeben um Singleton zu sparen
         */

        if (availableEvents.Count == resetEvents)                           //Refills the availableEvents list when empty
        {
            for (int i = playedEvents.Count; i > resetEvents; i--)
            {
                availableEvents.Add(playedEvents.Peek());
                playedEvents.Dequeue();
            }
        }

        eventIsPlaying = true;
        System.Random random = new System.Random();
        int nextEvent = random.Next(0, availableEvents.Count - 1);          //null check for the list is necessary in order to never run out and crash at this point

        availableEvents[nextEvent].enabled = true;                          //Activate the chosen Event script

        playedEvents.Enqueue(availableEvents[nextEvent]);                   //Add chosen Event to the playedEvents queue
        availableEvents.RemoveAt(nextEvent);                                //Remove chosen Event from the availableEvents list
    }

    /// <summary>
    /// Will be called via Action at the end of events to reset and restart the timer
    /// </summary>
    private void ResetTimer()
    {
        timer = setTimer;
        eventIsPlaying = false;
    }
}
