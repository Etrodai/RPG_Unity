using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private Action action;

    private float timer; //Time until next event will start / Should be randomized in order to make the events appear a bit more dynamic
    private float totalTime; //Total time elapsed since starting the game

    private List<MonoBehaviour> allEvents = new List<MonoBehaviour>();
    private List<int> availableEvents = new List<int>();
    private Queue<int> playedEvents = new Queue<int>();

    private void Start()
    {
        timer = 300f; //Must later be changed, so the timer will be saved, to continue at the right time after loading 
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            TriggerNextEvent();
            timer = 300f;
        }
    }

    private void TriggerNextEvent()
    {
        //int nextEvent = (int)UnityEngine.Random.Range(1f, availableEvents.Count);
        System.Random random = new System.Random();
        int nextEvent = random.Next(1, availableEvents.Count);

        /*action += allEvents[nextEvent]*/ //in BaseEvent noch abstract method einbauen und dann hier adden;
    }
}
