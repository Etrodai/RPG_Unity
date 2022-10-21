using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Prototype.Eventsystem;

public class EventmanagerScriptable : MonoBehaviour
{
    //Event determination variables
    [SerializeField] private List<EventScriptableObject> availableEvents = new List<EventScriptableObject>();
    private Queue<EventScriptableObject> usedEvents = new Queue<EventScriptableObject>();
    private EventScriptableObject activeEvent;
    private const int resetEvents = 0;

    //Timer related variables
    private float timer;
    private float totalTime;
    private const float setTimer = 5f;
    private const float updateTimerRate = 0.5f;
    private const float endTimer = 0f;
    private const float stopTime = 0f;
    private const float startTime = 1f;

    //EventBehaviour related variables
    private bool textIsActive;
    [SerializeField] private GameObject eventUI;
    [SerializeField] private TextMeshProUGUI eventTitle;
    [SerializeField] private TextMeshProUGUI eventText;
    [SerializeField] private GameObject decision1Button;
    [SerializeField] private GameObject decision2Button;
    [SerializeField] private TextMeshProUGUI decision1ButtonText;
    [SerializeField] private TextMeshProUGUI decision2ButtonText;
    private int textIndex;
    private const int resetTextIndex = 0;
    private bool endofEvent;
    private const int decision1Index = 0;
    private const int decision2Index = 1;
    private bool buttonIsActive;

    private int debugIndex = 0;
    private void Awake()
    {
        timer = setTimer;
    }

    private void Start()
    {
        eventUI.SetActive(false);
        StartCoroutine(NextEventTimer());
    }

    private IEnumerator NextEventTimer()
    {
        timer -= updateTimerRate;
        if (timer <= endTimer)
        {
            textIsActive = true;            //All have to go to the end logic of eventBehaviour
            textIndex = resetTextIndex;

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

        eventUI.SetActive(true);

        Time.timeScale = 0f;
        System.Random random = new System.Random();
        int nextEventIndex = random.Next(0, availableEvents.Count - 1);

        activeEvent = availableEvents[nextEventIndex];
        eventTitle.text = activeEvent.EventTitle;

        usedEvents.Enqueue(availableEvents[nextEventIndex]);
        availableEvents.RemoveAt(nextEventIndex);

        textIsActive = true;
        EventBehaviour(); //EventBehaviour enablen

        debugIndex++;
    }

    private void ResetTimer()
    {
        timer = setTimer;
        Time.timeScale = 1f;
    }
}
