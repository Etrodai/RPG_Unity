using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Prototype.Eventsystem;
using System;

public class EventmanagerScriptable : MonoBehaviour
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

    private int debugIndex = 0;
    private void Awake()
    {
        timer = setTimer;
    }

    private void Start()
    {
        StartTimer();
        resetEventTimer += ResetTimer;
        resetEventTimer += StartTimer;
    }

    private IEnumerator NextEventTimer()
    {
        timer -= updateTimerRate;
        if (timer <= endTimer)
        {
            NextEvent();
            StopAllCoroutines();
        }
        yield return new WaitForSeconds(updateTimerRate);
        StartCoroutine(NextEventTimer());
    }

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

        Debug.Log(debugIndex);

        Time.timeScale = stopTime;
        System.Random random = new System.Random();
        int nextEventIndex = random.Next(0, availableEvents.Count - 1);

        activeEvent = availableEvents[nextEventIndex];

        usedEvents.Enqueue(availableEvents[nextEventIndex]);
        availableEvents.RemoveAt(nextEventIndex);

        eventBehaviour.enabled = true;

        debugIndex++;
    }

    private void ResetTimer()
    {
        timer = setTimer;
        Time.timeScale = startTime;
    }

    private void StartTimer()
    {
        StartCoroutine(NextEventTimer());
    }
}
