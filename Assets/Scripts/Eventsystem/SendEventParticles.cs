using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Eventsystem
{
    public class SendEventParticles : MonoBehaviour
    {
        [SerializeField] private List<GameObject> particleSystems = new();

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
}
