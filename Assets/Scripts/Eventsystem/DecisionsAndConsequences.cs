using Buildings;
using ResourceManagement;
using UnityEngine;

namespace Eventsystem
{
    [System.Serializable] //Made Robin
    public struct DecisionsAndConsequences
    {
        [TextArea] public string decisionButtonText;
        public EventScriptableObject consequenceEvent;
        public Resource[] consequenceOnResources;
        public Building[] consequenceOnBuildings;
        [TextArea] public string consequenceText;
    }
}