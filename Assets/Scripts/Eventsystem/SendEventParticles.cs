using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Eventsystem
{
    public class SendEventParticles : MonoBehaviour //Made by Eric
    {
        [SerializeField] private List<GameObject> particleSystems = new();

        private EventManagerScriptable eventManager;

        private void Start()
        {
            eventManager = MainManagerSingleton.Instance.EventManager;

            for (int i = 0; i < particleSystems.Count; i++)                     //Adds the references of the different "particle" systems to the EventManager, so it can active the corresponding system with its corresponding event
            {
                eventManager.EventParticles.Add(particleSystems[i]);
            }
        }
    }
}
