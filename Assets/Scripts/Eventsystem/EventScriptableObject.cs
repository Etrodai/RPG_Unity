using UnityEngine;

namespace Eventsystem
{
    [CreateAssetMenu(fileName = "new Event", menuName = "Events")]
    public class EventScriptableObject : ScriptableObject //Made by Robin, adjustments made by Eric
    {
        [SerializeField] [TextArea] private string eventTitle;
        public string EventTitle => eventTitle;
        
        [SerializeField] [TextArea] private string[] eventText;
        public string[] EventText => eventText;


        [SerializeField] private DecisionsAndConsequences[] decisions;
        public DecisionsAndConsequences[] Decisions => decisions;
    }
}
