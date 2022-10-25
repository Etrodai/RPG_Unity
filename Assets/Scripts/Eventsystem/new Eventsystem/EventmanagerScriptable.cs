using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Prototype.Eventsystem;
using System;

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
    private const float setTimer = 5f;
    private const float updateTimerRate = 0.5f;
    private const float endTimer = 0f;
    private const float stopTime = 0f;
    private const float startTime = 1f;

    private void Awake()
    {
        timer = setTimer;
    }

    private void Start()
    {
        StartTimer();
        resetEventTimer += ResetTimer;
        resetEventTimer += StartTimer;                                      //Adding those methods to enable calling them at the end of the event in EventBehaviourScriptable
    }

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

        Time.timeScale = stopTime;
        System.Random random = new System.Random();
        int nextEventIndex = random.Next(0, availableEvents.Count - 1);

        activeEvent = availableEvents[nextEventIndex];                      //Acts as placeholder to give necessary information to EventBehaviourScriptable

        usedEvents.Enqueue(availableEvents[nextEventIndex]);
        availableEvents.RemoveAt(nextEventIndex);

        eventBehaviour.enabled = true;                                      //Acts as event behaviour start, since EventBehaviourScriptable has starting logic in OnEnable()
    }

    /// <summary>
    /// Resets the timer for the next event
    /// </summary>
    private void ResetTimer()
    {
        timer = setTimer;
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
