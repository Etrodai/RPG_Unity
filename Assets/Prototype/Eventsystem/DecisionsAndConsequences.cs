using Buildings;
using ResourceManagement;
using UnityEngine;

namespace Prototype.Eventsystem
{
    [System.Serializable]
    public struct DecisionsAndConsequences
    {
        [TextArea] public string decisions;
        public EventScriptableObject consequenceEvent;
        public bool startNow;
        public Resource[] consequenceOnResources;
        public Building[] consequenceOnBuildings;
        [TextArea] public string consequenceText;
    }
}