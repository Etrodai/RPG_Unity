using Eventsystem;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendEventParticles : MonoBehaviour
{
    [SerializeField] private List<GameObject> particleSystems = new List<GameObject>();

    private EventManagerScriptable eventManager;

    private void Start()
    {
        eventManager = MainManagerSingleton.Instance.EventManager;

        for (int i = 0; i < particleSystems.Count; i++)
        {
            eventManager.EventParticles.Add(particleSystems[i]);
        }
    }
}
